using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace AgateLib.Display.BitmapFont.Model
{
    public class FontModel
    {
        public class FontAbout
        {
            /// <summary>
            /// A description of the font, who licensed it and how it was licensed.
            /// </summary>
            [YamlMember(Alias = "desc", ApplyNamingConventions = true)]
            public string Description { get; set; }

            /// <summary>
            /// The URL of the font's origin.
            /// </summary>
            public string Url { get; set; }
        }

        public string ImagePath { get; set; }

        /// <summary>
        /// Provides an informational description of the font.
        /// </summary>
        public FontAbout About { get; set; }

        public List<FontVariationData> Variations { get; set; } = new List<FontVariationData>();
    }

}
