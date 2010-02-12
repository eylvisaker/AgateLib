/*
 * XMLFormatter class
 * 
 * This class was originally downloaded from 
 * http://www.ecodebank.com/details/?catid=2&catsubid=37&nid=680
 * 
 * The original code was published in Dutch in a magazine, available at
 * http://download.microsoft.com/download/e/b/9/eb9b8f51-f644-485f-a02d-85b46c2f5af7/p24-27_1%2023.pdf
 * 
 * The code implements an IFormatter object which serializes
 * objects to XML.  The original code had several serious bugs, 
 * especially when it came to serializing objects which are marked
 * with the SerializableAttribute but do not implement ISerializable.
 * The original code could not handle generic container
 * classes, or circular references in the object graph.
 * 
 * The following changes and bug fixes have been implemented by
 * Erik R. Ylvisaker <eylvisaker@gmail.com>
 *
 * Enough custom serialization has been written to properly
 * serialize List<T> and Dictionary<TKey,TValue>, ArrayList and 
 * jagged and multidimensional arrays in a way that is independent of
 * their internal representation, so they should be able to be serialized
 * in .NET and deserialized in another runtime (Mono, for example).
 * 
 * This should work on objects which implement ISerializable as well as objects
 * which are just marked with SerializableAttribute.
 * 
 * XmlFormatter also properly serializes and deserializes private members
 * of base classes of objects to be serialized, and compiler generated 
 * backing fields for automatically implemented properties (C# 3.0)
 * 
 * Objects appearing more than once in the object graph are only 
 * serialized/deserialized once.  This will correctly handle circular references
 * in the object graph.
 * 
 * XMLFormatter should function as a drop-in replacement for BinaryFormatter.
 * If XMLFormatter fails to serialize any object graph which BinaryFormatter 
 * can serialize, please notify me with an example.
 * 
 * The only license text at ecodebank.com states only 
 * "The code is free to use in your projects."
 * It appears that this means the code may be freely distributed and 
 * used without restriction.  
 * */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Collections;

namespace AgateLib.Serialization.Formatters.Xml
{
#if !XNA
	/// <summary>
	/// The <see cref="XmlFormatter"/> class implements a custom XmlFormatter
	/// which uses the <see cref="ISerializable"/> interace. 
	/// The class implements the <see cref="IFormatter"/> interface to serialize
	/// and deserialize the object to an XML representation.
	/// </summary>
	/// <remarks>
	/// The class calls the methods of ISerializable on the object if the object
	/// supports this interface. If not, the class will use Reflection to examine the public
	/// fields and properties of the object.<br/>
	/// When adding objects that inherit or implement IList, ICollection, the 
	/// elements of the list should be passed as an array to <see cref="SerializationInfo"/>.
	/// </remarks>
	public sealed class XmlFormatter : IFormatter
	{

		#region --- Class members ---

		/// <summary>
		/// The context in which this formatter performs his work.
		/// </summary>
		private StreamingContext context = new StreamingContext(StreamingContextStates.Persistence);
		/// <summary>
		/// The serialization binder used to map object types to names.
		/// </summary>
		private SerializationBinder binder;
		/// <summary>
		/// The surrogate selector to select the object that will perform the serialization.
		/// </summary>
		private ISurrogateSelector selector = null;
		/// <summary>
		/// Contains a list with objects that implement the IDeserializationCallback interface.
		/// </summary>
		private List<object> deserializationCallbackList;

		Dictionary<object, int> ObjectID;
		Dictionary<int, object> IDObject;
		Dictionary<int, object> uninitObject;

		List<SetFieldCallback> setFields;

		int lastKey = 0;

		#endregion
		#region --- Properties ---

		#region --- IFormatter Members ---

		/// <summary>
		/// Gets or sets the type binder.
		/// </summary>
		public SerializationBinder Binder
		{
			get
			{
				if (binder == null)
				{
					binder = new CustomBinder();
				}
				return binder;
			}
			set
			{
				binder = value;
			}
		}

		/// <summary>
		/// Gets or sets the StreamingContext.
		/// </summary>
		public StreamingContext Context
		{
			get
			{
				return context;
			}
			set
			{
				context = value;
			}
		}

		/// <summary>
		/// Gets or sets the SurrogateSelector.
		/// </summary>
		public ISurrogateSelector SurrogateSelector
		{
			get
			{
				return selector;
			}
			set
			{
				selector = value;
			}
		}

		#endregion

		private List<object> DeserializationCallBackList
		{
			get
			{
				if (deserializationCallbackList == null)
				{
					deserializationCallbackList = new List<object>();
				}
				return deserializationCallbackList;
			}
		}

		#endregion
		#region --- Constructors ---

		/// <summary>
		/// Default constructor does nothing.
		/// </summary>
		public XmlFormatter()
		{
		}

		#endregion
		#region --- Public Methods ---


		/// <summary>
		/// Serializes the passed object to the passed stream.
		/// </summary>
		/// <param name="serializationStream">The stream to serialize to.</param>
		/// <param name="objectToSerialize">The object to serialize.</param>
		public void Serialize(Stream serializationStream, object objectToSerialize)
		{
			if (objectToSerialize == null)
				return;

			if (serializationStream == null)
				throw new ArgumentException("Empty serializationStream!");

			ObjectID = new Dictionary<object, int>();

			XmlTextWriter writer = new XmlTextWriter(serializationStream, Encoding.UTF8);
			writer.WriteStartDocument(true);

			Serialize(writer, new FormatterConverter(), objectToSerialize.GetType().Name, objectToSerialize, objectToSerialize.GetType());

			writer.WriteEndDocument();
		}

		/// <summary>
		/// Deserializes an object from the passed stream.
		/// </summary>
		/// <param name="serializationStream">The stream to deserialize the object from.</param>
		/// <returns>The deserialized object.</returns>
		public object Deserialize(Stream serializationStream)
		{
			IDObject = new Dictionary<int, object>();
			uninitObject = new Dictionary<int, object>();
			setFields = new List<SetFieldCallback>();

			//if (_type == null)
			//    throw new InvalidOperationException ("Type property not initialized");

			return Deserialize(serializationStream, null);
		}


		#endregion
		#region --- Private Methods ---

		#region Serialization

