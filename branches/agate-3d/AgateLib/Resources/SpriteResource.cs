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

        List<SpriteFrameResource> mFrames = new List<SpriteFrameResource>();

        /// <summary>
        /// Gets or sets whether or not this sprite uses the PackedSprite class.
        /// </summary>
        public bool Packed
        {
            get { return mPacked; }
			set
			{
				for (int i = 0; i < Frames.Count; i++)
				{
					if (Frames[i].Filename != Filename)
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
        public List<SpriteFrameResource> Frames
        {
            get { return mFrames; }
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
                case "0.3.0":
                    Name = node.Attributes["name"].Value;
                    mSize = XmlHelper.ReadAttributeSize(node, "size");
                    mFilename = XmlHelper.ReadAttributeString(node, "image", string.Empty);
                    mPacked = XmlHelper.ReadAttributeBool(node, "packed", true);

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

        private void ReadFrames030(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Frame")
                    ReadFrame030(child);
                else
                    throw new AgateResourceException("Unrecognized node in Sprite " + Name + ": " + child.Name);
            }
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
                        "default filename, and frame " + mFrames.Count.ToString() + 
                        " does not specify a filename.");
            }

            if (frame.Filename != mFilename)
            {
                mPacked = false;
            }

            mFrames.Add(frame);
        }

        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
			XmlElement element = doc.CreateElement("Sprite");

			XmlHelper.AppendAttribute(element, doc, "name", Name);
			XmlHelper.AppendAttribute(element, doc, "image", Filename);
			XmlHelper.AppendAttribute(element, doc, "timePerFrame", TimePerFrame);
			XmlHelper.AppendAttribute(element, doc, "size", Size.ToString());

			for (int i = 0; i < Frames.Count; i++)
			{
				BuildNodes(element, doc, Frames[i]);
			}

			parent.AppendChild(element);
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
        /// Class representing a frame of a sprite in a SpriteResource.
        /// </summary>
        public class SpriteFrameResource
        {
            string mFilename;
            Rectangle mBounds;
            Point mOffset;

            /// <summary>
            /// Filename the frame is located in.
            /// </summary>
            public string Filename
            {
                get { return mFilename; }
                set { mFilename = value; }
            }
            /// <summary>
            /// Rectangle where the image data is.
            /// </summary>
            public Rectangle Bounds
            {
                get { return mBounds; }
                set { mBounds = value; }
            }
            /// <summary>
            /// Offset in the sprite to where the upper left of the image is drawn.
            /// </summary>
            public Point Offset
            {
                get { return mOffset; }
                set { mOffset = value; }
            }

            public override string ToString()
            {
                StringBuilder b = new StringBuilder();

                if (string.IsNullOrEmpty(mFilename) == false)
                {
                    b.Append("Filename: ");
                    b.Append(mFilename);
                    b.Append("   ");
                }

                b.Append("Bounds: ");
                b.Append(mBounds);
                b.Append("   ");

                b.Append("Offset: ");
                b.Append(mOffset);
                b.Append("   ");

                return b.ToString();
            }
		}

    }

}
