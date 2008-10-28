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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// Resource which loads a surface. <br/>
    /// XML Attributes:<br/> 
    ///   string name, string filename
    /// <para>A zero or missing value for any width/height means it doesn't apply.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Not all attributes are used at the moment.  minimum_size and maximum_size can be specified and
    /// will be preserved if the resource file is loaded and saved, but they are not used in the construction
    /// of the DisplayWindow.
    /// </remarks>
    public sealed class SurfaceResource : AgateResource 
    {
        string mFilename;

        /// <summary>
        /// Gets or sets the filename for the surface to be created from.
        /// </summary>
        public string Filename { get { return mFilename; } set { mFilename = value; } }

        /// <summary>
        /// Constructs a SurfaceResource object.
        /// </summary>
        /// <param name="name"></param>
        public SurfaceResource(string name)
            : base(name)
        {
        }

        private SurfaceResource(string name, string filename)
            : base(name)
        {
            Filename = filename;
        }

        internal SurfaceResource(XmlNode node, string version)
            : base(string.Empty)
        {
            switch (version)
            {
                case "0.3.0":
                    Name = node.Attributes["name"].Value;
                    Filename = node.Attributes["filename"].Value;

                    break;
            }
        }
        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
            XmlElement el = doc.CreateElement("Surface");

            XmlHelper.AppendAttribute(el, doc, "name", Name);
            XmlHelper.AppendAttribute(el, doc, "filename", Filename);

            parent.AppendChild(el);
        }

        /// <summary>
        /// Clones the SurfaceResource object.
        /// </summary>
        /// <returns></returns>
        protected override AgateResource Clone()
        {
            return new SurfaceResource(Name, Filename);
        }
    }
}
