using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// Resource which loads a sprite.  There are two different types of sprites, a general purpose
    /// sprite, implemented as the Sprite class, and a packed version which maximizes the memory
    /// efficiency, implemented as PackedSprite.<br/>
    /// XML Attributes:<br/> 
    ///   string name, bool packed, Size size, string image
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
        Size size;
        string filename = string.Empty;
        bool packed = false;
        List<SpriteFrameResource> mFrames = new List<SpriteFrameResource>();

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
                    size = XmlHelper.ReadAttributeSize(node, "size");
                    filename = XmlHelper.ReadAttributeString(node, "image", string.Empty);
                    packed = XmlHelper.ReadAttributeBool(node, "packed", true);

                    ReadFrames030(node);

                    if (packed == false && XmlHelper.ReadAttributeBool(node, "packed", false) == true)
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
                {
                    ReadFrame030(child);
                }
                else
                    throw new AgateResourceException("Unrecognized node in Sprite " + Name + ": " + child.Name);
            }
        }
        private void ReadFrame030(XmlNode node)
        {
            SpriteFrameResource frame = new SpriteFrameResource();

            frame.Bounds = XmlHelper.ReadAttributeRectangle(node, "rect");
            frame.Offset = XmlHelper.ReadAttributePoint(node, "offset");
            frame.Filename = XmlHelper.ReadAttributeString(node, string.Empty);

            if (string.IsNullOrEmpty(frame.Filename))
            {
                frame.Filename = filename;

                if (filename == null)
                    throw new AgateResourceException("Sprite resource " + Name + " does not have a default filename, and " +
                        "frame " + mFrames.Count.ToString() + " does not specify a filename.");
            }

            if (frame.Filename != filename)
            {
                packed = false;
            }

            mFrames.Add(frame);
        }

        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        protected override AgateResource Clone()
        {
            throw new NotImplementedException();
        }
        

        public class SpriteFrameResource
        {
            string mFilename;
            Rectangle mBounds;
            Point mOffset;

            public string Filename
            {
                get { return mFilename; }
                set { mFilename = value; }
            }
            public Rectangle Bounds
            {
                get { return mBounds; }
                set { mBounds = value; }
            }
            public Point Offset
            {
                get { return mOffset; }
                set { mOffset = value; }
            }
        }
    }

}
