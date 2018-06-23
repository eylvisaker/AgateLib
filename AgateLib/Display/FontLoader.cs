//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AgateLib.Display.BitmapFont;
using AgateLib.Display.BitmapFont.Model;
using AgateLib.Display.BitmapFont.TypeConverters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.Display
{
    public static class FontLoader
    {
        static Deserializer deserializer;

        private static Deserializer CreateDeserializer()
        {
            return new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .WithTypeConvertersForBasicStructures()
                .WithTypeConverter(new KerningPairModelYaml())
                .Build();
        }

        public static FontResource ReadFont(IContentProvider content, string name)
        {
            if (deserializer == null)
                deserializer = CreateDeserializer();

            string filename = name;

            if (!filename.EndsWith(".afont"))
            {
                filename += ".afont";
            }

            using (var file = new StreamReader(content.Open(filename)))
            {
                var result = deserializer.Deserialize<FontResource>(file);
                result.ImagePath = Path.GetDirectoryName(filename);

                return result;
            }
        }

        /// <summary>
        /// Loads a font from the ContentManager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Font Load(IContentProvider content, string name)
        {
            var fontModel = ReadFont(content, name);

            var path = fontModel.ImagePath;

            Font result = new Font(name);

            foreach (var fontSurfaceModel in fontModel)
            {
                var image = fontSurfaceModel.Image;

                if (image.EndsWith(".png"))
                {
                    image = image.Substring(0, image.Length - 4);
                }

                var surface = content.Load<Texture2D>(Path.Combine(path, image));

                FontMetrics metrics = new FontMetrics();
                foreach (var glyph in fontSurfaceModel.Metrics)
                {
                    metrics.Add((char)glyph.Key, glyph.Value);
                }

                var fontSurface = new BitmapFontTexture(surface, metrics, name);

                result.Core.AddFontSurface(new FontSettings(fontSurfaceModel.Size, fontSurfaceModel.Style), fontSurface);
            }

            return result;
        }
    }
}
