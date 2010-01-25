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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;

namespace AgateLib.Serialization.Xle
{
	/// <summary>
	/// The XleSerializationInfo class contains the XML data that is read from 
	/// or written to when doing XLE serialization.
	/// </summary>
	public class XleSerializationInfo
	{
		XmlDocument doc;
		Stack<XmlElement> nodes = new Stack<XmlElement>();

		/// <summary>
		/// The ITypeBinder object used.
		/// </summary>
		public ITypeBinder Binder { get; internal set; }

		internal XleSerializationInfo()
		{
			doc = new XmlDocument();
		}
		internal XleSerializationInfo(XmlDocument doc)
		{
			this.doc = doc;
		}

		internal XmlDocument XmlDoc
		{
			get { return doc; }
		}

		XmlElement CurrentNode
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

		void AddAttribute(XmlNode node, string name, string value)
		{
			XmlAttribute attrib = doc.CreateAttribute(name);
			attrib.Value = value;

			node.Attributes.Append(attrib);
		}

		internal void BeginSerialize(IXleSerializable objectGraph)
		{
			var root = doc.CreateElement("XleRoot");

			AddAttribute(root, "type", objectGraph.GetType().ToString());

			doc.AppendChild(root);

			nodes.Push(root);
			Serialize(objectGraph);

			System.Diagnostics.Debug.Assert(nodes.Count == 1);
			nodes.Clear();
		}

