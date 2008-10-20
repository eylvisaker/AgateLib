using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Resources
{
    /// <summary>
    /// Resource which loads a sprite.  There are two different types of sprites, a general purpose
    /// sprite, implemented as the Sprite class, and a packed version which maximizes the memory
    /// efficiency, implemented as PackedSprite.<br/>
    /// XML Attributes:<br/> 
    ///   string name, bool packed, Size size
    /// XML Nodes:<br/>
    ///   Image:  string filename 
    ///     Frame:
    ///       Rectangle bounds, Point offset
    /// </summary>
    public class SpriteResource : AgateResource 
    {
        public SpriteResource(string name)
            : base(name)
        {
            
        }

        internal override void BuildNodes(System.Xml.XmlElement parent, System.Xml.XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        protected override AgateResource Clone()
        {
            throw new NotImplementedException();
        }
    }
}
