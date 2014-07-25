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
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using AgateLib.Utility;
using System.Diagnostics;
using System.Xml.Linq;

namespace AgateLib.Resources
{
	/// <summary>
	/// Static class which loads and saves AgateResourceCollection objects to disk.
	/// </summary>
	public static class AgateResourceLoader
	{
		/// <summary>
		/// Saves the resources to a file located in the Filename property.
		/// </summary>
		public static void SaveResources(AgateResourceCollection resources, string filename)
		{
			XDocument doc = new XDocument();
			XElement root = new XElement("AgateResources",
				new XAttribute("Version", "1.0.0"));

			doc.Add(root);

			foreach (AgateResource res in resources)
			{
				res.BuildNodes(root);
			}

			//doc.Save(filename);
			throw new NotImplementedException();
		}

		/// <summary>
		/// Static method which creates a resource manager based on the contents of a file.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		[Obsolete("Use new AgateResourceCollection instead.")]
		public static AgateResourceCollection LoadResources(string filename)
		{
			return new AgateResourceCollection(filename);
		}

		/// <summary>
		/// Loads the resource information from a stream containing XML data.
		/// This erases all information in the current AgateResourceCollection.
		/// </summary>
		/// <param name="resources"></param>
		/// <param name="stream"></param>
		public static void LoadResources(AgateResourceCollection resources, Stream stream)
		{
			XDocument doc = new XDocument();
			try
			{
				doc = XDocument.Load(new StreamReader(stream));
			}
			catch (XmlException e)
			{
				throw new AgateResourceException("The XML resource file is malformed.", e);
			}

			XElement root = doc.Elements().First();

			if (root.Attribute("Version") == null)
				throw new AgateResourceException("XML resource file does not contain the required version attibute.");

			string version = root.Attribute("Version").Value;

			switch (version)
			{
				case "1.0.0":
				case "0.4.0":
				case "0.3.5":
				case "0.3.2":
				case "0.3.1":
				case "0.3.0":
					ReadVersion100(resources, root, version);
					break;

				default:
					throw new AgateResourceException("XML Resource file version " + version + " not supported.");
			}
		}


		private static void ReadVersion100(AgateResourceCollection resources, XElement root, string version)
		{
			foreach (var node in root.Elements())
			{
				try
				{
					resources.Add(ReadNode(node, version));
				}
				catch (Exception e)
				{
					throw new AgateResourceException("An error occurred while reading XML resource file: \r\n"
						+ e.Message, e);
				}
			}
		}

		private static AgateResource ReadNode(XElement node, string version)
		{
			switch (node.Name.LocalName)
			{
				case "StringTable":
					return new StringTable(node, version);

				case "DisplayWindow":
					return new DisplayWindowResource(node, version);

				case "Image":
					return new ImageResource(node, version);

				case "Surface":
					return new SurfaceResource(node, version);

				case "Sprite":
					return new SpriteResource(node, version);

				case "BitmapFont":
					return new BitmapFontResource(node, version);

				default:
					ReadError("Unrecognized node " + node.Name + ".");
					return null;
			}
		}
		private static void ReadError(string message)
		{
			if (ThrowOnReadError)
			{
				throw new InvalidOperationException(message);
			}
			else
				Debug.WriteLine(message);
		}

		static bool mThrowOnReadError = true;

		/// <summary>
		/// Gets or sets a bool value indicating whether or not an exception
		/// is thrown if an unknown resource type is encountered when reading
		/// an XML file.
		/// </summary>
		public static bool ThrowOnReadError
		{
			get { return mThrowOnReadError; }
			set { mThrowOnReadError = value; }
		}

	}
}
