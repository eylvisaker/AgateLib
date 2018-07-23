using AgateLib.Mathematics.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Parsers.Tmx
{
    public class TmxTileSetData
    {
        public string Name { get; set; }

        public int FirstTileId { get; set; }

        public int TileCount { get; set; }

        /// <summary>
        /// Columns of tiles in the image.
        /// </summary>
        public int Columns { get; set; }

        public Size TileSize { get; set; }

        public string ImageSource { get; set; }

        public Size ImageSize { get; set; }

        public Dictionary<int, TmxTileData> Tiles { get; set; } = new Dictionary<int, TmxTileData>();

        public PropertyBag Properties { get; set; } = new PropertyBag();

    }
}
