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
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AgateLib.Resources
{
    /// <summary>
    /// Class which represents a resource.
    /// </summary>
    public abstract class AgateResource : ICloneable
    {
        private string mName;
        private string mLanguage = "Default";

        /// <summary>
        /// Constructs a base resource object.
        /// </summary>
        /// <param name="name"></param>
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
