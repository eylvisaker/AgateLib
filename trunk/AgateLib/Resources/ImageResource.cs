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
using System.Xml;
using AgateLib.DisplayLib;
using System.Xml.Linq;

namespace AgateLib.Resources
{
	public class ImageResource : AgateResource 
	{
		Surface mBackingSurface;
		List<SurfaceResource> mSurfaces = new List<SurfaceResource>();

		public ImageResource()
		{ }

		internal ImageResource(XElement node, string version)
		{
			if (node.Attribute("filename") == null)
				throw new AgateResourceException("Image node did not include the required filename attribute.");

			switch (version)
			{
				case "0.3.2":
					Filename = node.Attribute("filename").Value;
					ReadSubNodes032(node);
					break;

				default:
					throw new AgateResourceException("Image resource not supported in file version.");
			}
		}

		private void ReadSubNodes032(XElement node)
		{
			foreach (XElement n in node.Elements())
			{
				switch (n.Name.LocalName)
				{
					case "Surface":
						ReadSurface(n);
						break;

					case "Sprite":
						ReadSprite(n);
						break;

					default:
						throw new AgateResourceException(
							"Could not understand node {0} which is a subnode of Image {1}", n.Name, Filename);
				}
			}
		}

		private void ReadSprite(XElement n)
		{
			throw new NotImplementedException();
		}

		private void ReadSurface(XElement n)
		{
			SurfaceResource res = new SurfaceResource(XmlHelper.ReadAttributeString(n, "name"));

			if (n.Attribute("left") != null ||
				n.Attribute("top") != null ||
				n.Attribute("width") != null ||
				n.Attribute("height") != null)
			{
				res.SourceRect = new AgateLib.Geometry.Rectangle(
					XmlHelper.ReadAttributeInt(n, "left"),
					XmlHelper.ReadAttributeInt(n, "top"),
					XmlHelper.ReadAttributeInt(n, "width"),
					XmlHelper.ReadAttributeInt(n, "height"));
			}

			res.Image = this;
			mSurfaces.Add(res);
		}

		public string Filename { get; set; }
		public IFileProvider FileProvider { get; set; }

		public List<SurfaceResource> Surfaces { get { return mSurfaces; } }

		public Surface GetOrLoadSurface()
		{
			if (mBackingSurface == null)
			{
				mBackingSurface = new Surface(Filename);
			}

			return mBackingSurface;
		}
		internal override void BuildNodes(XElement parent)
		{
			throw new NotImplementedException();
		}

		protected override AgateResource Clone()
		{
			throw new NotImplementedException();
		}

	}
}
