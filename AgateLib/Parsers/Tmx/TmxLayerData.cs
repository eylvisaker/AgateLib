using AgateLib.Mathematics.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Parsers.Tmx
{
    public class TmxLayerData
    {
        public string Name { get; set; }

        public PropertyBag Properties { get; set; } = new PropertyBag();

        /// <summary>
        /// Size of the map layer in tiles.
        /// </summary>
        public Size Size { get; set; }

        public bool HasTiles => TileData != null;
        public bool HasObjects => Objects != null;

        public int[] TileData { get; set; }

        /// <summary>
        /// Gets the tile at the specified location.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int TileAt(int x, int y) => TileData[x + y * Size.Width];

        public List<TmxObjectData> Objects { get; set; } = new List<TmxObjectData>();

        public override string ToString()
        {
            return $"MapLayerData: {Name}";
        }
    }
}