		/// <summary>
		/// Serializes the object using the passed XmlWriter.
		/// </summary>
		/// <param name="writer">The XmlWriter to write to.</param>
		/// <param name="converter">The converter to use when converting simple types.</param>
		/// <param name="elementName">The name of the element in the Xml.</param>
		/// <param name="objectToSerialize">The object to serialize.</param>
		/// <param name="objectType">The type of the object.</param>
		private void Serialize(XmlTextWriter writer, FormatterConverter converter, string elementName, object objectToSerialize, Type objectType)
		{
			// do custom serializations for primitives and library types.
			if (objectType.IsPrimitive || objectType == typeof(string))
			{
				CustomSerializationEntry newEntry = new CustomSerializationEntry(elementName, objectToSerialize.GetType(), objectToSerialize);

				// write the element
				WriteValueElement(elementName, writer, newEntry);

				return;
			}

			// check to see if we have a reference type where the object has already been serialized
			// and refer to the ID table then.
			if (objectToSerialize != null && objectToSerialize.GetType().IsValueType == false)
			{
				if (ObjectID.ContainsKey(objectToSerialize))
				{
					int key = ObjectID[objectToSerialize];

					WriteStartElement(writer, elementName);

					writer.WriteStartAttribute("id");
					writer.WriteString(key.ToString());
					writer.WriteEndAttribute();

					writer.WriteEndElement();

					return;
				}
				else
				{
					lastKey++;

					ObjectID.Add(objectToSerialize, lastKey);
				}
			}

			// Include type information when using ISerializable.
			bool includeTypeInfo = (objectToSerialize is ISerializable);

			WriteStartElement(writer, elementName);

			if (objectToSerialize != null && objectToSerialize.GetType().IsValueType == false)
			{
				WriteIDAttribute(writer, ObjectID[objectToSerialize]);
			}

			if (objectType.IsArray)
			{
				SerializeArray(writer, converter, (System.Array)objectToSerialize, objectType.GetElementType());
			}
			else if (IsTypeGenericDictionary(objectType))
			{
				SerializeDictionary(writer, converter, (IDictionary)objectToSerialize);
			}
			else
			{
				// Write out the type information
				WriteAttributes(writer, objectToSerialize);

				// for each serializable item in this object
				foreach (CustomSerializationEntry entry in GetMemberInfo(objectToSerialize, objectType, converter))
				{
					if (entry.ObjectType.IsPrimitive || entry.ObjectType == typeof(string) ||
						entry.ObjectType.IsEnum || entry.ObjectType == typeof(DateTime))
					{
						// simple type, directly write the value.
						WriteValueElement(writer, entry);
					}
					else if (entry.ObjectType.IsArray)
					{
						// the type is an array type. iterate through the members
						// get the type of the elements in the array
						Type enumeratedType = entry.ObjectType.GetElementType();
						// write the opening tag.
						WriteStartElement(writer, entry.Name);

						// write the array out to the xml, if it's not already been done.
						if (ObjectID.ContainsKey(entry.Value))
						{
							WriteIDAttribute(writer, ObjectID[entry.Value]);
						}
						else
						{
							// write the ID for the array
							lastKey++;
							ObjectID.Add(entry.Value, lastKey);
							WriteIDAttribute(writer, lastKey);

							SerializeArray(writer, converter, (System.Array)entry.Value, enumeratedType);
						}

						// write closing tag
						writer.WriteEndElement();
					}
					else
					{
						switch (entry.CollectionType)
						{
							case CollectionType.ArrayList:
							case CollectionType.List:

								// write the opening tag
								WriteStartElement(writer, entry.Name);

								WriteAttributes(writer, entry.Value);

								if (ObjectID.ContainsKey(entry.Value))
								{
									WriteIDAttribute(writer, ObjectID[entry.Value]);
								}
								else
								{
									// write the ID for the array
									lastKey++;
									ObjectID.Add(entry.Value, lastKey);
									WriteIDAttribute(writer, lastKey);


									IList items = (IList)entry.Value;

									// loop through the list
									foreach (object item in items)
									{
										// serialize the object (recursive call)
										Serialize(writer, converter, "Item", item, item.GetType());
									}
								}

								// write the closing tag
								writer.WriteEndElement();

								break;

							case CollectionType.Dictionary:

								WriteStartElement(writer, entry.Name);

								if (ObjectID.ContainsKey(entry.Value))
								{
									WriteIDAttribute(writer, ObjectID[entry.Value]);
								}
								else
								{
									// write the ID for the array
									lastKey++;
									ObjectID.Add(entry.Value, lastKey);
									WriteIDAttribute(writer, lastKey);

									SerializeDictionary(writer, converter, (IDictionary)entry.Value);
								}

								writer.WriteEndElement();

								break;


							default:
							case CollectionType.None:
								// serialize the object (recursive call)
								Serialize(writer, converter, entry.Name, entry.Value, entry.ObjectType);
								break;

						}

					}
				}
			}
			// write the closing tag
			writer.WriteEndElement();
			// flush the contents of the writer to the stream
			writer.Flush();
		}

		private void SerializeDictionary(XmlTextWriter writer, FormatterConverter converter, IDictionary dictionary)
		{

			WriteAttributes(writer, dictionary);

			// serialize the Comparer object.
			PropertyInfo comparerField = dictionary.GetType().GetProperty("Comparer",
				 BindingFlags.Instance | BindingFlags.Public);

			object comparer = comparerField.GetValue(dictionary, null);

			Serialize(writer, converter, "Comparer", comparer, comparer.GetType());

			// serialize the key/value pairs in the dictionary.
			foreach (object kvp in dictionary)
			{
				if (kvp is DictionaryEntry == false)
					throw new SerializationException("Dictionary enumeration expected object of type " +
						"System.Collections.DictionaryEntry, but returned object of type " +
						kvp.GetType().FullName);

				DictionaryEntry dentry = (DictionaryEntry)kvp;
				Type keyType = dentry.Key.GetType();
				Type valueType = dentry.Value.GetType();

				if (keyType.IsPrimitive || keyType == typeof(string) || keyType.IsEnum)
				{
					CustomSerializationEntry key = new CustomSerializationEntry("Key", keyType, dentry.Key);

					WriteValueElement(writer, key);
				}
				else
					Serialize(writer, converter, "Key", dentry.Key, dentry.Key.GetType());

				if (valueType.IsPrimitive || valueType == typeof(string) || valueType.IsEnum)
				{
					CustomSerializationEntry value = new CustomSerializationEntry("Value", valueType, dentry.Value);

					WriteValueElement(writer, value);
				}
				else
					Serialize(writer, converter, "Value", dentry.Value, dentry.Value.GetType());
			}
		}

		private void WriteIDAttribute(XmlTextWriter writer, int id)
		{
			writer.WriteStartAttribute("id");
			writer.WriteString(id.ToString());
			writer.WriteEndAttribute();
		}

		private static void WriteStartElement(XmlTextWriter writer, string elementName)
		{
			if (elementName.Contains("<"))
			{
				// hack to serialize properties which have compiler generated backing fields
				writer.WriteStartElement("Field");

				writer.WriteStartAttribute("name");
				writer.WriteString(elementName);
				writer.WriteEndAttribute();
			}
			else
			{
				// write the opening tag
				writer.WriteStartElement(elementName);
			}
		}

