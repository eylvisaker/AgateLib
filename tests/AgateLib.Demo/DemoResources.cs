using AgateLib.Demo.Selector;
using AgateLib.Display;
using AgateLib.Themes.Papyrus.Content;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AgateLib.Demo
{
    public interface IDemoResources
    {
        GraphicsDevice GraphicsDevice { get; }

        IContentProvider Content { get; }

        IFontProvider Fonts { get; }

        IServiceProvider ServiceProvider { get; }

        IContentLayoutEngine LocalizedContent { get; }

        IStyleConfigurator StyleConfigurator { get; }

        /// <summary>
        /// A purely white texture.
        /// </summary>
        Texture2D WhiteTexture { get; }

        UserInterfaceRenderer UserInterfaceRenderer { get; set; }

        Rectangle ScreenArea { get; }

        UserInterfaceScene CreateUserInterfaceScene();
    }

    public class DemoResources : IDemoResources
    {
        public DemoResources(GraphicsDevice graphics, ContentManager content, GameServiceContainer Services)
        {
            GraphicsDevice = graphics;
            ScreenArea = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            Content = new ContentProvider(content);
            ServiceProvider = Services;
            Fonts = LoadFonts();
            ContentLayoutEngine = new ContentLayoutEngine(Fonts);
            LocalizedContent = new LocalizedContentLayoutEngine(
                ContentLayoutEngine, new FakeTextRepository());

            ThemeLoader = new ThemeLoader(Fonts, Content);

            Themes = new ThemeCollection
            {
                ["default"] = Theme.CreateDefaultTheme(Content),
                ["FF"] = ThemeLoader.LoadTheme("UserInterface/FF")
            };

            PapyrusTheme.Register(Themes, Fonts, Content, ThemeLoader);

            Themes.DefaultThemeKey = "Papyrus";

            StyleConfigurator = new ThemeStyler(Themes);

            UserInterfaceRenderer = new UserInterfaceRenderer(
                new ComponentStyleRenderer(graphics, Content, Themes.DefaultTheme),
                ScreenArea);

            WhiteTexture = new TextureBuilder(graphics).SolidColor(10, 10, Color.White);
        }

        private FontProvider LoadFonts()
        {
            var fonts = new FontProvider
            {
                { "AgateSans", Font.Load(Content, "AgateLib/AgateSans") },
                { "AgateMono", Font.Load(Content, "AgateLib/AgateMono") }
            };

            return fonts;
        }

        public ResoucesConfiguration Config { get; set; } = new ResoucesConfiguration();

        public UserInterfaceScene CreateUserInterfaceScene()
        {
            var result = new UserInterfaceScene(
                   ScreenArea,
                   GraphicsDevice,
                   UserInterfaceRenderer,
                   LocalizedContent,
                   Fonts,
                   StyleConfigurator);

            result.Theme = Config.DefaultTheme;

            return result;
        }

        public GraphicsDevice GraphicsDevice { get; set; }

        public Rectangle ScreenArea { get; set; }

        public IContentProvider Content { get; set; }

        public FontProvider Fonts { get; set; }
        IFontProvider IDemoResources.Fonts => Fonts;

        public IServiceProvider ServiceProvider { get; set; }

        public Texture2D WhiteTexture { get; set; }
        public ContentLayoutEngine ContentLayoutEngine { get; set; }

        public IContentLayoutEngine LocalizedContent { get; set; }
        public ThemeCollection Themes { get; set; }

        public UserInterfaceRenderer UserInterfaceRenderer { get; set; }
        public IStyleConfigurator StyleConfigurator { get; set; }
        public ThemeLoader ThemeLoader { get; internal set; }
    }

    public class ResoucesConfiguration
    {
        public string DefaultTheme { get; set; } = "Papyrus";
    }
}
