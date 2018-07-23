using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Parsers.Tmx
{
    public class TmxObjectData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public MapObjectDataType DataType { get; set; }

        public Rectangle Bounds => (Rectangle)Polygon.BoundingRect;

        public Polygon Polygon { get; set; }

        public PropertyBag Properties { get; set; } = new PropertyBag();
    }

    public enum MapObjectDataType
    {
        Rectangle,
        Polygon,
        Polyline,
    }
}