		/// <summary>
		/// Serializes an array.  This does not write start or end tags, that is the responsibility of the caller.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="converter"></param>
		/// <param name="array"></param>
		/// <param name="enumeratedType"></param>
		private void SerializeArray(XmlTextWriter writer, FormatterConverter converter, System.Array array, Type enumeratedType)
		{
			// write the attributes.
			WriteAttributes(writer, array);

			// if the array is an null value, skip loop
			if (array != null)
			{
				foreach (object item in array)
				{
					if (enumeratedType.IsPrimitive || enumeratedType == typeof(string))
					{
						Type itemType = (item != null) ? item.GetType() : null;
						CustomSerializationEntry newEntry = new CustomSerializationEntry("Value", itemType, item);

						// write the element
						WriteValueElement("Item", writer, newEntry);
					}
					else if (item == null)
					{
						CustomSerializationEntry newEntry = new CustomSerializationEntry("Value", null, null);

						WriteValueElement("Item", writer, newEntry);
					}
					else
					{
						// serialize the object (recursive call)
						Serialize(writer, converter, "Item", item, item.GetType());
					}
				}
			}
		}

		/// <summary>
		/// Writes the Type and includeArrayAttribute attributes to the element
		/// </summary>
		/// <param name="writer">The XmlWriter to write to.</param>
		/// <param name="theObject">The object whose attributes are to be written.</param>
		private void WriteAttributes(XmlTextWriter writer, object theObject)
		{
			WriteAttributes(writer, theObject, null);
		}
		private void WriteAttributes(XmlTextWriter writer, object theObject, Type reflectedType)
		{
			if (theObject == null)
			{
				writer.WriteStartAttribute("null");
				writer.WriteString("true");
				writer.WriteEndAttribute();

				return;
			}

			Type objectType = theObject.GetType();
			Type elementType = objectType;

			if (objectType.IsArray)
			{
				elementType = objectType.GetElementType();
			}

			writer.WriteStartAttribute("type");
			writer.WriteString(elementType.FullName);
			writer.WriteEndAttribute();


			if (objectType.IsArray)
			{
				writer.WriteStartAttribute("isArray");
				writer.WriteString("true");
				writer.WriteEndAttribute();

				if (objectType.GetArrayRank() != 1)
				{
					Array array = (Array)theObject;

					writer.WriteStartAttribute("rank");
					writer.WriteString(objectType.GetArrayRank().ToString());
					writer.WriteEndAttribute();

					for (int i = 0; i < objectType.GetArrayRank(); i++)
					{
						writer.WriteStartAttribute("bound" + i.ToString());
						writer.WriteString((array.GetUpperBound(i) + 1).ToString());
						writer.WriteEndAttribute();
					}
				}
			}

			if (reflectedType != null)
			{
				writer.WriteStartAttribute("reflectedType");
				writer.WriteString(reflectedType.FullName);
				writer.WriteEndAttribute();
			}

			/*
			if (objectType.IsGenericType)
			{
				Type[] parameters = objectType.GetGenericArguments();

				writer.WriteStartAttribute("args");

				for (int i = 0; i < parameters.Length; i++)
					writer.WriteString(parameters[i].FullName);

				writer.WriteEndAttribute();
			}
			*/
		}

		/// <summary>
		/// Writes a simple element to the writer. The name of the element is the name of the object type.
		/// </summary>
		/// <param name="writer">The XmlWriter to write to.</param>
		/// <param name="entry">The entry to write to the element.</param>
		private void WriteValueElement(XmlTextWriter writer, CustomSerializationEntry entry)
		{
			WriteValueElement(entry.Name, writer, entry);
		}

		/// <summary>
		/// Writes a simple element to the writer. 
		/// </summary>
		/// <param name="tagName">The name of the tag to write.</param>
		/// <param name="writer">The XmlWriter to write to.</param>
		/// <param name="entry">The entry to write to the element.</param>
		private void WriteValueElement(string tagName, XmlTextWriter writer, CustomSerializationEntry entry)
		{
			// write opening tag
			WriteStartElement(writer, tagName);

			WriteAttributes(writer, entry.Value, entry.ReflectedType);
			//writer.WriteAttributeString("type", entry.ObjectType.FullName);

			if (entry.Value == null)
			{
				// no need to do anything else in this case.
				// this is possible for string types.
			}
			else if (entry.ObjectType.IsEnum)
			{
				writer.WriteValue(((int)entry.Value).ToString());
			}
			else if (entry.ObjectType == typeof(string) || entry.ObjectType == typeof(char))
			{
				// todo: refactor this somehow
				string outputValue = entry.Value.ToString();

				outputValue = outputValue.Replace("\\", "\\\\");
				outputValue = outputValue.Replace("\n", "\\n");
				outputValue = outputValue.Replace("\r", "\\r");
				outputValue = outputValue.Replace("\t", "\\t");

				writer.WriteValue(outputValue);
			}
			else
			{
				writer.WriteValue(entry.Value.ToString());
			}

			// write closing tag
			writer.WriteEndElement();
		}

		/// <summary>
		/// Gets all the serializable members of an object and return an enumarable collection.
		/// </summary>
		/// <param name="objectToSerialize">The object to get the members from.</param>
		/// <param name="objectToSerializeType">The type of the object.</param>
		/// <param name="converter">The converter to use when converting simple types.</param>
		/// <returns>An IEnumerable list of <see cref="CustomSerializationEntry"/> entries.</returns>
		private IEnumerable<CustomSerializationEntry> GetMemberInfo(object objectToSerialize, Type objectToSerializeType, FormatterConverter converter)
		{
			ISurrogateSelector selector1 = null;
			ISerializationSurrogate serializationSurrogate = null;
			SerializationInfo info = null;
			Type objectType = objectToSerializeType;

			// if the passed object is null, break the iteration.
			if (objectToSerialize == null)
				yield break;

			if (IsTypeGenericDictionary(objectToSerialize.GetType()))
			{
				throw new SerializationException("GetMemberInfo should not be called on Dictionary.");
			}

			if ((SurrogateSelector != null) && ((serializationSurrogate = SurrogateSelector.GetSurrogate(objectType, Context, out selector1)) != null))
			{
				// use a surrogate to get the members.
				info = new SerializationInfo(objectType, converter);

				if (!objectType.IsPrimitive)
				{
					// get the data from the surrogate.
					serializationSurrogate.GetObjectData(objectToSerialize, info, Context);
				}
			}
			else if (objectToSerialize is ISerializable)
			{
				// object implements ISerializable
				info = new SerializationInfo(objectType, converter);
				// get the data using ISerializable.
				((ISerializable)objectToSerialize).GetObjectData(info, Context);
			}

			if (info != null)
			{
				// either the surrogate provided the members, or the members were retrieved 
				// using ISerializable.
				// create the custom entries collection by copying all the members
				foreach (SerializationEntry member in info)
				{
					Type valueType = typeof(Object);

					if (member.Value != null)
						valueType = member.Value.GetType();

					CustomSerializationEntry entry = new CustomSerializationEntry(member.Name, valueType, member.Value);
					// yield return will return the entry now and return to this point when
					// the enclosing loop (the one that contains the call to this method)
					// request the next item.
					yield return entry;
				}
			}
			else
			{
				// The item does not hava surrogate, not does it implement ISerializable, or
				// it is of type Dictionary<TKey, TValue>
				// We use reflection to get the objects state.
				if (objectType.IsSerializable == false)
				{
					throw new SerializationException(string.Format("Type is not serializable : {0}.", objectType.FullName));
				}
				// Get all serializable members
				MemberInfo[] members = FormatterServices.GetSerializableMembers(objectType, Context);

				foreach (MemberInfo member in members)
				{
					// get the type
					FieldInfo field = member.DeclaringType.GetField(member.Name, BindingFlags.Public | BindingFlags.NonPublic |
																				 BindingFlags.Instance);

					string name = member.Name;

					if (name.Contains("+"))
					{
						name = name.Substring(name.IndexOf("+") + 1);

						if (name.Contains("+"))
							throw new SerializationException("Something is wrong here.");
					}

					// create the entry
					CustomSerializationEntry entry = new CustomSerializationEntry(name, null);

					// get the value of the member
					entry.Value = GetMemberValue(objectToSerialize, member);

					if (entry.Value == null)
						entry.ObjectType = typeof(Object);
					else
						entry.ObjectType = entry.Value.GetType();

					entry.DetectListType();

					if (member.ReflectedType != objectType)
					{
						entry.ReflectedType = member.ReflectedType;
					}

					// yield return will return the entry now and return to this point when
					// the enclosing loop (the one that contains the call to this method)
					// request the next item.
					yield return entry;
				}

			}
		}

