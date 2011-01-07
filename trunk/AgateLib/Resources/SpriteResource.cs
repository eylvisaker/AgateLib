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

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Sprites;

namespace AgateLib.Resources
{
	/// <summary>
	/// Resource which loads a sprite.  There are two different types of sprites, a general purpose
	/// sprite, implemented as the Sprite class, and a packed version which maximizes the memory
	/// efficiency, implemented as PackedSprite.<br/>
	/// XML Attributes:<br/> 
	///   string name, bool packed, Size size, string image, double timePerFrame (in milliseconds)
	///   
	/// XML Nodes:<br/>
	///     Image:
	///         Required attributes:
	///             string file
	///         Optional attribute:
	///             Point offset
	///     Frame:
	///         Required attributes:
	///             Rectangle rect, Point offset
	///         Optional attribute:
	///             string image
	/// </summary>
	public class SpriteResource : AgateResource
	{
		Size mSize;
		string mFilename = string.Empty;
		bool mPacked = false;
		double mTimePerFrame = 60;
		bool mHasSize = false;

		List<SpriteSubResource> mImages = new List<SpriteSubResource>();

		/// <summary>
		/// Gets a boolean indicating whether or not the sprite size was specified in the resource
		/// file.
		/// </summary>
		public bool HasSize
		{
			get { return mHasSize; }
		}

		/// <summary>
		/// Gets or sets whether or not this sprite uses the PackedSprite class.
		/// </summary>
		public bool Packed
		{
			get { return mPacked; }
			set
			{
				for (int i = 0; i < ChildElements.Count; i++)
				{
					if (ChildElements[i].Filename != Filename)
						throw new AgateException("Sprite is not packed.");
				}

				mPacked = value;
			}
		}
		/// <summary>
		/// Gets or sets the size of this sprite.
		/// </summary>
		public Size Size
		{
			get { return mSize; }
			set { mSize = value; }
		}

		/// <summary>
		/// Gets or sets the amount of time each frame should be displayed, in milliseconds.
		/// </summary>
		public double TimePerFrame
		{
			get { return mTimePerFrame; }
			set { mTimePerFrame = value; }
		}
		/// <summary>
		/// Gets the list of frames 
		/// </summary>
		public List<SpriteSubResource> ChildElements
		{
			get { return mImages; }
		}

		/// <summary>
		/// Gets or sets the default filename for sprite frames.
		/// </summary>
		public string Filename
		{
			get { return mFilename; }
			set { mFilename = value; }
		}

		/// <summary>
		/// Constructs a SpriteResource object.
		/// </summary>
		/// <param name="name"></param>
		public SpriteResource(string name)
			: base(name)
		{

		}


		internal SpriteResource(XmlNode node, string version)
			: base(string.Empty)
		{
			switch (version)
			{
				case "0.3.2":
				case "0.3.1":
					Name = node.Attributes["name"].Value;
					mFilename = XmlHelper.ReadAttributeString(node, "image", string.Empty);
					mPacked = XmlHelper.ReadAttributeBool(node, "packed", true);

					if (node.Attributes["size"] != null)
					{
						mSize = XmlHelper.ReadAttributeSize(node, "size");
						mHasSize = true;
					}
					else
						mHasSize = false;

					ReadFrames031(node);

					// check and make sure the sprite can be packed, and this matches the packed attribute 
					if (mPacked == false && XmlHelper.ReadAttributeBool(node, "packed", false) == true)
					{
						throw new AgateResourceException("Sprite resource " + Name + " has the packed=true attribute," +
							" but some frames are located in separate files.");
					}

					break;

				case "0.3.0":
					Name = node.Attributes["name"].Value;
					mFilename = XmlHelper.ReadAttributeString(node, "image", string.Empty);
					mPacked = XmlHelper.ReadAttributeBool(node, "packed", true);

					if (node.Attributes["size"] != null)
					{
						mSize = XmlHelper.ReadAttributeSize(node, "size");
						mHasSize = true;
					}
					else
						mHasSize = false;

					ReadFrames030(node);

					// check and make sure the sprite can be packed, and this matches the packed attribute 
					if (mPacked == false && XmlHelper.ReadAttributeBool(node, "packed", false) == true)
					{
						throw new AgateResourceException("Sprite resource " + Name + " has the packed=true attribute," +
							" but some frames are located in separate files.");
					}

					break;
			}
		}

		private void ReadFrames031(XmlNode node)
		{
			foreach (XmlNode child in node.ChildNodes)
			{
				if (child.Name == "Image")
					ReadImage031(child);
				else if (child.Name == "Frame")
					ReadFrame030(child);
				else
					System.Diagnostics.Trace.WriteLine(
						"Unrecognized node in Sprite " + Name + ": " + child.Name);
			}
		}

		private void ReadFrames030(XmlNode node)
		{
			foreach (XmlNode child in node.ChildNodes)
			{
				if (child.Name == "Frame")
					ReadFrame030(child);
				else
					System.Diagnostics.Trace.WriteLine(
						"Unrecognized node in Sprite " + Name + ": " + child.Name);
			}
		}

