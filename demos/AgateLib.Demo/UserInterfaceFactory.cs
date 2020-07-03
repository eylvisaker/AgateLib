using AgateLib.Display;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Demo
{
    public class UserInterfaceFactory
    {
        public UserInterfaceFactory(GraphicsDevice graphics, IContentProvider content)
        {
            this.Graphics = graphics;
            this.Content = content;

            Fonts = new FontProvider
            {
                { "AgateSans", Font.Load(Content, "AgateLib/AgateSans") },
                { "AgateMono", Font.Load(Content, "AgateLib/AgateMono") }
            };

            ContentLayoutEngine = new ContentLayoutEngine(Fonts);

            ThemeLoader = new ThemeLoader(Fonts, Content);

            Themes = new ThemeCollection(Config)
            {
                ["default"] = Theme.CreateDefaultTheme(Content),
                ["FF"] = ThemeLoader.LoadTheme("UserInterface/FF")
            };

            StyleConfigurator = new ThemeStyler(Themes);

            Renderer = new UserInterfaceRenderer(
                new ComponentStyleRenderer(graphics),
                Config);

            SetScreenAreaByBackBuffer();
        }

        public UserInterfaceConfig Config { get; set; } = new UserInterfaceConfig();

        public GraphicsDevice Graphics { get; }

        public IContentProvider Content { get; }

        public FontProvider Fonts { get; }

        public ContentLayoutEngine ContentLayoutEngine { get; }
        public ThemeLoader ThemeLoader { get; }
        public ThemeCollection Themes { get; }
        public ThemeStyler StyleConfigurator { get; }
        public UserInterfaceRenderer Renderer { get; }
        public IContentLayoutEngine LocalizedContent { get; set; }

        public string DefaultThemeKey
        {
            get => Config.DefaultTheme;
            set => Config.DefaultTheme = value;
        }

        /// <summary>
        /// Creates a user interface scene.
        /// </summary>
        /// <param name="config">An optional config object which will override the 
        /// settings for this scene.</param>
        /// <returns></returns>
        public UserInterfaceScene CreateUserInterfaceScene(
            UserInterfaceConfig config = null)
        {
            var result = new UserInterfaceScene(
                   config ?? Config,
                   Graphics,
                   Renderer,
                   LocalizedContent ?? ContentLayoutEngine,
                   Fonts,
                   StyleConfigurator);

            result.Theme = DefaultThemeKey;

            return result;
        }


        public void SetScreenAreaByBackBuffer()
        {
            Config.ScreenArea = new Rectangle(0, 0,
                    Graphics.PresentationParameters.BackBufferWidth,
                    Graphics.PresentationParameters.BackBufferHeight);

        }
    }
}