		/// <summary>
		/// Determines if the passed member is public and writable.
		/// </summary>
		/// <param name="member">The member to investigate.</param>
		/// <returns>True if public and writable, False if not.</returns>
		private bool CanSerialize(MemberInfo member)
		{
			if (member.MemberType == MemberTypes.Field)
			{
				return true;
			}
			else if (member.MemberType == MemberTypes.Property)
			{
				// if the member is a property, the member is writable when public set methods exist.
				PropertyInfo property = member as PropertyInfo;

				if (property.CanRead && property.CanWrite && property.GetGetMethod().IsPublic && !property.GetGetMethod().IsStatic && property.GetSetMethod().IsPublic && !property.GetSetMethod().IsStatic)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Get the value of a member.
		/// </summary>
		/// <param name="item">The item to get the member from.</param>
		/// <param name="member">The member to get the value from.</param>
		/// <returns>The value of the member.</returns>
		private object GetMemberValue(object item, MemberInfo member)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Field:
					string name = member.Name;

					if (name.Contains("+"))
					{
						name = name.Substring(name.IndexOf('+') + 1);
					}

					FieldInfo field = member.DeclaringType.GetField(name,
						BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance);

					return field.GetValue(item);

				default:
					{
						throw new NotSupportedException(string.Format("Member cannot be serialized. {0}", member.MemberType.ToString()));
					}
			}
		}

		#endregion

		#region Deserialization

		/// <summary>
		/// Deserializes an object from the given stream for the given type.
		/// </summary>
		/// <param name="serializationStream">The stream to read the object from.</param>
		/// <param name="objectType">The type of object to create.</param>
		/// <returns>The deserialized object.</returns>
		private object Deserialize(Stream serializationStream, Type objectType)
		{
			//if (_type == null) 
			//    _type = objectType;

			object deserialized = null;

			// create xml reader from stream
			using (XmlTextReader reader = new XmlTextReader(serializationStream))
			{

				DeserializationCallBackList.Clear();

				// for some reason, XmlTextReader will throw a blank while it's on the 
				// document tag, but if you call ReadStartElement, it will skip the
				// first element in the document!  A call to IsStartElement seems to
				// have a side-effect to get around this.
				reader.IsStartElement();

				deserialized = InitializeObject(reader, new FormatterConverter(), objectType);
			}

			foreach (IDeserializationCallback callBack in DeserializationCallBackList)
			{
				callBack.OnDeserialization(null);
			}

			// this is to deal with circular references in the object graph
			foreach (SetFieldCallback fieldCallback in setFields)
			{
				fieldCallback.Execute(this);
			}

			return deserialized;
		}

		/// <summary>
		/// Reads an object from the XML and initializes the object.
		/// </summary>
		/// <param name="reader">The XmlReader to read from.</param>
		/// <param name="converter">The converter used to parse the values from the XML.</param>
		/// <param name="objectType">The type of the object to create.</param>
		/// <returns>The recreated object.</returns>
		private object InitializeObject(XmlTextReader reader, FormatterConverter converter, Type objectType)
		{
			Type actualType;
			ISurrogateSelector selector1 = null;
			ISerializationSurrogate serializationSurrogate = null;
			SerializationInfo info = null;
			object initializedObject = null;
			bool isArray;
			int id = 0;
			bool skipDeserializationCallback = false;
			List<SetFieldCallback> fieldCallbacks = new List<SetFieldCallback>();

			// check if a type attribute is present
			if (reader.HasAttributes && objectType != typeof(TimeSpan))
			{
				// if so, get the type
				string actualTypeName = reader.GetAttribute("type");
				isArray = string.IsNullOrEmpty(reader.GetAttribute("isArray")) ? false : true;

				int.TryParse(reader.GetAttribute("id"), out id);

				// check to see if this field is just an id
				if (string.IsNullOrEmpty(actualTypeName) && id > 0)
				{
					return IDObject[id];
				}

				actualType = Binder.BindToType("", actualTypeName);
			}
			else
			{
				// passed type is actual type.
				actualType = objectType;

				isArray = actualType.IsArray;
			}