		private void ReadImage031(XmlNode node)
		{
			SpriteImageResource image = new SpriteImageResource();

			image.Filename = XmlHelper.ReadAttributeString(node, "file");

			for (int i = 0; i < node.ChildNodes.Count; i++)
			{
				if (node.ChildNodes[i].Name == "Grid")
				{
					var g = new SpriteImageResource.Grid();

					g.Location = XmlHelper.ReadAttributePoint(node.ChildNodes[i], "loc");
					g.Size = XmlHelper.ReadAttributeSize(node.ChildNodes[i], "size");
					g.Array = XmlHelper.ReadAttributeSize(node.ChildNodes[i], "array");

					image.Grids.Add(g);
				}
			}

			mImages.Add(image);
		}
		private void ReadFrame030(XmlNode node)
		{
			SpriteFrameResource frame = new SpriteFrameResource();

			frame.Bounds = XmlHelper.ReadAttributeRectangle(node, "rect");
			frame.Offset = XmlHelper.ReadAttributePoint(node, "offset");
			frame.Filename = XmlHelper.ReadAttributeString(node, "image", string.Empty);

			if (string.IsNullOrEmpty(frame.Filename))
			{
				frame.Filename = mFilename;

				if (mFilename == null)
					throw new AgateResourceException("Sprite resource " + Name + " does not have a " +
						"default filename, and frame " + mImages.Count.ToString() +
						" does not specify a filename.");
			}

			if (frame.Filename != mFilename)
			{
				mPacked = false;
			}

			mImages.Add(frame);
		}

		internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
		{
			XmlElement element = doc.CreateElement("Sprite");

			XmlHelper.AppendAttribute(element, doc, "name", Name);
			XmlHelper.AppendAttribute(element, doc, "image", Filename);
			XmlHelper.AppendAttribute(element, doc, "timePerFrame", TimePerFrame);
			XmlHelper.AppendAttribute(element, doc, "size", Size.ToString());

			for (int i = 0; i < ChildElements.Count; i++)
			{
				BuildNodes(element, doc, ChildElements[i]);
			}

			parent.AppendChild(element);
		}
		internal void BuildNodes(XmlElement parent, XmlDocument doc, SpriteSubResource sub)
		{
			if (sub is SpriteImageResource)
				BuildNodes(parent, doc, (SpriteImageResource)sub);
			else if (sub is SpriteFrameResource)
				BuildNodes(parent, doc, (SpriteFrameResource)sub);
			else
				throw new NotImplementedException();
		}
		internal void BuildNodes(XmlElement parent, XmlDocument doc, SpriteImageResource image)
		{
			XmlElement element = doc.CreateElement("Image");

			XmlHelper.AppendAttribute(element, doc, "file", image.Filename);

			for (int i = 0; i < image.Grids.Count; i++)
			{
				XmlElement grid = doc.CreateElement("Grid");

				XmlHelper.AppendAttribute(grid, doc, "loc", image.Grids[i].Location);
				XmlHelper.AppendAttribute(grid, doc, "size", image.Grids[i].Size);
				XmlHelper.AppendAttribute(grid, doc, "array", image.Grids[i].Array);
			}
		}
		internal void BuildNodes(XmlElement parent, XmlDocument doc, SpriteFrameResource frame)
		{
			XmlElement element = doc.CreateElement("Frame");

			XmlHelper.AppendAttribute(element, doc, "rect", frame.Bounds.ToString());
			XmlHelper.AppendAttribute(element, doc, "offset", frame.Offset.ToString());

			if (Packed == false && frame.Filename != Filename)
				XmlHelper.AppendAttribute(element, doc, "image", frame.Filename);

			parent.AppendChild(element);
		}

		/// <summary>
		/// Clones the resource.
		/// </summary>
		/// <returns></returns>
		protected override AgateResource Clone()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		public class SpriteSubResource
		{
			/// <summary>
			/// Filename for this part of the sprite.
			/// </summary>
			public string Filename { get; set; }
		}
		/// <summary>
		/// Class representing a frame of a sprite in a SpriteResource.
		/// </summary>
		public class SpriteFrameResource : SpriteSubResource 
		{
			/// <summary>
			/// Rectangle where the image data is.
			/// </summary>
			public Rectangle Bounds { get; set; }
			/// <summary>
			/// Offset in the sprite to where the upper left of the image is drawn.
			/// </summary>
			public Point Offset { get; set; }
			/// <summary>
			/// Converts the sprite resource to a string for debugging info.
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				StringBuilder b = new StringBuilder();

				if (string.IsNullOrEmpty(Filename) == false)
				{
					b.Append("Filename: ");
					b.Append(Filename);
					b.Append("   ");
				}

				b.Append("Bounds: ");
				b.Append(Bounds);
				b.Append("   ");

				b.Append("Offset: ");
				b.Append(Offset);
				b.Append("   ");

				return b.ToString();
			}
		}
		/// <summary>
		/// Class representing an image to automatically load frames from.
		/// </summary>
		public class SpriteImageResource : SpriteSubResource 
		{
			/// <summary>
			/// 
			/// </summary>
			public class Grid
			{
				/// <summary>
				/// 
				/// </summary>
				public Point Location { get; set; }
				/// <summary>
				/// 
				/// </summary>
				public Size Size { get; set; }
				/// <summary>
				/// 
				/// </summary>
				public Size Array { get; set; }
			}

			List<Grid> mGrids = new List<Grid>();

			/// <summary>
			/// 
			/// </summary>
			public IList<Grid> Grids
			{
				get { return mGrids; }
			}
		}
	}

}
