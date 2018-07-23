using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Parsers.Tmx
{
    public class TmxTileData
    {
        public PropertyBag Properties { get; set; } = new PropertyBag();

        public List<TmxObjectData> Collision { get; set; } = new List<TmxObjectData>();
    }
}
