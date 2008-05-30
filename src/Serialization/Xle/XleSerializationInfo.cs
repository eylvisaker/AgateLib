using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;

namespace ERY.AgateLib.Serialization.Xle
{
    public class XleSerializationInfo
    {
        XmlDocument doc;
        Stack<XmlElement> nodes = new Stack<XmlElement>();

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

        public XmlElement CurrentNode
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
            var root = doc.CreateElement("Root");

            AddAttribute(root, "type", objectGraph.GetType().ToString());
            
            doc.AppendChild(root);

            nodes.Push(root);
            Serialize(objectGraph);

            System.Diagnostics.Debug.Assert(nodes.Count == 1);
            nodes.Clear();
        }

        #region --- Writing methods ---

        
        public void Write(string name, string value)
        {
            if (value == null) value = "";

            Write(name, value, false);
        }
        public void Write(string name, double value)
        {
            Write(name, value, false);
        }
        public void Write(string name, float value)
        {
            Write(name, value, false);
        }
        public void Write(string name, bool value)
        {
            Write(name, value, false);
        }
        public void Write(string name, char value)
        {
            Write(name, value, false);
        }
        public void Write(string name, short value)
        {
            Write(name, value, false);
        }
        public void Write(string name, int value)
        {
            Write(name, value, false);
        }
        public void Write(string name, long value)
        {
            Write(name, value, false);
        }
        public void Write(string name, decimal value)
        {
            Write(name, value, false);
        }

        public void Write(string name, string value, bool asAttribute)
        {
            if (value == null) value = "";

            WriteImpl(name, value ?? string.Empty, asAttribute);
        }
        public void Write(string name, double value, bool asAttribute)
        {
            WriteImpl(name, value, asAttribute);
        }
        public void Write(string name, float value, bool asAttribute)
        {
            WriteImpl(name, value, asAttribute);
        }
        public void Write(string name, bool value, bool asAttribute)
        {
            WriteImpl(name, value, asAttribute);
        }
        public void Write(string name, char value, bool asAttribute)
        {
            WriteImpl(name, value, asAttribute);
        }
        public void Write(string name, short value, bool asAttribute)
        {
            WriteImpl(name, value, asAttribute);
        }
        public void Write(string name, int value, bool asAttribute)
        {
            WriteImpl(name, value, asAttribute);
        }
        public void Write(string name, long value, bool asAttribute)
        {
            WriteImpl(name, value, asAttribute);
        }
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

        public void Write(string name, int[] value)
        {
            byte[] array = new byte[value.Length * 4];

            unsafe
            {
                fixed (int* val = value)
                {
                    Marshal.Copy((IntPtr)val, array, 0, array.Length);
                }
            }
            Write(name, array);

        }
        public void Write(string name, byte[] value)
        {
            string newValue = Convert.ToBase64String(value, Base64FormattingOptions.None);

            XmlElement el = WriteAsElement(name, newValue);
            AddAttribute(el, "array", "true");
            AddAttribute(el, "encoding", "Base64");
        }
        public void Write(string name, IXleSerializable value)
        {
            XmlElement element = CreateElement(name);
            AddAttribute(element, "type", value.GetType().ToString());

            nodes.Push(element);

            Serialize(value);

            nodes.Pop();
        }

        public void Write<T>(string name, T[] value) where T : IXleSerializable
        {
            List<T> list = new List<T>();
            list.AddRange(value);

            Write(name, list);
        }
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

                if (value[i].GetType() != listType)
                    AddAttribute(item, "type", value[i].GetType().ToString());

                nodes.Push(item);
                Serialize(value[i]);
                nodes.Pop();
            }

            nodes.Pop();
        }
        public void Write<Tkey, Tvalue>(string name, Dictionary<Tkey, Tvalue> value)
            where Tkey : IConvertible
            where Tvalue : IXleSerializable
        {
            Type keyType = typeof(Tkey);
            Type valueType = typeof(Tvalue);

            XmlElement element = CreateElement(name);
            //AddAttribute(element, "dictionary", "true");
            //AddAttribute(element, "keytype", keyType.ToString());
            //AddAttribute(element, "valuetype", valueType.ToString());

            nodes.Push(element);

            foreach(KeyValuePair<Tkey, Tvalue> kvp in value)
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
        public Dictionary<TKey, string> ReadDictionary<TKey>(string name)
            where TKey : IConvertible
        {
            XmlElement element = (XmlElement)CurrentNode[name];

            nodes.Push(element);

            Dictionary<TKey, string> retval = new Dictionary<TKey, string>();

            for (int i = 0; i < element.ChildNodes.Count; i++)
            {
                XmlElement current = (XmlElement)CurrentNode.ChildNodes[i];
                string keyString = current.GetAttribute("key");
                TKey key = (TKey)Convert.ChangeType(keyString, typeof(TKey));

                string valueString = current.GetAttribute("value");

                retval.Add(key, valueString);
            }

            nodes.Pop();

            return retval;
        }

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
        public string ReadString(string name, string defaultValue)
        {
            return ReadStringImpl(name, true, defaultValue);
        }
        public string ReadString(string name)
        {
            return ReadStringImpl(name, false, string.Empty);
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

        public int[] ReadInt32Array(string name)
        {
            byte[] array = ReadByteArray(name);
            int[] result = new int[array.Length / 4];

            if (array.Length % 4 != 0)
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

        public T[] ReadArray<T>(string name)
        {
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

        public Array ReadArray(string name)
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
            System.Collections.IList list = (System.Collections.IList) Activator.CreateInstance(listType);

            for (int i = 0; i < element.ChildNodes.Count; i++)
            {
                XmlElement item = (XmlElement)element.ChildNodes[i];

                if (item.Name != "Item")
                    throw new XleSerializationException("Could not understand data.  Expected Item, found " + item.Name + ".");

                nodes.Push(item);

                object o = DeserializeObject(type);
                list.Add(o);

                nodes.Pop();
            }

            Array retval = (Array)Activator.CreateInstance(arrayType, list.Count);
            list.CopyTo(retval, 0);

            return retval;
        }
        public List<T> ReadList<T>(string name)
        {
            Array ar = ReadArrayImpl(name, typeof(T));

            List<T> retval = new List<T>();
            retval.AddRange((T[])ar);

            return retval;
        }


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



        internal object BeginDeserialize()
        {
            XmlElement root = (XmlElement)doc.ChildNodes[0];

            if (root.Name != "Root")
                throw new XleSerializationException("Could not understand stream.  Expected to find a Root element, but found " + root.Name + ".");

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
