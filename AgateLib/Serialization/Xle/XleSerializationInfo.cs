//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Reflection;
using AgateLib.Drivers;

namespace AgateLib.Serialization.Xle
{
	/// <summary>
	/// The XleSerializationInfo class contains the XML data that is read from 
	/// or written to when doing XLE serialization.
	/// </summary>
	public class XleSerializationInfo
	{
		XDocument doc;
		Stack<XElement> nodes = new Stack<XElement>();
		XleTypeSerializerCollection TypeSerializers;

		internal XleSerializationInfo(ITypeBinder Binder1, XleTypeSerializerCollection TypeSerializers, IObjectConstructor constructor = null, XDocument document = null)
		{
			this.Binder = Binder1;
			this.TypeSerializers = TypeSerializers;

			this.doc = document;
			this.ObjectConstructor = constructor;

			if (this.doc == null)
				this.doc = new XDocument();
		}

		internal XDocument XmlDoc
		{
			get { return doc; }
		}

		IObjectConstructor ObjectConstructor { get; set; }
		XElement CurrentNode
		{
			get
			{
				return nodes.Peek();
			}
		}

		void Serialize(IXleSerializable o)
		{
			o.WriteData(this);
		}
		void Serialize(object o)
		{
			WritePublicProperties(o);
		}

		void AddAttribute(XElement node, string name, string value)
		{
			XAttribute attrib = new XAttribute(name, value);

			node.Add(attrib);
		}

		internal void BeginSerialize(IXleSerializable objectGraph)
		{
			var root = new XElement("XleRoot");

			AddAttribute(root, "type", objectGraph.GetType().ToString());

			doc.Add(root);

			nodes.Push(root);
			Serialize(objectGraph);

			System.Diagnostics.Debug.Assert(nodes.Count == 1);
			nodes.Clear();
		}
		internal void BeginSerialize(object objectGraph)
		{
			var root = new XElement("XleRoot");

			AddAttribute(root, "type", objectGraph.GetType().ToString());

			doc.Add(root);
			nodes.Push(root);
			Serialize(objectGraph);

			System.Diagnostics.Debug.Assert(nodes.Count == 1);
			nodes.Clear();
		}

		#region --- Writing methods ---

		#region --- Writing single values to the XML ---

		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, string value, bool asAttribute = false)
		{
			if (value == null) value = "";

			WriteImpl(name, value ?? string.Empty, asAttribute);
		}

