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

using AgateLib.Quality;
using AgateLib.UserInterface.Styling.Themes.Model;
using AgateLib.UserInterface.Styling.Themes.Model.TypeConverters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.UserInterface.Styling.Themes
{
    /// <summary>
    /// Interface for an object which loads themes from a content provider.
    /// </summary>
    public interface IThemeLoader
    {
        ThemeModel LoadThemeData(string filename);

        Theme LoadTheme(string mainThemeFilename, params string[] addiationalStyleFilenames);
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
        private readonly FontProvider fonts;
        private readonly IContentProvider content;

        public ThemeLoader(FontProvider fonts, IContentProvider content)
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .WithTypeConvertersForBasicStructures()
                .WithTypeConverter(new ImageSourceYamlConverter())
                .Build();
            this.fonts = fonts;
            this.content = content;
        }

        public ThemeModel LoadThemeData(string filename)
        {
            Require.ArgumentNotNull(filename, nameof(filename));

            ThemeModel result;

            string filecontent = null;

            if (!filename.EndsWith(".atheme"))
            {
                filecontent = filecontent ?? content.ReadAllTextOrNull(filename + Extension);
            }

            filecontent = filecontent ?? content.ReadAllTextOrNull(filename);

            if (filecontent == null)
            {
                throw new FileNotFoundException(filename);
            }

            result = deserializer.Deserialize<ThemeModel>(filecontent);

            // TODO: Load imports

            //NormalizePaths(items, filename);

            //foreach (var item in items)
            //{
            //    result.Add(item);
            //}

            return result;
        }

        [Obsolete("Use LoadAdditionalStyles instead.")]
        public ThemeStylePatternList Old_LoadThemeData(params string[] filenames)
            => LoadAdditionalStyles(filenames);

        public ThemeStylePatternList LoadAdditionalStyles(params string[] filenames)
        {
            if (filenames.Length == 0)
                return new ThemeStylePatternList();

            ThemeStylePatternList result = new ThemeStylePatternList();

            foreach (string filename in filenames)
            {
                string filecontent = null;

                if (!filename.EndsWith(".atheme"))
                {
                    filecontent = filecontent ?? content.ReadAllTextOrNull(filename + Extension);
                }

                filecontent = filecontent ?? content.ReadAllTextOrNull(filename);

                if (filecontent == null)
                {
                    throw new FileNotFoundException(filename);
                }

                var items = deserializer.Deserialize<ThemeStylePatternList>(filecontent);

                NormalizePaths(items, filename);

                foreach (var item in items)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private void NormalizePaths(ThemeStylePatternList result, string filename)
        {
            string path = Path.GetDirectoryName(filename + Extension);

            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            foreach (var style in result)
            {
                AddPath(style.Background, path);
                AddPath(style.Border, path);
            }
        }

        private void AddPath(BorderStyle border, string path)
        {
            if (border?.Image == null)
            {
                return;
            }

            border.Image.File = CombinePath(path, border.Image.File);
        }

        private void AddPath(BackgroundStyle background, string path)
        {
            if (background?.Image == null)
            {
                return;
            }

            background.Image.File = CombinePath(path, background.Image.File);
        }

        private string CombinePath(string a, string b)
        {
            if (string.IsNullOrWhiteSpace(b))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(a))
            {
                return b;
            }

            return a + "/" + b;
        }

        public Theme LoadTheme(string mainThemeFile, params string[] additionalStyles)
        {
            // Try to load the old format themes first.
            // If there is any exception, ignore it and try to load the new format.
            // This way we only report exceptions for the new format.
            try
            {
                Theme oldThemeType = new Theme(content)
                {
                    Model = new ThemeModel
                    {
                        Styles = LoadAdditionalStyles(mainThemeFile)
                    },
                    Fonts = fonts
                };

                oldThemeType.Model.Styles.AddRange(LoadAdditionalStyles(additionalStyles));

                return oldThemeType;
            }
            catch
            {
                // Ignore exceptions when trying to load old format data.
            }

            ThemeModel themeData;

            try
            {
                themeData = LoadThemeData(mainThemeFile);
            }
            catch (Exception e)
            {
                throw new UserInterfaceLoadException(
                    $"Loading {mainThemeFile} failed with {e.GetType().Name}",
                    e);
            }

            Theme result = new Theme(content, themeData)
            {
                Fonts = fonts,
                RootFolder = System.IO.Path.GetDirectoryName(mainThemeFile)
            };

            try
            {
                result.Model.Styles.AddRange(LoadAdditionalStyles(additionalStyles));

                return result;
            }
            catch (Exception e)
            {
                throw new UserInterfaceLoadException(
                    $"Loading additional styles for {mainThemeFile} failed with {e.GetType().Name}",
                    e);
            }
        }
    }
}