		#region --- Writing methods ---

		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, string value)
		{
			if (value == null) value = "";

			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, double value)
		{
			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, float value)
		{
			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, bool value)
		{
			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, char value)
		{
			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, short value)
		{
			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, int value)
		{
			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, long value)
		{
			Write(name, value, false);
		}
		/// <summary>
		/// Writes a field to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		public void Write(string name, decimal value)
		{
			Write(name, value, false);
		}


		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, string value, bool asAttribute)
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
		public void WriteEnum<T>(string name, T value, bool asattribute) where T : struct
		{
			if (typeof(T).IsEnum == false)
				throw new XleSerializationException("Type passed is not an enum.");

			WriteImpl(name, value.ToString(), asattribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, double value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, float value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, bool value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, char value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, short value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, int value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, long value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}
		/// <summary>
		/// Writes a field to the XML data as an element or an attribute.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="asAttribute">Pass true to write the field as an attribute in the parent element.</param>
		public void Write(string name, decimal value, bool asAttribute)
		{
			WriteImpl(name, value, asAttribute);
		}

		void WriteImpl<T>(string name, T value) where T : IConvertible
		{
			WriteImpl(name, value, false);
		}
		void WriteImpl<T>(string name, T value, bool asAttribute) where T : IConvertible
		{
			if (asAttribute)
			{
				WriteAsAttribute<T>(name, value);
			}
			else
				WriteAsElement(name, value);

		}

		/// <summary>
		/// Writes an int[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, int[] value)
		{
			byte[] array = new byte[value.Length * 4];

			if (array.Length > 0)
			{
				unsafe
				{
					fixed (int* val = value)
					{
						Marshal.Copy((IntPtr)val, array, 0, array.Length);
					}
				}
			}
			Write(name, array);

		}


		/// <summary>
		/// Writes a bool[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, bool[] value)
		{
			byte[] array = new byte[value.Length];

			for (int i = 0; i < value.Length; i++)
				array[i] = (byte)(value[i] ? 1 : 0);
			
			Write(name, array);
		}

		/// <summary>
		/// Writes an double[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, double[] value)
		{
			byte[] array = new byte[value.Length * 8];

			unsafe
			{
				fixed (double* val = value)
				{
					Marshal.Copy((IntPtr)val, array, 0, array.Length);
				}
			}
			Write(name, array);

		}
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
			MemoryStream ms = new MemoryStream();
			Stream compressed = TranslateStream(ms, compression, CompressionMode.Compress);

			byte[] uncompressedData = ReadFromStream(value);
			compressed.Write(uncompressedData, 0, uncompressedData.Length);

			byte[] buffer = ms.GetBuffer();

			string newValue = Convert.ToBase64String(
				buffer, Base64FormattingOptions.InsertLineBreaks);

			XmlElement el = WriteAsElement(name, newValue);
			AddAttribute(el, "stream", "true");
			AddAttribute(el, "compression", compression.ToString());
			AddAttribute(el, "encoding", "Base64");
		}

		/// <summary>
		/// Writes a byte[] array to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The array data to write.</param>
		public void Write(string name, byte[] value)
		{
			string newValue = Convert.ToBase64String(value, Base64FormattingOptions.InsertLineBreaks);

			XmlElement el = WriteAsElement(name, newValue);
			AddAttribute(el, "array", "true");
			AddAttribute(el, "encoding", "Base64");
		}
		/// <summary>
		/// Writes an object implementing IXleSerializable to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The object data to write.</param>
		public void Write(string name, IXleSerializable value)
		{
			XmlElement element = CreateElement(name);
			AddAttribute(element, "type", value.GetType().ToString());

			nodes.Push(element);

			Serialize(value);

			nodes.Pop();
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

			XmlElement element = CreateElement(name);
			AddAttribute(element, "array", "true");
			AddAttribute(element, "type", listType.ToString());

			nodes.Push(element);

			for (int i = 0; i < value.Count; i++)
			{
				XmlElement item = doc.CreateElement("Item");
				CurrentNode.AppendChild(item);

				if (value[i] == null)
					AddAttribute(item, "type", "null");
				else
				{
					if (value[i].GetType() != listType)
						AddAttribute(item, "type", value[i].GetType().ToString());

					nodes.Push(item);
					Serialize(value[i]);
					nodes.Pop();
				}
			}

			nodes.Pop();
		}
		/// <summary>
		/// Writes a List&lt;T&gt; of strings to the XML data as an element.
		/// </summary>
		/// <param name="name">The name of the XML element used.</param>
		/// <param name="value">The list data to write.</param>
		public void Write(string name, List<string> value) 
		{
			XmlElement element = CreateElement(name);
			AddAttribute(element, "array", "true");
			
			nodes.Push(element);

			for (int i = 0; i < value.Count; i++)
			{
				XmlElement item = doc.CreateElement("Item");
				CurrentNode.AppendChild(item);
				item.InnerText = value[i];
			}

			nodes.Pop();
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

			XmlElement element = CreateElement(name);
			AddAttribute(element, "dictionary", "true");
			//AddAttribute(element, "keytype", keyType.ToString());
			//AddAttribute(element, "valuetype", valueType.ToString());

			nodes.Push(element);

			foreach (KeyValuePair<Tkey, Tvalue> kvp in value)
			{
				XmlElement item = doc.CreateElement("Item");
				CurrentNode.AppendChild(item);

				AddAttribute(item, "key", kvp.Key.ToString());

				//if (keyType != kvp.Key.GetType())
				//    AddAttribute(item, "IDtype", kvp.Key.GetType().ToString());

				if (kvp.Value.GetType() != valueType)
					AddAttribute(item, "type", kvp.Value.GetType().ToString());

				nodes.Push(item);
				Serialize(kvp.Value);
				nodes.Pop();
			}

			nodes.Pop();
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

			XmlElement element = CreateElement(name);
			//AddAttribute(element, "dictionary", "true");
			//AddAttribute(element, "keytype", keyType.ToString());
			//AddAttribute(element, "valuetype", valueType.ToString());

			nodes.Push(element);

			foreach (KeyValuePair<Tkey, int> kvp in value)
			{
				XmlElement item = doc.CreateElement("Item");
				CurrentNode.AppendChild(item);

				AddAttribute(item, "key", kvp.Key.ToString());
				AddAttribute(item, "value", kvp.Value.ToString());
			}

			nodes.Pop();
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

			XmlElement element = CreateElement(name);
			//AddAttribute(element, "dictionary", "true");
			//AddAttribute(element, "keytype", keyType.ToString());
			//AddAttribute(element, "valuetype", typeof(string).ToString());

			nodes.Push(element);

			foreach (KeyValuePair<Tkey, string> kvp in value)
			{
				XmlElement item = doc.CreateElement("Item");
				CurrentNode.AppendChild(item);

				AddAttribute(item, "key", kvp.Key.ToString());

				//if (keyType != kvp.Key.GetType())
				//    AddAttribute(item, "keytype", kvp.Key.GetType().ToString());

				AddAttribute(item, "value", kvp.Value.ToString());
			}

			nodes.Pop();
		}

		private XmlElement WriteAsElement<T>(string name, T value) where T : IConvertible
		{
			XmlElement element = doc.CreateElement(name);

			element.InnerText = value.ToString();

			CurrentNode.AppendChild(element);

			return element;
		}
		private void WriteAsAttribute<T>(string name, T value) where T : IConvertible
		{
			AddAttribute(CurrentNode, name, Convert.ToString(value));
		}

		private XmlElement CreateElement(string name)
		{
			XmlElement element = doc.CreateElement(name);

			for (int i = 0; i < CurrentNode.ChildNodes.Count; i++)
			{
				if (CurrentNode.ChildNodes[i].Name == name)
					throw new XleSerializationException("The name " + name + " already exists.");
			}

			CurrentNode.AppendChild(element);

			return element;
		}


		#endregion
		#region --- Reading methods ---


		private Type GetType(string name)
		{
			return Binder.GetType(name);
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
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new XleSerializationException("Node " + name + " was not found.");

			nodes.Push(element);

			Dictionary<TKey, TValue> retval = new Dictionary<TKey, TValue>();

			for (int i = 0; i < element.ChildNodes.Count; i++)
			{
				XmlElement current = (XmlElement)CurrentNode.ChildNodes[i];
				string keyString = current.GetAttribute("key");
				TKey key = (TKey)Convert.ChangeType(keyString, typeof(TKey));

				nodes.Push(current);
				TValue val = (TValue)DeserializeObject(typeof(TValue));
				nodes.Pop();

				retval.Add(key, val);
			}

			nodes.Pop();

			return retval;
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
			XmlElement element = (XmlElement)CurrentNode[name];

			nodes.Push(element);

			Dictionary<Tkey, string> retval = new Dictionary<Tkey, string>();

			for (int i = 0; i < element.ChildNodes.Count; i++)
			{
				XmlElement current = (XmlElement)CurrentNode.ChildNodes[i];
				string keyString = current.GetAttribute("key");
				Tkey key = (Tkey)Convert.ChangeType(keyString, typeof(Tkey));

				string valueString = current.GetAttribute("value");

				retval.Add(key, valueString);
			}

			nodes.Pop();

			return retval;
		}

		public Dictionary<Tkey, int> ReadDictionaryInt32<Tkey>(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			nodes.Push(element);

			Dictionary<Tkey, int> retval = new Dictionary<Tkey, int>();

			for (int i = 0; i < element.ChildNodes.Count; i++)
			{
				XmlElement current = (XmlElement)CurrentNode.ChildNodes[i];
				string keyString = current.GetAttribute("key");
				Tkey key = (Tkey)Convert.ChangeType(keyString, typeof(Tkey));

				string valueString = current.GetAttribute("value");

				retval.Add(key, int.Parse(valueString));
			}

			nodes.Pop();

			return retval;
		}
		/// <summary>
		/// Reads an object from the XML data.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object ReadObject(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

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
		/// <summary>
		/// Reads binary data stored as a stream.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Stream ReadStream(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new XleSerializationException("Field " + name + " was not found.");

			if (element.Attributes["stream"] == null || element.Attributes["stream"].Value != "true")
				throw new XleSerializationException("Field " + name + " is not a stream.");
			if (element.Attributes["encoding"] == null)
				throw new XleSerializationException("Field " + name + " does not have encoding information.");

			string encoding = element.Attributes["encoding"].Value;
			byte[] bytes;

			if (encoding == "Base64")
			{
				bytes = Convert.FromBase64String(element.InnerText);
			}
			else
				throw new XleSerializationException("Unrecognized encoding " + encoding);

			CompressionType compression = CompressionType.None;

			if (element.Attributes["compression"] != null)
			{
				compression = (CompressionType)
					Enum.Parse(typeof(CompressionType), element.Attributes["compression"].Value, true);
			}

			MemoryStream ms = new MemoryStream(bytes);
			ms.Position = 0;
			Stream uncompressed = TranslateStream(ms, compression, CompressionMode.Decompress);

			return uncompressed;
		}

		private string ReadStringImpl(string name, bool haveDefault, string defaultValue)
		{
			string attribute = CurrentNode.GetAttribute(name);

			if (string.IsNullOrEmpty(attribute) == false)
				return attribute;

			XmlElement element = (XmlElement)CurrentNode[name];

			if (element != null)
			{
				if (element.Attributes["encoding"] != null)
				{
					throw new XleSerializationException("Cannot decode encoded strings.");
				}

				return element.InnerText;
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
			string attribute = CurrentNode.GetAttribute(name);

			if (string.IsNullOrEmpty(attribute) == false)
				return bool.Parse(attribute);

			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			return bool.Parse(element.InnerText);
		}
		/// <summary>
		/// Reads a integer value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public int ReadInt32(string name)
		{
			string attribute = CurrentNode.GetAttribute(name);

			if (string.IsNullOrEmpty(attribute) == false)
				return int.Parse(attribute);

			XmlElement element = (XmlElement)CurrentNode[name];

			if (element != null)
			{
				return int.Parse(element.InnerText);
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
			string attribute = CurrentNode.GetAttribute(name);

			if (string.IsNullOrEmpty(attribute) == false)
				return int.Parse(attribute);

			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				return defaultValue;

			return int.Parse(element.InnerText);
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

			return (T)Enum.Parse(typeof(T), ReadStringImpl(name, false, string.Empty));
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

			return (T)Enum.Parse(typeof(T), ReadStringImpl(name, true, defaultValue.ToString()));
		}

		/// <summary>
		/// Reads a double value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public double ReadDouble(string name)
		{
			string attribute = CurrentNode.GetAttribute(name);

			if (string.IsNullOrEmpty(attribute) == false)
				return double.Parse(attribute);

			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			return double.Parse(element.InnerText);
		}

		/// <summary>
		/// Reads a float value from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public float ReadFloat(string name)
		{
			string attribute = CurrentNode.GetAttribute(name);

			if (string.IsNullOrEmpty(attribute) == false)
				return float.Parse(attribute);

			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			return float.Parse(element.InnerText);
		}
		/// <summary>
		/// Reads a integer array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public int[] ReadInt32Array(string name)
		{
			byte[] array = ReadByteArray(name);
			int[] result = new int[array.Length / 4];

			if (array.Length % 4 != 0)
				throw new XleSerializationException("Encoded array is wrong size!");

			if (array.Length > 0)
			{
				unsafe
				{
					fixed (byte* ar = array)
					{
						Marshal.Copy((IntPtr)ar, result, 0, result.Length);
					}
				}
			}

			return result;
		}
		/// <summary>
		/// Reads a boolean array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public bool[] ReadBooleanArray(string name)
		{
			byte[] array = ReadByteArray(name);
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
		public double[] ReadDoubleArray(string name)
		{
			byte[] array = ReadByteArray(name);
			double[] result = new double[array.Length / 8];

			if (array.Length % 8 != 0)
				throw new XleSerializationException("Encoded array is wrong size!");

			unsafe
			{
				fixed (byte* ar = array)
				{
					Marshal.Copy((IntPtr)ar, result, 0, result.Length);
				}
			}

			return result;
		}

		/// <summary>
		/// Reads an array of objects from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public T[] ReadArray<T>(string name)
		{
			if (typeof(T) == typeof(int) || typeof(T) == typeof(double))
				throw new XleSerializationException("Cannot use the generic ReadArray method to deserialize an array of primitives.");

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
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			if (element.Attributes["array"] == null || element.Attributes["array"].Value != "true")
				throw new XleSerializationException("Element " + name + " is not an array.");
			if (element.Attributes["type"] == null && defaultType == null)
				throw new XleSerializationException("Element " + name + " does not have type information.");

			Type type;
			if (element.Attributes["type"] == null)
				type = defaultType;
			else
			{
				string typename = element.Attributes["type"].Value;

				type = GetType(typename);

				if (type == null)
					throw new XleSerializationException("Could not find type for " + typename + ".");
			}

			Type listType = typeof(List<>).MakeGenericType(type);
			Type arrayType = type.MakeArrayType();
			System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(listType);

			for (int i = 0; i < element.ChildNodes.Count; i++)
			{
				XmlElement item = (XmlElement)element.ChildNodes[i];

				if (item.Name != "Item")
					throw new XleSerializationException("Could not understand data.  Expected Item, found " + item.Name + ".");

				if (type == typeof(string))
				{
					list.Add(item.InnerText);
				}
				else 
				{
					nodes.Push(item);

					object o = DeserializeObject(type);
					list.Add(o);

					nodes.Pop();
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

			List<T> retval = new List<T>();
			retval.AddRange((T[])ar);

			return retval;
		}


		/// <summary>
		/// Reads a byte array from the XML data.  If the name is not present 
		/// an XleSerializationException is thrown.
		/// </summary>
		/// <param name="name">Name of the field.</param>
		/// <returns></returns>
		public byte[] ReadByteArray(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new XleSerializationException("Node " + name + " not found.");

			if (element.Attributes["array"] == null || element.Attributes["array"].Value != "true")
				throw new XleSerializationException("Element " + name + " is not an array.");
			if (element.Attributes["encoding"] == null)
				throw new XleSerializationException("Element " + name + " does not have encoding information.");

			if (element.Attributes["encoding"].Value == "Base64")
			{
				byte[] array = Convert.FromBase64String(element.InnerText);
				return array;
			}
			else
			{
				throw new XleSerializationException("Unrecognized encoding " + element.Attributes["encoding"]);
			}

		}



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
				case CompressionType.Deflate:
					return new DeflateStream(value, mode, true);
				case CompressionType.GZip:
					return new GZipStream(value, mode, true);

				default:
					throw new ArgumentException("Did not understand compression type.", "compression");
			}
		}

		#endregion


		internal object BeginDeserialize()
		{
			XmlElement root = (XmlElement)doc.ChildNodes[0];

			if (root.Name != "XleRoot")
				throw new XleSerializationException("Could not understand stream.  Expected to find an XleRoot element, but found " + root.Name + ".");

			nodes.Push(root);

			object retval = DeserializeObject();

			System.Diagnostics.Debug.Assert(nodes.Count == 1);
			nodes.Pop();

			return retval;
		}

		private object DeserializeObject()
		{
			return DeserializeObject(null);
		}
		private object DeserializeObject(Type defaultType)
		{
			XmlAttribute attrib = CurrentNode.Attributes["type"];
			Type type = defaultType;

			if (attrib == null && defaultType == null)
				throw new XleSerializationException("Object lacks type information.");
			else if (attrib != null)
			{
				// load the type if it is not the default type.
				string typename = CurrentNode.Attributes["type"].Value;

				if (typename == "null")
					return null;

				type = Binder.GetType(typename);

				if (type == null)
				{
					throw new XleSerializationException("Could not find Type object for " + typename + ".");
				}
			}

			IXleSerializable obj;

			try
			{
				obj = (IXleSerializable)Activator.CreateInstance(type, true);
			}
			catch (MissingMethodException e)
			{
				throw new XleSerializationException("Type " + type.ToString() + " does not have a default constructor.", e);
			}

			obj.ReadData(this);

			return obj;
		}


	}
}