		/// <summary>
		/// Writes an enum field to the XML data as an element or an attribute.
		/// </summary>
		/// <typeparam name="T">Type of the enum.  If this is not an enum type, an exception is thrown</typeparam>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void WriteEnum<T>(string name, T value, bool asAttribute = false) where T : struct
		{
			if (typeof(T).IsEnum == false)
				throw new XleSerializationException("Type passed is not an enum.");

			WriteImpl(name, value.ToString(), asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, double value, bool asAttribute = false)
		{
			WriteImpl(name, value.ToString(), asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, bool value, bool asAttribute = false)
		{
			WriteImpl(name, value.ToString(), asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, char value, bool asAttribute = false)
		{
			WriteImpl(name, value.ToString(), asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, int value, bool asAttribute = false)
		{
			WriteImpl(name, value.ToString(), asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, long value, bool asAttribute = false)
		{
			WriteImpl(name, value.ToString(), asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, decimal value, bool asAttribute = false)
		{
			WriteImpl(name, value.ToString(), asAttribute);
		}

		void WriteImpl(string name, string value, bool asAttribute = false)
		{
			if (asAttribute)
			{
				WriteAsAttribute(name, value);
			}
			else
				WriteAsElement(name, value);

		}

		/// <summary>
		/// Writes a nullable value to 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="asAttribute"></param>
		public void Write<T>(string name, T? value, bool asAttribute = false) where T : struct, IConvertible
		{
			if (asAttribute)
			{
				if (value == null)
					AddAttribute(CurrentNode, name, "null");
				else
					WriteAsAttribute(name, ((T)value).ToString());
			}
			else
			{
				if (value == null)
					WriteAsElement(name, "null");
				else
					WriteAsElement(name, ((T)value).ToString());
			}
		}

		private XElement WriteAsElement(string name, string value)
		{
			XElement element = CreateElement(name);

			element.Value = value;

			return element;
		}
		private void WriteAsAttribute(string name, string value)
		{
			AddAttribute(CurrentNode, name, value);
		}

		private XElement CreateElement(string name)
		{
			XElement element = new XElement(name);

			if (CurrentNode.Elements(name).Count() > 0)
				throw new XleSerializationException("The name " + name + " already exists.");

			CurrentNode.Add(element);

			return element;
		}

		#endregion
		#region --- Writing arrays ---

		/// <summary>
		/// Writes an int[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, int[] value)
		{
			WriteImpl(name, value, NumericEncoding.Csv);
		}
		/// <summary>
		/// Writes an int[] array to the XML data as an element with the specified encoding.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="encoding"></param>
		public void Write(string name, int[] value, NumericEncoding encoding)
		{
			WriteImpl(name, value, encoding);
		}

		private void WriteImpl(string name, int[] value, NumericEncoding encoding)
		{
			switch (encoding)
			{
				case NumericEncoding.Base64:
					byte[] array = new byte[value.Length * 4];

					if (array.Length > 0)
					{
						for (int j = 0; j < value.Length; j++)
						{
							var bytes = BitConverter.GetBytes(value[j]);

							for (int i = 0; i < bytes.Length; i++)
							{
								array[j * 4 + i] = bytes[i];
							}
						}
					}

					WriteBase64Encoded(name, array);
					break;

				case NumericEncoding.Csv:
					WriteAsCsv(name, value);

					break;

				default:
					throw new ArgumentException("Value of encoding is not understood.");
			}
		}

		private void WriteAsCsv<T>(string name, IEnumerable<T> value)
		{
			WriteAsCsv(name, value, x => x.ToString());
		}
		private void WriteAsCsv<T>(string name, IEnumerable<T> value, Func<T, string> converter)
		{
			string newValue = string.Join(",", value.Select(x => converter(x)).ToArray());

			XElement el = WriteAsElement(name, newValue);

			AddAttribute(el, "array", "true");
			AddAttribute(el, "encoding", "Csv");
		}

		/// <summary>
		/// Writes a bool[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, bool[] value)
		{
			WriteAsCsv(name, value);
		}

		/// <summary>
		/// Writes an double[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, double[] value)
		{
			WriteAsCsv(name, value);
		}

		/// <summary>
		/// Writes a byte[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, byte[] value)
		{
			WriteAsCsv(name, value);
		}

		private void WriteBase64Encoded(string name, byte[] value)
		{
			string newValue = Convert.ToBase64String(value);

			XElement el = WriteAsElement(name, newValue);

			AddAttribute(el, "array", "true");
			AddAttribute(el, "encoding", "Base64");
		}


		/// <summary>
		/// Writes an array of objects implementing IXleSerializable to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write<T>(string name, T[] value) where T : IXleSerializable
		{
			List<T> list = new List<T>();
			list.AddRange(value);

			Write(name, list);
		}
		/// <summary>
		/// Writes a List&lt;T&gt; of objects implementing IXleSerializable to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The list data to write.</param>
		public void Write<T>(string name, List<T> value) where T : IXleSerializable
		{
			Type listType = typeof(T);

			XElement element = CreateElement(name);
			AddAttribute(element, "array", "true");
			AddAttribute(element, "type", listType.ToString());

			try
			{
				nodes.Push(element);

				for (int i = 0; i < value.Count; i++)
				{
					XElement item = new XElement("Item");
					CurrentNode.Add(item);

					if (value[i] == null)
						AddAttribute(item, "type", "null");
					else
					{
						if (value[i].GetType() != listType)
							AddAttribute(item, "type", value[i].GetType().ToString());

						try
						{
							nodes.Push(item);
							Serialize(value[i]);
						}
						finally
						{
							nodes.Pop();
						}
					}
				}
			}
			finally
			{
				nodes.Pop();
			}
		}
		/// <summary>
		/// Writes an array of strings to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, string[] value)
		{
			Write(name, value.ToList());
		}
		/// <summary>
		/// Writes a List&lt;T&gt; of strings to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The list data to write.</param>
		public void Write(string name, List<string> value)
		{
			XElement element = CreateElement(name);
			AddAttribute(element, "array", "true");
			AddAttribute(element, "type", "string");

			try
			{
				nodes.Push(element);

				for (int i = 0; i < value.Count; i++)
				{
					XElement item = new XElement("Item");
					CurrentNode.Add(item);
					item.Value = value[i];
				}
			}
			finally
			{
				nodes.Pop();
			}
		}
		/// <summary>
		/// Writes a Dictionary of objects implementing IXleSerializable to the XML data as an element.
		/// The key type must implement IConvertible and the value type must implment IXleSerializable.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The dictionary to write.</param>
		[CLSCompliant(false)]
		public void Write<Tkey, Tvalue>(string name, Dictionary<Tkey, Tvalue> value)
			where Tkey : IConvertible
			where Tvalue : IXleSerializable
		{
			Type keyType = typeof(Tkey);
			Type valueType = typeof(Tvalue);

			XElement element = CreateElement(name);
			AddAttribute(element, "dictionary", "true");
			//AddAttribute(element, "keytype", keyType.ToString());
			//AddAttribute(element, "valuetype", valueType.ToString());

			try
			{
				nodes.Push(element);

				foreach (KeyValuePair<Tkey, Tvalue> kvp in value)
				{
					XElement item = new XElement("Item");
					CurrentNode.Add(item);

					AddAttribute(item, "key", kvp.Key.ToString());

					//if (keyType != kvp.Key.GetType())
					//    AddAttribute(item, "IDtype", kvp.Key.GetType().ToString());

					if (kvp.Value.GetType() != valueType)
						AddAttribute(item, "type", kvp.Value.GetType().ToString());

					try
					{
						nodes.Push(item);
						Serialize(kvp.Value);
					}
					finally
					{
						nodes.Pop();
					}
				}
			}
			finally
			{
				nodes.Pop();
			}
		}
		/// <summary>
		/// Writes a Dictionary of integers to the XML data as an element.
		/// The key type must implement IConvertible and the value type must implment IXleSerializable.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The dictionary to write.</param>
		[CLSCompliant(false)]
		public void Write<Tkey>(string name, Dictionary<Tkey, int> value)
			where Tkey : IConvertible
		{
			Type keyType = typeof(Tkey);

			XElement element = CreateElement(name);
			//AddAttribute(element, "dictionary", "true");
			//AddAttribute(element, "keytype", keyType.ToString());
			//AddAttribute(element, "valuetype", valueType.ToString());

			try
			{
				nodes.Push(element);

				foreach (KeyValuePair<Tkey, int> kvp in value)
				{
					XElement item = new XElement("Item");
					CurrentNode.Add(item);

					AddAttribute(item, "key", kvp.Key.ToString());
					AddAttribute(item, "value", kvp.Value.ToString());
				}
			}
			finally
			{
				nodes.Pop();
			}
		}
		/// <summary>
		/// Writes a Dictionary of strings implementing IXleSerializable to the XML data as an element.
		/// The key type must implement IConvertible.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The dictionary to write.</param>
		[CLSCompliant(false)]
		public void Write<Tkey>(string name, Dictionary<Tkey, string> value)
			where Tkey : IConvertible
		{
			Type[] args = value.GetType().GetGenericArguments();
			Type keyType = args[0];

			XElement element = CreateElement(name);
			//AddAttribute(element, "dictionary", "true");
			//AddAttribute(element, "keytype", keyType.ToString());
			//AddAttribute(element, "valuetype", typeof(string).ToString());

			try
			{
				nodes.Push(element);

				foreach (KeyValuePair<Tkey, string> kvp in value)
				{
					XElement item = new XElement("Item");
					CurrentNode.Add(item);

					AddAttribute(item, "key", kvp.Key.ToString());

					//if (keyType != kvp.Key.GetType())
					//    AddAttribute(item, "keytype", kvp.Key.GetType().ToString());

					AddAttribute(item, "value", kvp.Value.ToString());
				}
			}
			finally
			{
				nodes.Pop();
			}
		}

		#endregion
		#region --- Writing Streams ---

		/// <summary>
		/// Writes a binary stream to the XML data as an element.  
		/// Compresses the stream using GZip compression.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The stream of data to write.</param>
		public void Write(string name, Stream value)
		{
			Write(name, value, CompressionType.GZip);
		}
		/// <summary>
		/// Writes a binary stream to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The stream of data to write.</param>
		/// <param name="compression">The compression algorithm to use.</param>
		public void Write(string name, Stream value, CompressionType compression)
		{
			WriteImpl(name, value, compression);
		}

		private void WriteImpl(string name, Stream value, CompressionType compression)
		{
			if (compression != CompressionType.None)
			{
				MemoryStream ms = new MemoryStream();
				Stream compressed = TranslateStream(ms, compression, CompressionMode.Compress);

				byte[] uncompressedData = ReadFromStream(value);
				compressed.Write(uncompressedData, 0, uncompressedData.Length);

				value = ms;
				value.Seek(0, SeekOrigin.Begin);
			}

			byte[] buffer = new byte[value.Length];
			value.Read(buffer, 0, (int)value.Length);

			string newValue = Convert.ToBase64String(buffer);

			XElement el = WriteAsElement(name, newValue);
			AddAttribute(el, "stream", "true");
			AddAttribute(el, "compression", compression.ToString());
			AddAttribute(el, "encoding", "Base64");
		}

		#endregion
		#region --- Writing generic objects ---

		public void Write(string name, Array value)
		{
			XElement element = CreateElement(name);
			AddAttribute(element, "array", "true");

			if (value == null)
				AddAttribute(element, "type", "null");
			else
			{
				AddAttribute(element, "type", value.GetType().GetElementType().FullName);

				IXleTypeSerializer typeSer = TypeSerializers[value.GetType().GetElementType()];
				if (typeSer == null)
					throw new XleSerializationException("Could not serialize object of type " + value.GetType().FullName);

				try
				{
					nodes.Push(element);

					for (int i = 0; i < value.Length; i++)
					{
						object item = value.GetValue(i);

						if (item == null)
						{
							XElement eitem = new XElement("Item", new XAttribute("type", "null"));
							element.Add(eitem);
						}
						else
						{
							XElement eitem = new XElement("Item",
								new XAttribute("type", item.GetType().FullName));

							element.Add(eitem);

							try
							{
								nodes.Push(eitem);

								typeSer.Serialize(this, value.GetValue(i));
							}
							finally
							{
								nodes.Pop();
							}
						}
					}
				}
				finally
				{
					nodes.Pop();
				}
			}
		}
		public void Write(string name, object value)
		{
			if (value is IXleSerializable)
			{
				Write(name, (IXleSerializable)value);
				return;
			}
			if (value is Array)
			{
				Write(name, (Array)value);
				return;
			}

			XElement element = CreateElement(name);

			if (value == null)
				AddAttribute(element, "type", "null");
			else
			{
				AddAttribute(element, "type", value.GetType().ToString());

				IXleTypeSerializer typeSer = TypeSerializers[value.GetType()];
				if (typeSer == null)
					throw new XleSerializationException("Could not serialize object of type " + value.GetType().FullName);

				try
				{
					nodes.Push(element);

					typeSer.Serialize(this, value);
				}
				finally
				{
					nodes.Pop();
				}
			}
		}

		/// <summary>
		/// Writes an object implementing IXleSerializable to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The object data to write.</param>
		public void Write(string name, IXleSerializable value)
		{
			XElement element = CreateElement(name);

			if (value == null)
				AddAttribute(element, "type", "null");
			else
			{
				AddAttribute(element, "type", value.GetType().ToString());

				try
				{
					nodes.Push(element);

					Serialize(value);
				}
				finally
				{
					nodes.Pop();
				}
			}
		}

		#endregion

		#endregion
		#region --- Reading methods ---

		/// <summary>
		/// Checks to see if the given key is in the deserialized data.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool ContainsKey(string name)
		{
			var attribute = CurrentNode.Attribute(name);

			if (attribute != null)
				return true;

			XElement element = CurrentNode.Element(name);

			if (element != null)
				return true;

			return false;
		}

		#region --- Reading single values ---

		/// <summary>
		/// Reads an object from the XML data.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object ReadObject(string name)
		{
			XElement element = CurrentNode.Element(name);

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			try
			{
				nodes.Push(element);
				return DeserializeObject();
			}
			finally
			{
				nodes.Pop();
			}
		}

		/// <summary>
		/// Reads an object of the specified type from the Xle serialized data.
		/// </summary>
		/// <typeparam name="T">Type of the object to read.</typeparam>
		/// <param name="name">Key name of the object.</param>
		/// <returns></returns>
		public T ReadObject<T>(string name)
		{
			return (T)ReadObject(name);
		}

		/// <summary>
		/// Reads a string from the XML data, with an optional default value substituted
		/// if the name is not present.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public string ReadString(string name, string defaultValue)
		{
			return ReadStringImpl(name, true, defaultValue);
		}
		/// <summary>
		/// Reads a string from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">The name of the XML field to decode.</param>
		/// <returns></returns>
		public string ReadString(string name)
		{
			return ReadStringImpl(name, false, string.Empty);
		}

		private string ReadStringImpl(string name, bool haveDefault, string defaultValue)
		{
			var attrib = CurrentNode.Attribute(name);

			if (attrib != null)
				return attrib.Value;

			XElement element = CurrentNode.Element(name);

			if (element != null)
			{
				if (element.Attribute("encoding") != null)
				{
					throw new XleSerializationException("Cannot decode encoded strings.");
				}

				return element.Value;
			}

			if (haveDefault)
				return defaultValue;
			else
				throw new XleSerializationException("Field " + name + " was not found.");
		}
		/// <summary>
		/// Reads a boolean value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public bool ReadBoolean(string name)
		{
			return ReadBooleanImpl(name, false, false);
		}
		/// <summary>
		/// Reads a boolean value from the XML data.  If the name is not present 
		/// the default value is returned.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <param name="defaultValue">Default value to return if the field is not present.</param>
		/// <returns></returns>
		public bool ReadBoolean(string name, bool defaultValue)
		{
			return ReadBooleanImpl(name, true, defaultValue);
		}
		private bool ReadBooleanImpl(string name, bool hasDefault, bool defaultValue)
		{
			var attribute = CurrentNode.Attribute(name);

			if (attribute != null && string.IsNullOrEmpty(attribute.Value) == false)
				return bool.Parse(attribute.Value);

			XElement element = CurrentNode.Element(name);

			if (element == null)
			{
				if (hasDefault)
					return defaultValue;
				else
					throw new XleSerializationException("Node " + name + " not found.");
			}

			return bool.Parse(element.Value);
		}
		/// <summary>
		/// Reads a integer value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public int? ReadInt32Nullable(string name)
		{
			var attribute = CurrentNode.Attribute(name);
			if (attribute == null)
				return null;

			string attributeValue = attribute.Value;
			if (attributeValue == "null")
				return null;
			else if (string.IsNullOrEmpty(attributeValue) == false)
				return int.Parse(attributeValue);

			XElement element = CurrentNode.Element(name);

			if (element != null)
			{
				if (element.Value == "null")
					return null;

				return int.Parse(element.Value);
			}

			throw new XleSerializationException("Node " + name + " not found.");
		}/// <summary>
		/// Reads a integer value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public int ReadInt32(string name)
		{
			var attribute = CurrentNode.Attribute(name);

			if (attribute != null && string.IsNullOrEmpty(attribute.Value) == false)
				return int.Parse(attribute.Value);

			XElement element = CurrentNode.Element(name);

			if (element != null)
			{
				return int.Parse(element.Value);
			}

			throw new XleSerializationException("Node " + name + " not found.");
		}
		/// <summary>
		/// Reads a integer value from the XML data.  If the name is not present 
		/// the default value is returned.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <param name="defaultValue">The default value to return if the name is not present.</param>
		/// <returns></returns>
		public int ReadInt32(string name, int defaultValue)
		{
			var attribute = CurrentNode.Attribute(name);

			if (attribute != null && string.IsNullOrEmpty(attribute.Value) == false)
				return int.Parse(attribute.Value);

			XElement element = CurrentNode.Element(name);

			if (element == null)
				return defaultValue;

			return int.Parse(element.Value);
		}

		/// <summary>
		/// Reads an enum field from the XML data.
		/// </summary>
		/// <typeparam name="T">Type of the enum.  If this is not an enum type, an exception is thrown.</typeparam>
		/// <param name="name">The name of the XML element used.</param>
		public T ReadEnum<T>(string name) where T : struct
		{
			if (typeof(T).IsEnum == false)
				throw new XleSerializationException("Type passed is not an enum.");

			return (T)Enum.Parse(typeof(T), ReadStringImpl(name, false, string.Empty), true);
		}
		/// <summary>
		/// Reads an enum field from the XML data.
		/// </summary>
		/// <typeparam name="T">Type of the enum.  If this is not an enum type, an exception is thrown.</typeparam>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="defaultValue">Value returned if the key is not present in the XML data.</param>
		public T ReadEnum<T>(string name, T defaultValue) where T : struct
		{
			if (typeof(T).IsEnum == false)
				throw new XleSerializationException("Type passed is not an enum.");

			return (T)Enum.Parse(typeof(T), ReadStringImpl(name, true, defaultValue.ToString()), true);
		}

		/// <summary>
		/// Reads a double value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public double ReadDouble(string name)
		{
			var attribute = CurrentNode.Attribute(name);

			if (attribute != null && string.IsNullOrEmpty(attribute.Value) == false)
				return double.Parse(attribute.Value);

			XElement element = CurrentNode.Element(name);

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			return double.Parse(element.Value);
		}

		/// <summary>
		/// Reads a float value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public float ReadFloat(string name)
		{
			var attribute = CurrentNode.Attribute(name);

			if (attribute != null && string.IsNullOrEmpty(attribute.Value) == false)
				return float.Parse(attribute.Value);

			XElement element = CurrentNode.Element(name);

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			return float.Parse(element.Value);
		}

		#endregion
		#region --- Reading array values ---

		private NumericEncoding GetEncoding(string name)
		{
			XElement element = CurrentNode.Element(name);

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");
			if (element.Attribute("encoding") == null)
				throw new XleSerializationException("Element " + name + " does not have encoding information.");

			return (NumericEncoding)Enum.Parse(typeof(NumericEncoding), element.Attribute("encoding").Value, true);
		}

		/// <summary>
		/// Reads a byte array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		[Obsolete("Use ReadArray<byte> instead.")]
		public byte[] ReadByteArray(string name)
		{
			return ReadArray<byte>(name);
		}

		/// <summary>
		/// Reads a integer array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		[Obsolete("Use ReadArray<int> instead.")]
		public int[] ReadInt32Array(string name)
		{
			return ReadArray<int>(name);
		}

		/// <summary>
		/// Reads a boolean array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		[Obsolete("Use ReadArray<bool> instead.")]
		public bool[] ReadBooleanArray(string name)
		{
			return ReadArray<bool>(name);
		}

		/// <summary>
		/// Reads a double array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		[Obsolete("Use ReadArray<double> instead.")]
		public double[] ReadDoubleArray(string name)
		{
			return ReadArray<double>(name);
		}

		/// <summary>
		/// Reads a byte array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		byte[] _ReadByteArray(string name)
		{
			XElement element = CurrentNode.Element(name);
			return Convert.FromBase64String(element.Value);
		}

		T[] _ReadBase64Array<T>(string name, int byteCount, Func<byte[], T> converter)
		{
			List<T> retval = new List<T>();

			byte[] array = ReadArray<byte>(name);

			if (array.Length % 4 != 0)
				throw new XleSerializationException("Encoded array is wrong size!");

			byte[] bytes = new byte[byteCount];

			for (int i = 0; i < array.Length; i += byteCount)
			{
				Array.Copy(array, i, bytes, 0, byteCount);
				retval.Add(converter(bytes));
			}

			return retval.ToArray();
		}
		/// <summary>
		/// Reads a integer array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		int[] _ReadInt32Array(string name)
		{
			return _ReadBase64Array(name, 4, x => BitConverter.ToInt32(x, 0));
		}

		/// <summary>
		/// Reads a boolean array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		bool[] _ReadBooleanArray(string name)
		{
			byte[] array = ReadArray<byte>(name);
			bool[] result = new bool[array.Length];

			for (int i = 0; i < array.Length; i++)
				result[i] = array[i] != 0;

			return result;
		}

		/// <summary>
		/// Reads a double array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		double[] _ReadDoubleArray(string name)
		{
			return _ReadBase64Array<double>(name, 8, x => BitConverter.ToDouble(x, 0));
		}

		T[] _ReadCsvArray<T>(string name, Func<string, T> converter)
		{
			XElement element = CurrentNode.Element(name);

			string value = element.Value.Trim();
			string[] vals = value.Split(new char[] { ',' },
							StringSplitOptions.RemoveEmptyEntries);

			return vals.Select(x => converter(x)).ToArray();
		}

		/// <summary>
		/// Reads an array of objects from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public T[] ReadArray<T>(string name)
		{
			if (typeof(T).IsPrimitive)
			{
				switch (GetEncoding(name))
				{
					case NumericEncoding.Csv:
						return _ReadCsvArray(name, x => (T)Convert.ChangeType(x, typeof(T), System.Globalization.CultureInfo.InvariantCulture));

					case NumericEncoding.Base64:
						if (typeof(T) == typeof(int)) return (T[])(object)_ReadInt32Array(name);
						if (typeof(T) == typeof(double)) return (T[])(object)_ReadDoubleArray(name);
						if (typeof(T) == typeof(bool)) return (T[])(object)_ReadBooleanArray(name);
						if (typeof(T) == typeof(byte)) return (T[])(object)_ReadByteArray(name);

						throw new NotImplementedException();
				}
			}

			try
			{
				T[] retval = (T[])ReadArray(name);
				return retval;
			}
			catch (InvalidCastException ex)
			{
				throw new XleSerializationException("Array " + name + " is not of type " + typeof(T).Name + ".", ex);
			}
		}

		private Array ReadArray(string name)
		{
			return ReadArrayImpl(name, null);
		}
		private Array ReadArrayImpl(string name, Type defaultType)
		{
			XElement element = CurrentNode.Element(name);

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			if (element.Attribute("array") == null || element.Attribute("array").Value != "true")
				throw new XleSerializationException("Element " + name + " is not an array.");
			if (element.Attribute("type") == null && defaultType == null)
				throw new XleSerializationException("Element " + name + " does not have type information.");

			Type type;
			if (element.Attribute("type") == null)
				type = defaultType;

			else
			{
				string typename = element.Attribute("type").Value;

				type = GetType(typename);

				if (type == null)
					throw new XleSerializationException("Could not find type for " + typename + ".");
			}

			Type listType = typeof(List<>).MakeGenericType(type);
			Type arrayType = type.MakeArrayType();
			System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(listType);

			foreach (XElement item in element.Elements())
			{
				if (item.Name != "Item")
					throw new XleSerializationException("Could not understand data.  Expected Item, found " + item.Name + ".");

				if (type == typeof(string))
				{
					list.Add(item.Value);
				}
				else
				{
					try
					{
						nodes.Push(item);

						object o = DeserializeObject(type);
						list.Add(o);
					}
					finally
					{
						nodes.Pop();
					}
				}
			}

			Array retval = (Array)Activator.CreateInstance(arrayType, list.Count);
			list.CopyTo(retval, 0);

			return retval;
		}
		/// <summary>
		/// Reads a list of objects from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public List<T> ReadList<T>(string name)
		{
			Array ar;

			if (typeof(T) == typeof(int))
				ar = ReadInt32Array(name);
			else if (typeof(T) == typeof(double))
				ar = ReadDoubleArray(name);
			else
				ar = ReadArrayImpl(name, typeof(T));

			return ((T[])ar).ToList();
		}

		/// <summary>
		/// Reads a dictionary type from the XML data.
		/// The key type must implement IConvertible and the value type must implement
		/// IXleSerializable.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(string name)
			where TKey : IConvertible
			where TValue : IXleSerializable
		{
			XElement element = CurrentNode.Element(name);

			if (element == null)
				throw new XleSerializationException("Node " + name + " was not found.");

			try
			{
				nodes.Push(element);

				Dictionary<TKey, TValue> retval = new Dictionary<TKey, TValue>();

				foreach (var current in CurrentNode.Elements())
				{
					string keyString = current.Attribute("key").Value;
					TKey key = (TKey)Convert.ChangeType(keyString, typeof(TKey), CultureInfo.InvariantCulture);

					try
					{
						nodes.Push(current);
						TValue val = (TValue)DeserializeObject(typeof(TValue));

						retval.Add(key, val);
					}
					finally
					{
						nodes.Pop();
					}
				}

				return retval;
			}
			finally
			{
				nodes.Pop();
			}
		}
		/// <summary>
		/// Reads a dictionary type of strings from the XML data.
		/// The key type must implement IConvertible and the value type must implement
		/// IXleSerializable.
		/// </summary>
		/// <typeparam name="Tkey">The key type of the dictionary.</typeparam>
		/// <param name="name">The name of the element in the XML stream to decode.</param>
		/// <returns></returns> 
		[CLSCompliant(false)]
		[Obsolete("Use ReadDictionaryString instead.")]
		public Dictionary<Tkey, string> ReadDictionary<Tkey>(string name)
			where Tkey : IConvertible
		{
			return ReadDictionaryString<Tkey>(name);
		}
		/// <summary>
		/// Reads a dictionary type of strings from the XML data.
		/// The key type must implement IConvertible and the value type must implement
		/// IXleSerializable.
		/// </summary>
		/// <typeparam name="Tkey">The key type of the dictionary.</typeparam>
		/// <param name="name">The name of the element in the XML stream to decode.</param>
		/// <returns></returns> 
		[CLSCompliant(false)]
		public Dictionary<Tkey, string> ReadDictionaryString<Tkey>(string name)
			where Tkey : IConvertible
		{
			XElement element = CurrentNode.Element(name);

			try
			{
				nodes.Push(element);

				Dictionary<Tkey, string> retval = new Dictionary<Tkey, string>();

				foreach (var current in element.Elements())
				{
					string keyString = current.Attribute("key").Value;
					Tkey key = (Tkey)Convert.ChangeType(keyString, typeof(Tkey), CultureInfo.InvariantCulture);

					string valueString = current.Attribute("value").Value;

					retval.Add(key, valueString);
				}

				return retval;
			}
			finally
			{
				nodes.Pop();
			}

		}

		/// <summary>
		/// Reads a dictionary of the form Dictionary&lt;Tkey, int&gt;.
		/// </summary>
		/// <typeparam name="Tkey"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public Dictionary<Tkey, int> ReadDictionaryInt32<Tkey>(string name)
		{
			XElement element = CurrentNode.Element(name);

			try
			{
				nodes.Push(element);

				Dictionary<Tkey, int> retval = new Dictionary<Tkey, int>();

				foreach (var current in element.Elements())
				{
					string keyString = current.Attribute("key").Value;
					Tkey key = (Tkey)Convert.ChangeType(keyString, typeof(Tkey), CultureInfo.InvariantCulture);

					string valueString = current.Attribute("value").Value;

					retval.Add(key, int.Parse(valueString));
				}

				return retval;
			}
			finally
			{
				nodes.Pop();
			}
		}

		#endregion
		#region --- Reading streams ---
		/// <summary>
		/// Reads binary data stored as a stream.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Stream ReadStream(string name)
		{
			XElement element = CurrentNode.Element(name);

			if (element == null)
				throw new XleSerializationException("Field " + name + " was not found.");

			if (element.Attribute("stream") == null || element.Attribute("stream").Value != "true")
				throw new XleSerializationException("Field " + name + " is not a stream.");
			if (element.Attribute("encoding") == null)
				throw new XleSerializationException("Field " + name + " does not have encoding information.");

			string encoding = element.Attribute("encoding").Value;
			byte[] bytes;

			if (encoding == "Base64")
			{
				bytes = Convert.FromBase64String(element.Value);
			}
			else
				throw new XleSerializationException("Unrecognized encoding " + encoding);

			CompressionType compression = CompressionType.None;

			if (element.Attribute("compression") != null)
			{
				compression = (CompressionType)
					Enum.Parse(typeof(CompressionType), element.Attribute("compression").Value, true);
			}

			MemoryStream ms = new MemoryStream(bytes);
			ms.Position = 0;
			Stream uncompressed = TranslateStream(ms, compression, CompressionMode.Decompress);

			return uncompressed;
		}

		#endregion

		#endregion
		#region --- Type Binding ---

		private Type GetType(string name)
		{
			return Binder.GetType(name);
		}

		/// <summary>
		/// The ITypeBinder object used.
		/// </summary>
		public ITypeBinder Binder { get; private set; }

		#endregion
		#region --- Dealing with streams ---

		/// <summary>
		/// Reads data from a stream until the end is reached. The
		/// data is returned as a byte array. An IOException is
		/// thrown if any of the underlying IO calls fail.
		/// </summary>
		/// <param name="stream">The stream to read data from</param>
		static byte[] ReadFromStream(Stream stream)
		{
			// Code is from
			// http://www.yoda.arachsys.com/csharp/readbinary.html
			byte[] buffer = new byte[32768];
			using (MemoryStream ms = new MemoryStream())
			{
				while (true)
				{
					int read = stream.Read(buffer, 0, buffer.Length);
					if (read <= 0)
						return ms.ToArray();
					ms.Write(buffer, 0, read);
				}
			}
		}

		private Stream TranslateStream(Stream value, CompressionType compression, CompressionMode mode)
		{
			switch (compression)
			{
				case CompressionType.None:
					return value;

				default:
					throw new ArgumentException("Did not understand compression type.", "compression");
			}
		}

		#endregion

		internal object BeginDeserialize()
		{
			XElement root = doc.Element("XleRoot");

			if (root == null || root.Name != "XleRoot")
				throw new XleSerializationException("Could not understand stream.  Expected to find an XleRoot element, but found " + root.Name + ".");

			try
			{
				nodes.Push(root);

				object retval = DeserializeObject();

				System.Diagnostics.Debug.Assert(nodes.Count == 1);

				return retval;
			}
			finally
			{
				nodes.Pop();
			}
		}

		private object DeserializeObject()
		{
			return DeserializeObject(null);
		}
		private object DeserializeObject(Type defaultType)
		{
			XAttribute attrib = CurrentNode.Attribute("type");
			Type type = defaultType;

			if (attrib == null && defaultType == null)
				throw new XleSerializationException("Object lacks type information.");
			else if (attrib != null)
			{
				// load the type if it is not the default type.
				string typename = CurrentNode.Attribute("type").Value;

				if (typename == "null")
					return null;

				type = Binder.GetType(typename);

				if (type == null)
				{
					throw new XleSerializationException("Could not find Type object for " + typename + ".");
				}
			}

			if (typeof(IXleSerializable).IsAssignableFrom(type))
			{
				IXleSerializable obj;

				try
				{
					obj = (IXleSerializable)ObjectConstructor.CreateInstance(type);
				}
				catch (Exception e)
				{
					throw new XleSerializationException("Type " + type.ToString() + " threw an exception on construction.", e);
				}

				obj.ReadData(this);

				return obj;
			}
			else
			{
				var typser = TypeSerializers[type];

				if (typser == null)
					throw new XleSerializationException("Could not deserialize type " + type.ToString());

				return typser.Deserialize(this);
			}
		}

		public void WritePublicProperties(object item)
		{
			Type type = item.GetType();

			foreach (var prop in type.GetProperties())
			{
				prop_WriteValue(prop.Name, prop.GetValue(item, null));
			}
		}

		public void ReadPublicProperties(IXleSerializable item, bool allowDefaults = false)
		{
			Type type = item.GetType();

			foreach (var prop in type.GetProperties())
			{
				if (ContainsKey(prop.Name) == false && allowDefaults == true)
					continue;

				object value = prop_ReadValue(prop.Name, prop.PropertyType);

				prop.SetValue(item, prop_ReadValue(prop.Name, prop.PropertyType), null);
			}
		}

		private object prop_ReadValue(string name, Type t)
		{
			if (t == typeof(string)) return ReadString(name);
			else if (t == typeof(double)) return ReadDouble(name);
			else if (t == typeof(int)) return ReadInt32(name);
			else if (t == typeof(bool)) return ReadBoolean(name);
			else if (t == typeof(int[])) return ReadInt32Array(name);
			else if (t == typeof(IXleSerializable)) return ReadObject(name);

			else throw new NotImplementedException();
		}

		private void prop_WriteValue(string name, object value)
		{
			var type = value.GetType();

			if (value == null) Write(name, (IXleSerializable)null);
			else if (value is string) Write(name, (string)value);
			else if (value is int) Write(name, (int)value);
			else if (value is double) Write(name, (double)value);
			else if (value is bool) Write(name, (bool)value);
			else if (value is int[]) Write(name, (int[])value);
			else if (value is IXleSerializable) Write(name, (IXleSerializable)value);

			else throw new NotImplementedException();
		}

		private Type TypeOfValue(string item)
		{
			XElement el = CurrentNode.Element(item);

			return Binder.GetType(el.Attribute("type").Value);
		}
	}
}
