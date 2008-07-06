using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ERY.AgateLib.Resources
{
    public sealed class StringTable : AgateResource , IDictionary<string, string>
    {
        Dictionary<string, string> mTable = new Dictionary<string, string>();

        internal StringTable() : base ("StringTable")
        {}
        internal StringTable(XmlNode node, string version) : base("StringTable")
        {
            switch (version)
            {
                case "0.3.0":
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        XmlNode stringNode = node.ChildNodes[i];

                        if (stringNode.Name != "string")
                            throw new System.IO.InvalidDataException(
                                "Invalid node appeared in string table.");
                        if (stringNode.Attributes["name"] == null)
                            throw new System.IO.InvalidDataException(
                                "Unnamed string node found.");

                        string key = stringNode.Attributes["name"].Value;
                        string value = stringNode.InnerText;

                        mTable.Add(key, value);
                    }
                    break;
            }
        }

        internal void Combine(StringTable strings)
        {
            foreach (string key in strings.mTable.Keys)
            {
                mTable.Remove(key);
                mTable.Add(key, strings.mTable[key]);
            }
        }

        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
            XmlElement element = doc.CreateElement("StringTable");

            foreach(string keyName in mTable.Keys)
            {
                if (string.IsNullOrEmpty(mTable[keyName]))
                    continue;
                
                XmlElement key = doc.CreateElement("string");
                XmlHelper.AppendAttribute(key, doc, "name", keyName);

                key.InnerText = mTable[keyName];

                element.AppendChild(key);
            }

            parent.AppendChild(element);
        }

        protected override AgateResource Clone()
        {
            return new StringTable();
        }

        #region --- IDictionary<string,string> Members ---

        public void Add(string key, string value)
        {
            mTable.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return mTable.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return mTable.Keys; }
        }

        public bool Remove(string key)
        {
            return mTable.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return mTable.TryGetValue(key, out value);
        }

        public ICollection<string> Values
        {
            get { return mTable.Values; }
        }

        public string this[string key]
        {
            get
            {
                return mTable[key];
            }
            set
            {
                mTable[key] = value;
            }
        }

        #endregion
        #region --- ICollection<KeyValuePair<string,string>> Members ---

        void ICollection<KeyValuePair<string,string>> .Add(KeyValuePair<string, string> item)
        {
            ((ICollection<KeyValuePair<string, string>>)mTable).Add(item);
        }

        public void Clear()
        {
            mTable.Clear();
        }

        bool ICollection<KeyValuePair<string,string>> .Contains(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)mTable).Contains(item);
        }

        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, string>>)mTable).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mTable.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)mTable).Remove(item);
        }

        #endregion
        #region --- IEnumerable<KeyValuePair<string,string>> Members ---

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return mTable.GetEnumerator();
        }

        #endregion
        #region --- IEnumerable Members ---

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mTable.GetEnumerator();
        }

        #endregion

    }
}
