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
using AgateLib.UserInterface.Styling.Themes.Model;
using AgateLib.UserInterface.Styling.Themes.Model.TypeConverters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.UserInterface.Styling.Themes
{
    public interface IThemeLoader
    {
        ThemeData LoadThemeData(IContentProvider content, string filename);

        Theme LoadTheme(IContentProvider content, string filename);
    }

    [Singleton]
    public class ThemeLoader : IThemeLoader
    {
        /// <summary>
        /// Extension for theme files.
        /// </summary>
        public const string Extension = ".atheme";

        private readonly Deserializer deserializer;
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

        public ThemeData LoadThemeData(IContentProvider content, string filename)
        {
            string filecontent = null;

            if (!filename.EndsWith(".atheme"))
            {
                filecontent = filecontent ?? content.ReadAllTextOrNull(filename + Extension);
            }

            filecontent = filecontent ?? content.ReadAllTextOrNull(filename);

            if (filecontent == null)
                throw new FileNotFoundException(filename);

            var result = deserializer.Deserialize<ThemeData>(filecontent);

            NormalizePaths(result, filename);

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

        public Theme LoadTheme(IContentProvider content, string filename)
        {
            try
            {
                return new Theme
                {
                    Data = LoadThemeData(content, filename),
                    Fonts = fonts
                };
            }
            catch (Exception e)
            {
                throw new UserInterfaceLoadException($"Loading '{filename}' failed with {e.GetType().Name}",
                    e);
            }
        }
    }
}
