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

using AgateLib.UserInterface.Styling.Themes.Model;
using AgateLib.UserInterface.Styling.Themes.Model.TypeConverters;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.UserInterface.Styling.Themes
{
    /// <summary>
    /// Interface for an object which loads themes from a content provider.
    /// </summary>
    public interface IThemeLoader
    {
        ThemeData LoadThemeData(IContentProvider content, params string[] filenames);

        Theme LoadTheme(IContentProvider content, params string[] filenames);
    }

    /// <summary>
    /// Object which loads theams from a content provider.
    /// </summary>
    [Singleton]
    public class ThemeLoader : IThemeLoader
    {
        /// <summary>
        /// Extension for theme files.
        /// </summary>
        public const string Extension = ".atheme";

        private readonly IDeserializer deserializer;
        private readonly IFontProvider fonts;

        public ThemeLoader(IFontProvider fonts)
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .WithTypeConvertersForBasicStructures()
                .WithTypeConverter(new ImageSourceYamlConverter())
                .Build();
            this.fonts = fonts;
        }

        public ThemeData LoadThemeData(IContentProvider content, params string[] filenames)
        {
            if (filenames.Length == 0)
                throw new ArgumentException("Must pass at least one filename to load.");

            ThemeData result = new ThemeData();

            foreach (string filename in filenames)
            {
                string filecontent = null;

                if (!filename.EndsWith(".atheme"))
                {
                    filecontent = filecontent ?? content.ReadAllTextOrNull(filename + Extension);
                }

                filecontent = filecontent ?? content.ReadAllTextOrNull(filename);

                if (filecontent == null)
                    throw new FileNotFoundException(filename);

                var items = deserializer.Deserialize<ThemeData>(filecontent);

                NormalizePaths(items, filename);

                foreach (var item in items)
                    result.Add(item);
            }

            return result;
        }

        private void NormalizePaths(ThemeData result, string filename)
        {
            string path = Path.GetDirectoryName(filename + Extension);

            if (string.IsNullOrWhiteSpace(path))
                return;

            foreach (var style in result)
            {
                AddPath(style.Background, path);
                AddPath(style.Border, path);
            }
        }

        private void AddPath(BorderStyle border, string path)
        {
            if (border?.Image == null)
                return;

            border.Image.File = CombinePath(path, border.Image.File);
        }

        private void AddPath(BackgroundStyle background, string path)
        {
            if (background?.Image == null)
                return;

            background.Image.File = CombinePath(path, background.Image.File);
        }

        private string CombinePath(string a, string b)
        {
            if (string.IsNullOrWhiteSpace(b))
                return null;

            if (string.IsNullOrWhiteSpace(a))
                return b;

            return a + "/" + b;
        }

        public Theme LoadTheme(IContentProvider content, params string[] filenames)
        {
            try
            {
                return new Theme
                {
                    Data = LoadThemeData(content, filenames),
                    Fonts = fonts
                };
            }
            catch (Exception e)
            {
                throw new UserInterfaceLoadException($"Loading '{string.Join(",", filenames)}' failed with {e.GetType().Name}",
                    e);
            }
        }
    }
}