			// check first to see if we are deserializing an array
			if (isArray)
			{
				AddUninitObject(id, null);

				initializedObject = DeserializeArray(reader, converter, actualType);

				AddInitializedObject(id, initializedObject);
			}
			// check whether a surrogate should be used, iserializable is implemented or reflection is needed.
			else if ((SurrogateSelector != null) && ((serializationSurrogate = SurrogateSelector.GetSurrogate(actualType, Context, out selector1)) != null))
			{
				// use surrogate
				info = new SerializationInfo(actualType, converter);

				if (!actualType.IsPrimitive)
				{
					// create a instance of the type.
					initializedObject = FormatterServices.GetUninitializedObject(actualType);

					// read the first element
					reader.ReadStartElement();

					while (reader.IsStartElement())
					{
						// determine type
						string typeName = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
						Type type = Binder.BindToType("", typeName);
						// using ISerializable
						info.AddValue(reader.Name, DetermineValue(reader, converter, type));
						reader.ReadEndElement();
					}
					// use the surrogate to populate the instance
					initializedObject = serializationSurrogate.SetObjectData(initializedObject, info, Context, SurrogateSelector);

					AddInitializedObject(id, initializedObject);
				}
			}
			else if (IsTypeGenericDictionary(actualType))
			{
				IDictionary dictionary;

				// read the comparer, if present
				reader.ReadStartElement();

				if (reader.IsStartElement() && reader.Name == "Comparer")
				{
					string typename = reader.GetAttribute("type");
					int comparerID = 0;
					object comparer = null;

					if (string.IsNullOrEmpty(typename) &&
						int.TryParse(reader.GetAttribute("id"), out comparerID))
					{
						if (IDObject.ContainsKey(comparerID))
							comparer = IDObject[comparerID];
					}

					if (comparer == null)
					{
						comparer = DetermineValue(reader, converter, Binder.BindToType("", typename));
					}

					dictionary = (IDictionary)Activator.CreateInstance(actualType, comparer);

					reader.ReadStartElement();
				}
				else
					dictionary = (IDictionary)Activator.CreateInstance(actualType);

				// read the key value pairs
				while (reader.IsStartElement())
				{
					string typename;

					if (reader.Name != "Key")
						throw new SerializationException("Expected Key node, but found " + reader.Name + ".");

					typename = reader.GetAttribute("type");
					object key = DetermineValue(reader, converter, Binder.BindToType("", typename));

					reader.ReadEndElement();

					reader.IsStartElement();

					if (reader.Name != "Value")
						throw new SerializationException("Expected Value node, but found " + reader.Name + ".");

					typename = reader.GetAttribute("type");
					object value = DetermineValue(reader, converter, Binder.BindToType("", typename));

					dictionary.Add(key, value);

					reader.ReadEndElement();
				}

				skipDeserializationCallback = true;
				initializedObject = dictionary;
				AddInitializedObject(id, initializedObject);
			}
			else if (IsTypeGenericList(actualType))
			{
				AddUninitObject(id, null);

				initializedObject = DeserializeList(reader, converter, actualType);

				AddInitializedObject(id, initializedObject);
			}
			else if (typeof(ISerializable).IsAssignableFrom(actualType))
			{
				// The item implements ISerializable. Create a SerializationInfo object
				info = new SerializationInfo(actualType, converter);
				// Populate the collection
				PopulateSerializationInfo(reader, converter, actualType, info);
				// Get the specialized Serialization Constructor
				ConstructorInfo ctor = actualType.GetConstructor(
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
					null, // no binder
					CallingConventions.HasThis,
					new Type[] { typeof(SerializationInfo), typeof(StreamingContext) },
					null);

				// Create the object
				initializedObject = ctor.Invoke(new object[] { info, Context });
				AddInitializedObject(id, initializedObject);
			}
			else if (actualType.IsPrimitive || actualType == typeof(string))
			{
				initializedObject = DetermineValue(reader, converter, actualType);
			}
			else
			{
				// The item does not implement ISerializable. Use reflection to get public 
				// fields and properties.
				initializedObject = FormatterServices.GetUninitializedObject(actualType);
				AddUninitObject(id, initializedObject);

				List<MemberInfo> memberList = new List<MemberInfo>();
				List<object> valuesList = new List<object>();

				// make sure we don't try to read fields on an object which has none.
				if (reader.IsEmptyElement == false)
				{
					// read the first element.
					reader.ReadStartElement();

					while (reader.IsStartElement())
					{
						// check to see if this member is null
						bool isNull = string.IsNullOrEmpty(reader.GetAttribute("null")) ? false : true;

						if (isNull)
						{
							// don't read an end element because there won't be one
							// for a null object.  just skip to the next start element.
							reader.ReadStartElement();
							continue;
						}

						// check to see if the current field is from a base class or a
						string reflectedType = reader.GetAttribute("reflectedType");
						Type sourceType = actualType;  // place where the field is declared
						string typename = reader.GetAttribute("type");
						Type dataType = null;

						if (string.IsNullOrEmpty(typename) == false)
						{
							dataType = Binder.BindToType("", typename);
						}

						// get the object reference ID, for use later.
						int thisID = 0;
						int.TryParse(reader.GetAttribute("id"), out thisID);

						if (string.IsNullOrEmpty(reflectedType) == false)
						{
							sourceType = Binder.BindToType("", reflectedType);
						}

						// get the correct name, especially for compiler generated fields.
						string fieldName = reader.GetAttribute("name");

						if (string.IsNullOrEmpty(fieldName))
						{
							fieldName = reader.Name;
						}

						// Get the public field of this type that matches the name.
						MemberInfo[] possibleMembers = sourceType.GetMember(fieldName, MemberTypes.Field,
							BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

						MemberInfo[] test = sourceType.GetMembers(
							BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

						if (possibleMembers == null)
							throw new SerializationException(string.Format("Serialization stream contained element not found in type.{0}", reader.Name));

						Type checkType = sourceType.BaseType;

						while (possibleMembers.Length == 0 && checkType != null)
						{
							possibleMembers = checkType.GetMember(fieldName, MemberTypes.Field,
								BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

							checkType = checkType.BaseType;
						}
						//while (possibleMembers.Length == 0 && sourceType.get
						if (possibleMembers.Length == 0)
						{
							if (reader.IsEmptyElement)
							{
								reader.ReadStartElement();
							}
							else
								reader.ReadEndElement();
							continue;
						}

						if (possibleMembers.Length > 1)
							throw new SerializationException(string.Format("More than one member found for tag {0}", reader.Name));

						if (possibleMembers[0].MemberType == MemberTypes.Property)
							throw new SerializationException("We have a property where we should only have fields.");

						// determine the field we are writing to
						Type parentType = possibleMembers[0].ReflectedType;
						FieldInfo field = parentType.GetField(possibleMembers[0].Name,
							BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

						// check and see if the serialized data is an array
						isArray = false;

						if (string.IsNullOrEmpty(reader.GetAttribute("isArray")) == false)
							isArray = true;

						//////////////////////////////////////////
						// now we have all the information, here we begin trying to 
						// deserialize the item

						// check to see if the object is already being deserialized
						if (uninitObject.ContainsKey(thisID) && dataType == null)
						{
							SetFieldCallback callback = new SetFieldCallback();

							callback.field = field;
							callback.targetID = thisID;

							fieldCallbacks.Add(callback);

							reader.ReadStartElement();
							continue;
						}
						// check to see if the object has already been deserialized
						else if (IDObject.ContainsKey(thisID) && dataType == null)
						{
							object value = IDObject[thisID];

							memberList.Add(possibleMembers[0]);
							valuesList.Add(value);

							reader.ReadStartElement();
							continue;
						}
						else if (dataType == null && string.IsNullOrEmpty(typename) == false)
						{
							throw new SerializationException("Could not find type " + typename + ".");
						}
						else if (dataType == null)
						{
							throw new SerializationException("Object ID " + thisID.ToString() + " not found.");
						}
						else if (isArray)
						{
							AddUninitObject(thisID, null);

							System.Array array = DeserializeArray(reader, converter, dataType);


							memberList.Add(possibleMembers[0]);
							valuesList.Add(array);

							AddInitializedObject(thisID, array);

							if (array.Length == 0)
							{
								reader.ReadStartElement();
								continue;
							}
						}
						else if (IsTypeGenericList(dataType))
						{
							// the type is a list, so create it.
							//IList list = GetMemberValue(initializedObject, field) as IList;
							IList list = DeserializeList(reader, converter, dataType);

							memberList.Add(possibleMembers[0]);
							valuesList.Add(list);

							if (reader.IsEmptyElement)
							{
								reader.ReadStartElement();
								continue;
							}
						}
						else
						{
							// determine the value.
							object value = DetermineValue(reader, converter, dataType);
							memberList.Add(possibleMembers[0]);
							valuesList.Add(value);

							if (dataType == typeof(string) && value.Equals("") && isNull == false)
							{
								// annoying that XmlReader will throw an exception at 
								// ReadEndElement for an element written like <element />
								// to get around it, call ReadStartElement 
								reader.ReadStartElement();
								continue;
							}
						}

						reader.ReadEndElement();

					} // end while
				} // end if (reader.IsEmptyElement)

				if (memberList.Count > 0)
				{
					initializedObject = FormatterServices.PopulateObjectMembers(initializedObject, memberList.ToArray(), valuesList.ToArray());
				}

				AddInitializedObject(id, initializedObject);

			}

			if ((initializedObject as IDeserializationCallback) != null &&
				skipDeserializationCallback == false)
			{
				DeserializationCallBackList.Add(initializedObject);
			}

			// check for field setting callbacks
			for (int i = 0; i < fieldCallbacks.Count; i++)
			{
				fieldCallbacks[i].theObj = initializedObject;

				this.setFields.Add(fieldCallbacks[i]);
			}

			return initializedObject;
		}

		private IList DeserializeList(XmlTextReader reader, FormatterConverter converter, Type dataType)
		{
			Type listType = dataType;

			if (reader.HasAttributes)
			{
				listType = Type.GetType(reader.GetAttribute("type"));
			}

			IList list = Activator.CreateInstance(listType) as IList;

			if (reader.IsEmptyElement == false)
			{

				// read the next element
				reader.ReadStartElement();

				while (reader.IsStartElement())
				{
					if (!reader.IsEmptyElement)
					{
						string itemTypeName = reader.GetAttribute("type");
						Type itemType = Type.GetType(itemTypeName);

						// Initialize the object (recursive call)
						object listItem = InitializeObject(reader, converter, itemType);
						list.Add(listItem);
						reader.ReadEndElement();
					}
					else
					{
						reader.ReadStartElement();
					}
				}

			}
			return list;
		}

		private void AddUninitObject(int id, object uninitObj)
		{
			if (uninitObj != null && uninitObj.GetType().IsValueType)
				return;

			uninitObject.Add(id, uninitObject);

		}

		private void AddInitializedObject(int id, object initializedObject)
		{
			if (initializedObject.GetType().IsValueType)
				return;

			if (uninitObject.ContainsKey(id))
			{
				uninitObject.Remove(id);
			}

			IDObject.Add(id, initializedObject);
		}

		private System.Array DeserializeArray(XmlTextReader reader, FormatterConverter converter, Type dataType)
		{
			ArrayList list = new ArrayList();
			List<SetArrayIndexCallback> fieldSet = new List<SetArrayIndexCallback>();

			bool isNullArray = string.IsNullOrEmpty(reader.GetAttribute("null")) ? false : true;
			bool isEmptyArray = reader.IsEmptyElement;

			if (isNullArray)
			{
				throw new SerializationException("Tried to deserialize an empty array!");
				//return null;
			}

			if (isEmptyArray)
			{
				return (Array)Activator.CreateInstance(dataType.MakeArrayType(), 0);
			}

			// check for multidimensional arrays
			int rank;
			int[] bounds = null;

			if (int.TryParse(reader.GetAttribute("rank"), out rank))
			{
				bounds = new int[rank];
				for (int i = 0; i < rank; i++)
				{
					bounds[i] = int.Parse(reader.GetAttribute("bound" + i.ToString()));
				}
			}
			else
				rank = 1;

			// read the next element
			reader.ReadStartElement();

			while (reader.IsStartElement())
			{
				bool isNull = string.IsNullOrEmpty(reader.GetAttribute("null")) ? false : true;

				if (isNull)
				{
					list.Add(null);
					reader.ReadStartElement();

					continue;
				}

				// check if the item already exists
				int id = 0;

				if (int.TryParse(reader.GetAttribute("id"), out id))
				{
					if (IDObject.ContainsKey(id))
					{
						list.Add(IDObject[id]);

						reader.ReadStartElement();
						continue;
					}
					else if (uninitObject.ContainsKey(id))
					{
						SetArrayIndexCallback callback = new SetArrayIndexCallback();

						callback.targetID = id;
						callback.index = list.Count;

						// insert a placeholder value
						list.Add(null);

						fieldSet.Add(callback);

						reader.ReadStartElement();
						continue;
					}
				}

				// object not found, so create it
				// Initialize the object (recursive call)
				object listItem = InitializeObject(reader, converter, dataType);
				list.Add(listItem);

				if (reader.IsEmptyElement)
					reader.ReadStartElement();
				else
					reader.ReadEndElement();
			}

			Type arrayType = dataType.MakeArrayType(rank);
			Array array;

			// copy the values from the temporary ArrayList into the created array
			if (rank == 1)
			{
				array = (Array)Activator.CreateInstance(arrayType, list.Count);

				for (int i = 0; i < array.Length; i++)
				{
					array.SetValue(list[i], i);
				}

			}
			else
			{
				switch (rank)
				{
					case 2:
						array = (Array)Activator.CreateInstance(arrayType, bounds[0], bounds[1]);
						break;

					case 3:
						array = (Array)Activator.CreateInstance(arrayType, bounds[0], bounds[1], bounds[2]);
						break;

					case 4:
						array = (Array)Activator.CreateInstance(arrayType, bounds[0], bounds[1], bounds[2], bounds[3]);
						break;

					case 5:
						array = (Array)Activator.CreateInstance(arrayType, bounds[0], bounds[1], bounds[2], bounds[3], bounds[4]);
						break;

					case 6:
						array = (Array)Activator.CreateInstance(arrayType, bounds[0], bounds[1], bounds[2], bounds[3], bounds[4], bounds[5]);
						break;

					default:
						throw new SerializationException("Array rank is too high!");
				}

				int[] indices = new int[rank];

				// copy all array values to the next list
				for (int i = 0; i < list.Count; i++)
				{
					int checkIndex = rank - 1;

					// check to see if we've hit the boundary on the last dimension
					while (indices[checkIndex] == bounds[checkIndex])
					{
						// we've hit the boundary, so increment the next index up.
						indices[checkIndex] = 0;
						checkIndex--;
						indices[checkIndex]++;
					}

					array.SetValue(list[i], indices);

					indices[rank - 1]++;
				}
			}

			foreach (var field in fieldSet)
			{
				field.theObj = array;
				this.setFields.Add(field);
			}

			return array;
		}

		/// <summary>
		/// Populates the serialized members in the SerializationInfo.
		/// </summary>
		/// <param name="reader">The XmlReader to read from.</param>
		/// <param name="converter">The converter used to parse the values from the XML.</param>
		/// <param name="actualType">The type of the object to create.</param>
		/// <param name="info">The object to populate.</param>
		private void PopulateSerializationInfo(XmlTextReader reader, FormatterConverter converter, Type actualType, SerializationInfo info)
		{
			// read the next element.
			reader.ReadStartElement();

			while (reader.IsStartElement())
			{
				Type type = null;

				// determine type
				string typeName = reader.GetAttribute("type");
				bool isArray = string.IsNullOrEmpty(reader.GetAttribute("isArray")) ? false : true;

				// If type is found in attribute, get the System.Type by using the Binder.
				if (typeName != null)
					type = Binder.BindToType("", typeName);
				else
					type = typeof(object);

				if (reader.IsEmptyElement)
				{
					// if the type is an array type, place a empty array in the collection.
					if (isArray)
						info.AddValue(reader.Name, type.MakeArrayType().GetConstructor(new Type[] { typeof(System.Int32) }).Invoke(new object[] { 0 }));
					else
						info.AddValue(reader.Name, null);

					reader.ReadStartElement();
				}
				else
				{
					info.AddValue(reader.Name, DetermineValue(reader, converter, type));
					reader.ReadEndElement();
					/*
					if (isArray)
					{
						// Item found is an array type.
						string name = reader.Name;
						// create a list of the type.
						IList list = (IList) typeof (List<>).MakeGenericType (new Type[] { type }).GetConstructor (System.Type.EmptyTypes).Invoke (null);
						// read the next element.
						reader.ReadStartElement ();

						while (reader.IsStartElement ())
						{
							if (!reader.IsEmptyElement)
							{
								// determine value
								list.Add (DetermineValue (reader, converter, type));
								reader.ReadEndElement ();
							}
							else
							{
								reader.ReadStartElement ();
							}
						}
						// create an array of the element type and copy the list to the array.
						Array array = (Array) type.MakeArrayType ().GetConstructor (new Type[] { typeof (System.Int32) }).Invoke (new object[] { list.Count });
						list.CopyTo (array, 0);
						// add the array to the collection.
						info.AddValue (name, array, type.MakeArrayType ());

						if (!reader.IsEmptyElement)
							reader.ReadEndElement ();
						else
							reader.ReadStartElement ();
					}
					else
					{
						// type if not an array type.
						// determine value and add it to the collection
						info.AddValue(reader.Name, DetermineValue(reader, converter, type));
						reader.ReadEndElement();
					}
					 * */
				}
			}
		}

		/// <summary>
		/// Determines the value of an object.
		/// </summary>
		/// <param name="reader">The XML reader the read from.</param>
		/// <param name="converter">The converter used to parse the values from the XML.</param>
		/// <param name="objectType">The type of the object to create.</param>
		/// <returns>The value of the object.</returns>
		private object DetermineValue(XmlTextReader reader, FormatterConverter converter, Type objectType)
		{
			object parsedObject;

			// check if the value can be directly determined or that the type is a complex type.
			if (objectType.IsPrimitive || objectType == typeof(string)
				|| objectType == typeof(DateTime) || objectType == typeof(object))
			{
				// directly parse
				string value = reader.ReadString();

				// convert escape sequences
				if (objectType == typeof(string) || objectType == typeof(char))
				{
					StringBuilder result = new StringBuilder(value.Length);

					for (int i = 0; i < value.Length; i++)
					{
						if (value[i] == '\\' && value.Length > i + 1)
						{
							switch (value[i + 1])
							{
								case 'n': result.Append('\n'); break;
								case 't': result.Append('\t'); break;
								case 'r': result.Append('\r'); break;

								default: result.Append(value[i + 1]); break;

							}
							i++;
						}
						else
							result.Append(value[i]);
					}

					value = result.ToString();
				}

				parsedObject = converter.Convert(value, objectType);
			}
			else if (objectType.IsEnum)
			{
				string value = reader.ReadString();
				int intValue = (int)converter.Convert(value, typeof(int));

				parsedObject = intValue;

				foreach (Object v in Enum.GetValues(objectType))
				{
					if ((int)v == intValue)
					{
						parsedObject = v;
						break;
					}
				}

				//parsedObject = converter.Convert(converter.Convert(value, typeof(int)), objectType);
			}
			else
			{
				// Initialize the object (recursive call)
				parsedObject = InitializeObject(reader, converter, objectType);
			}

			return parsedObject;
		}

		#endregion

		#region --- Type Identification ---

		private static bool IsTypeGenericDictionary(Type _objectType)
		{
			if (typeof(IDictionary).IsAssignableFrom(_objectType))
			{
				if (_objectType.IsGenericType)
				{
					// detect if it's a Dictionary<TKey,TValue> object.
					Type[] genericParams = _objectType.GetGenericArguments();

					if (genericParams.Length == 2)
					{
						Type gendictionaryType = typeof(Dictionary<,>);
						Type dictionaryType = gendictionaryType.MakeGenericType(genericParams);

						if (_objectType == dictionaryType)
						{
							return true;
						}
					}
				}
			}

			return false;
		}
		private static bool IsTypeGenericList(Type type)
		{
			Type[] genericParams = type.GetGenericArguments();

			if (genericParams.Length == 1)
			{
				Type genListType = typeof(List<>);
				Type listType = genListType.MakeGenericType(genericParams);

				if (type == listType)
				{
					return true;
				}
			}

			return false;
		}

		#endregion

		#endregion

		/// <summary>
		/// Enum which indicates which type of collection is being serialized.
		/// </summary>
		public enum CollectionType
		{
			/// <summary>
			/// Non-collection type
			/// </summary>
			None,
			/// <summary>
			/// System.Collections.ArrayList type
			/// </summary>
			ArrayList,
			/// <summary>
			/// System.Collections.Generic.List&lt;T&gt; type
			/// </summary>
			List,
			/// <summary>
			/// System.Collections.Generic.Dictionary&lt;Tkey,Tvalue&gt; type
			/// </summary>
			Dictionary,
		}
		/// <summary>
		/// The <see cref="CustomSerializationEntry"/> mimics the <see cref="System.Runtime.Serialization.SerializationEntry"/> class
		/// to make it possible to create our own entries. The class acts as a placeholder for a type,
		/// it's name and it's value. This class is used in the <see cref="XmlFormatter"/> class to 
		/// serialize objects.
		/// </summary>
		public struct CustomSerializationEntry
		{
			#region --- Class members ---

			/// <summary>
			/// The type in which this member is declared in.
			/// </summary>
			private Type _reflectedType;
			/// <summary>
			/// The name of the object.
			/// </summary>
			private string _name;
			/// <summary>
			/// The type of the object.
			/// </summary>
			private Type _objectType;
			/// <summary>
			/// The value of the object.
			/// </summary>
			private object _value;
			/// <summary>
			/// Indicates whether the object is a list.
			/// </summary>
			private CollectionType _collectionType;

			#endregion

			/// <summary>
			/// Converts the custom serialization entry to a string.
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return "CustomSerializationEntry: " + Name + " : " + _objectType.ToString();
			}
			#region --- Properties ---

			/// <summary>
			/// Gets the name of the object.
			/// </summary>
			public string Name
			{
				get
				{
					return _name;
				}
			}

			/// <summary>
			/// Gets the System.Type of the object.
			/// </summary>
			public Type ObjectType
			{
				get
				{
					return _objectType;
				}
				set
				{
					_objectType = value;
				}
			}


			/// <summary>
			/// Gets the System.Type of the object this is reflected from.
			/// </summary>
			public Type ReflectedType
			{
				get
				{
					return _reflectedType;
				}
				set
				{
					_reflectedType = value;
				}
			}

			/// <summary>
			/// Gets or sets the value contained in the object.
			/// </summary>
			public object Value
			{
				get
				{
					return _value;
				}
				set
				{
					_value = value;
				}
			}

			/// <summary>
			/// Gets or sets whether the object is a list type.
			/// </summary>
			public CollectionType CollectionType
			{
				get
				{
					return _collectionType;
				}
				set
				{
					_collectionType = value;
				}
			}

			#endregion

			#region --- Constructors ---

			/// <summary>
			/// Constructor to create a <see cref="CustomSerializationEntry"/> without a value. 
			/// Value is set to null.
			/// </summary>
			/// <param name="name">The name of the object.</param>
			/// <param name="objectType">The System.Type of the object.</param>
			public CustomSerializationEntry(string name, Type objectType)
				: this(name, objectType, null, null)
			{
			}

			/// <summary>
			/// Constructor to create a <see cref="CustomSerializationEntry"/>. 
			/// </summary>
			/// <param name="name">The name of the object.</param>
			/// <param name="objectType">The System.Type of the object.</param>
			/// <param name="value">The value of the object.</param>
			public CustomSerializationEntry(string name, Type objectType, object value)
				: this(name, objectType, value, null)
			{
			}

			/// <summary>
			/// Constructor to create a <see cref="CustomSerializationEntry"/>. 
			/// </summary>
			/// <param name="name">The name of the object.</param>
			/// <param name="objectType">The System.Type of the object.</param>
			/// <param name="value">The value of the object.</param>
			/// <param name="reflectedType">The reflected type of the object.</param>
			public CustomSerializationEntry(string name, Type objectType, object value, Type reflectedType)
			{
				_name = name;
				_objectType = objectType;
				_value = value;
				_reflectedType = reflectedType;
				_collectionType = CollectionType.None;

				DetectListType();
			}

			#endregion

			internal void DetectListType()
			{
				if (typeof(IList).IsAssignableFrom(_objectType))
				{
					if (_objectType == typeof(ArrayList))
					{
						_collectionType = CollectionType.ArrayList;
					}
					else if (IsTypeGenericList(_objectType))
					{
						_collectionType = CollectionType.List;
					}
				}
				if (IsTypeGenericDictionary(_objectType))
				{
					_collectionType = CollectionType.Dictionary;
				}
			}

		}



		#region Binder class

		/// <summary>
		/// The <see cref="CustomBinder"/> class performs the mapping to types
		/// declared in this assembly. It accumulates all types defined in the assembly
		/// this class is defined in. Optionally, an assembly can be passed as an argument.
		/// </summary>
		public sealed class CustomBinder : SerializationBinder
		{
			#region --- Class members ---

			/// <summary>
			/// The list that holds the types and type names contained in the assembly.
			/// </summary>
			private static Dictionary<string, Type> _typeList = null;

			#endregion

			#region --- Constructors ---

			/// <summary>
			/// Static constructor to load a list of types contained in the assembly
			/// only once (during the first call).
			/// </summary>
			static CustomBinder()
			{
				//_typeList = LoadTypes (typeof (CustomBinder).Assembly);
			}

			#endregion

			#region --- Methods ---

			/// <summary>
			/// Loads the types from the passed assembly in the list. The key of the list
			/// is the simple name of the type.
			/// </summary>
			/// <param name="assembly"></param>
			/// <remarks>
			/// Because the list uses the simple name of the type, it should be unique within the 
			/// assembly. Otherwise, a <see cref="System.ArgumentException"/> is thrown.
			/// </remarks>
			/// <returns>A Dictionary&lt;string, Type&gt; object.</returns>
			private static Dictionary<string, Type> LoadTypes(System.Reflection.Assembly assembly)
			{
				Dictionary<string, Type> typeList = new Dictionary<string, Type>();

				foreach (Type type in assembly.GetTypes())
				{
					typeList.Add(type.Name, type);
				}
				return typeList;
			}

			/// <summary>
			/// Binds the passed typename to the type contained in the dictionary.
			/// </summary>
			/// <param name="assemblyName">The assembly to load the type from.</param>
			/// <param name="typeName">The simple name of the type.</param>
			/// <remarks>
			/// When the passed type is not found in the assembly, the method will try 
			/// to get the Type using System.GetType. If still not found, the method will
			/// return typeof (Object).
			/// </remarks>
			/// <returns>The Type reference of the type name.</returns>
			public override Type BindToType(string assemblyName, string typeName)
			{
				Dictionary<string, Type> typeList = null;

				if (assemblyName.Length > 0)
				{
					System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
					typeList = LoadTypes(assembly);
				}
				else
				{
					typeList = _typeList;
				}

				Type type = Type.GetType(typeName);
				if (type != null)
					return type;
				else
				{
					Assembly appAssembly = Assembly.GetEntryAssembly();
					AssemblyName[] names = appAssembly.GetReferencedAssemblies();

					foreach (AssemblyName name in names)
					{
						Assembly current = Assembly.Load(name);
						Type thisType = current.GetType(typeName);

						if (thisType != null)
						{
							return thisType;
						}
					}

					foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
					{
						Type thisType = ass.GetType(typeName);

						if (thisType != null)
							return thisType;
					}


				}

				return null;
				//throw new SerializationException(string.Format("Could not find type {0}", typeName));
			}

			#endregion
		}

		#endregion

		class SetFieldCallback
		{
			public object theObj;
			public FieldInfo field;
			public int targetID;

			public virtual void Execute(XmlFormatter formatter)
			{
				field.SetValue(theObj, formatter.IDObject[targetID]);
			}
		}
		class SetArrayIndexCallback : SetFieldCallback
		{
			public int index;


			public override void Execute(XmlFormatter formatter)
			{
				Array array = (Array)theObj;

				if (array.Rank > 1)
				{
					int[] indices = new int[array.Rank];
					int[] bounds = new int[array.Rank];

					// find the bounds for each dimension in the array.
					for (int i = 0; i < array.Rank; i++)
					{
						bounds[i] = array.GetUpperBound(i) + 1;
					}

					// find the correct indices in the multidimensional array.
					for (int i = 0; i < index; i++)
					{
						int checkIndex = array.Rank - 1;

						// check to see if we've hit the boundary on the last dimension
						while (indices[checkIndex] == bounds[checkIndex])
						{
							// we've hit the boundary, so increment the next index up.
							indices[checkIndex] = 0;
							checkIndex--;
							indices[checkIndex]++;
						}


						indices[array.Rank - 1]++;
					}

					array.SetValue(formatter.IDObject[targetID], indices);

				}
				else
					array.SetValue(formatter.IDObject[targetID], index);
			}
		}
	}
#endif
}
