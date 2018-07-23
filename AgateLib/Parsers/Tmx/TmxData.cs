using AgateLib.Mathematics.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Parsers.Tmx
{
    public class TmxData
    {
        /// <summary>
        /// The directory the TMX file was loaded from. Any filenames referenced therein
        /// will be relative to this directory.
        /// </summary>
        public string Subdirectory { get; set; }

        public Size Size { get; set; }

        public Size TileSize { get; set; }

        public List<TmxTileSetData> TileSets { get; set; } = new List<TmxTileSetData>();

        public List<TmxLayerData> Layers { get; set; } = new List<TmxLayerData>();

        public PropertyBag Properties { get; set; } = new PropertyBag();
    }
}
