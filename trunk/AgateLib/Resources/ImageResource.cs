using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AgateLib.DisplayLib;

namespace AgateLib.Resources
{
	public class ImageResource : AgateResource 
	{
		Surface mBackingSurface;
		List<SurfaceResource> mSurfaces = new List<SurfaceResource>();

		public ImageResource()
		{ }

		internal ImageResource(XmlNode node, string version)
		{
			if (node.Attributes["filename"] == null)
				throw new AgateResourceException("Image node did not include the required filename attribute.");

			switch (version)
			{
				case "0.3.2":
					Filename = node.Attributes["filename"].Value;
					ReadSubNodes032(node);
					break;

				default:
					throw new AgateResourceException("Image resource not supported in file version.");
			}
		}

		private void ReadSubNodes032(XmlNode node)
		{
			foreach (XmlNode n in node.ChildNodes)
			{
				switch (n.Name)
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

		private void ReadSprite(XmlNode n)
		{
			throw new NotImplementedException();
		}

		private void ReadSurface(XmlNode n)
		{
			SurfaceResource res = new SurfaceResource(XmlHelper.ReadAttributeString(n, "name"));

			if (n.Attributes["left"] != null ||
				n.Attributes["top"] != null ||
				n.Attributes["width"] != null ||
				n.Attributes["height"] != null)
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
				mBackingSurface = new Surface(FileProvider, Filename);
			}

			return mBackingSurface;
		}
		internal override void BuildNodes(XmlElement parent, XmlDocument doc)
		{
			throw new NotImplementedException();
		}

		protected override AgateResource Clone()
		{
			throw new NotImplementedException();
		}

	}
}
