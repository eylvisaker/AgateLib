using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// Class which represents a resource.
    /// </summary>
    public abstract class AgateResource : ICloneable
    {
        private string mName;
        private string mLanguage = "Default";

        public AgateResource(string name)
        {
            mName = name; 
        }
        /// <summary>
        /// Name of the resource
        /// </summary>
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        /// <summary>
        /// Language this resource is to be used for.  If this is a language-independent
        /// resource, then the Language will be "Default".
        /// </summary>
        public string Language
        {
            get { return mLanguage; }
            private set { mLanguage = value; }
        }

        /// <summary>
        /// Serializes the AgateResource object to a subelement of parent.
        /// </summary>
        /// <param name="parent">The parent element of this resource.</param>
        /// <param name="doc">The XML document used to create elements.</param>
        internal abstract void BuildNodes(XmlElement parent, XmlDocument doc);

        #region --- ICloneable Members ---

        /// <summary>
        /// Override to construct a copy of the resource.
        /// </summary>
        /// <returns></returns>
        protected abstract AgateResource Clone();

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}
