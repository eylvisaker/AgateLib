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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface.Styling.Themes
{
    public interface ITheme
    {
        /// <summary>
        /// Read-only please.
        /// </summary>
        ThemeModel Model { get; }

        IFontProvider Fonts { get; set; }

        void Apply(IRenderElement widget, RenderElementStack parentStack);

        T LoadContent<T>(ThemePathTypes themePathTypes, string file);
    }

    [Obsolete]
    public interface IWidgetStackState
    {
        IRenderElement Parent { get; }
    }

    /// <summary>
    /// Represents a theme.
    /// </summary>
    public class Theme : ITheme
    {
        #region --- Static Members ---

        /// <summary>
        /// Loads a theme. If you're loading
        /// multiple themes, it is more efficient
        /// to create a ThemeLoader object and reuse it.
        /// </summary>
        /// <param name="fonts"></param>
        /// <param name="content"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete("Use a ThemeLoader object instead.")]
        public static ITheme Load(IFontProvider fonts, IContentProvider content, string mainThemeFile, params string[] additionalStyleFilenames)
        {
            if (fonts is FontProvider f)
            {
                ThemeLoader themeLoader = new ThemeLoader(f, content);

                return themeLoader.LoadTheme(mainThemeFile, additionalStyleFilenames);
            }

            throw new NotSupportedException("Must use a ThemeLoader object instead.");
        }

        /// <summary>
        /// Creates the default AgateLib theme which requires no assets.
        /// </summary>
        /// <returns></returns>
        public static Theme CreateDefaultTheme(IContentProvider content)
        {
            var stylePatterns = new ThemeStylePatternList
            {
                new ThemeStyle
                {
                    Selector = "workspace",
                    Background = new BackgroundStyle
                    {
                        Color = new Color(Color.Black, 0.2f)
                    },
                },

                new ThemeStyle
                {
                    Selector = "window, workspace > *",
                    Background = new BackgroundStyle { Color = Color.Black, },
                    Font = new FontStyleProperties { Color = Color.White, },
                    Margin = LayoutBox.SameAllAround(10),
                    Animation = new AnimationStyle
                    {
                        Entry = "fade 0.35",
                        Exit = "fade 0.35"
                    }
                },

                new ThemeStyle
                {
                    Selector = "label",
                    Padding = new LayoutBox(12, 8, 12, 8)
                },

                new ThemeStyle
                {
                    Selector = "textbox",
                    Border = BorderStyle.Solid(Color.White, 1),
                    Padding = new LayoutBox(4, 2, 4, 2),
                },

                new ThemeStyle
                {
                    Selector = "button",
                },

                new ThemeStyle
                {
                    Selector = "button:disabled",
                    Font = new FontStyleProperties { Color = Color.Gray }
                },

                new ThemeStyle
                {
                    Selector = "button:focus",
                    Background = new BackgroundStyle { Color = Color.White },
                    Font = new FontStyleProperties { Color = Color.Black },
                },

                new ThemeStyle
                {
                    Selector = "valuespinner:focus",
                    Background = new BackgroundStyle { Color = Color.White },
                    Font = new FontStyleProperties { Color = Color.Black },
                },

                new ThemeStyle
                {
                    Selector = "radiobutton:disabled",
                    Font = new FontStyleProperties { Color = Color.Gray }
                },

                new ThemeStyle
                {
                    Selector = "radiobutton:focus",
                    Background = new BackgroundStyle { Color = Color.White },
                    Font = new FontStyleProperties { Color = Color.Black },
                },

                new ThemeStyle
                {
                    Selector = "radiobutton:checked",
                    Background = new BackgroundStyle { Color = Color.DarkCyan },
                    Font = new FontStyleProperties { Color = Color.Black },
                },

                new ThemeStyle
                {
                    Selector = "radiobutton:checked:disabled",
                    Background = new BackgroundStyle { Color = Color.DarkCyan },
                    Font = new FontStyleProperties { Color = Color.Gray },
                },

                new ThemeStyle
                {
                    Selector = "radiobutton:checked:focus",
                    Background = new BackgroundStyle { Color = Color.Yellow },
                    Font = new FontStyleProperties { Color = Color.Black },
                }
            };

            return new Theme(content, new ThemeModel { Styles = stylePatterns });

        }

        #endregion
        #region --- privates. Don't touch my privates! ---

        private ThemeModel model;
        private List<ThemeStyle> themeMatches = new List<ThemeStyle>();
        private string rootFolder;
        private readonly IContentProvider content;

        #endregion

        /// <summary>
        /// Initializes a Theme object with an empty ThemeData value.
        /// </summary>
        public Theme(IContentProvider content) : this(content, new ThemeModel())
        {
        }

        /// <summary>
        /// Initializes a Theme object.
        /// </summary>
        /// <param name="model"></param>
        public Theme(IContentProvider content, ThemeModel model)
        {
            AgateLib.Quality.Require.ArgumentNotNull(content, nameof(content));

            this.content = content;
            this.Model = model;
        }

        public event EventHandler PathsChanged;

        /// <summary>
        /// Gets or sets the data for the theme. This cannot be null.
        /// </summary>
        public ThemeModel Model
        {
            get => model;
            set
            {
                model = value ?? throw new ArgumentNullException(nameof(Model));
                InitializeSelectors();
            }
        }

        public IFontProvider Fonts { get; set; }

        /// <summary>
        /// The root folder from which to load assets for the theme.
        /// </summary>
        public string RootFolder
        {
            get => rootFolder;
            set
            {
                if (rootFolder == value)
                    return;

                rootFolder = value;

                PathsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Apply(IRenderElement element, RenderElementStack stack)
        {
            element.Display.Theme = this;
            
            element.Display.ElementStyles.Clear();
            
            element.Display.ElementStyles.AddRange(
                model.Styles.SelectMany(x => x.MatchElementStyle(element, stack)));
        }

        private void InitializeSelectors()
        {
            foreach (var item in model.Styles)
            {
                item.MatchExecutor = new CssWidgetSelector(item.Selector);
            }
        }

        public T LoadContent<T>(ThemePathTypes themePathTypes, string file)
        {
            string root
                = (themePathTypes.HasFlag(ThemePathTypes.Cursors) ? model.Paths.Cursors : null)
                ?? (themePathTypes.HasFlag(ThemePathTypes.Images) ? model.Paths.Images : null);

            root = System.IO.Path.Combine(RootFolder, root);

            return content.Load<T>(System.IO.Path.Combine(root, file));
        }
    }
}
