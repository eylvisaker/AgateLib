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

        UserInterfaceFactory UserInterfaceFactory { get; }

        /// <summary>
        /// A purely white texture.
        /// </summary>
        Texture2D WhiteTexture { get; }

        UserInterfaceRenderer UserInterfaceRenderer { get; }

        Rectangle ScreenArea { get; }

        UserInterfaceScene CreateUserInterfaceScene();
    }

    public class DemoResources : IDemoResources
    {
        public DemoResources(GraphicsDevice graphics, ContentManager content, Rectangle screenArea)
        {
            GraphicsDevice = graphics;
            Content = new ContentProvider(content);

            UserInterfaceFactory = new UserInterfaceFactory(graphics, Content);

            UserInterfaceFactory.LocalizedContent = new LocalizedContentLayoutEngine(
                ContentLayoutEngine, new FakeTextRepository());

            ScreenArea = screenArea;

            PapyrusTheme.Register(Themes, Fonts, Content, ThemeLoader);

            UserInterfaceFactory.DefaultThemeKey = "Papyrus";

            WhiteTexture = new TextureBuilder(graphics).SolidColor(10, 10, Color.White);
        }

        public ResoucesConfiguration Config { get; set; } = new ResoucesConfiguration();

        public UserInterfaceFactory UserInterfaceFactory { get; set; }

        public UserInterfaceScene CreateUserInterfaceScene()
            => UserInterfaceFactory.CreateUserInterfaceScene();

        public GraphicsDevice GraphicsDevice { get; set; }

        public IContentProvider Content { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public Texture2D WhiteTexture { get; set; }

        public Rectangle ScreenArea
        {
            get => UserInterfaceFactory.Config.ScreenArea;
            set => UserInterfaceFactory.Config.ScreenArea = value;
        }

        public FontProvider Fonts => UserInterfaceFactory.Fonts;

        IFontProvider IDemoResources.Fonts => Fonts;

        public ContentLayoutEngine ContentLayoutEngine => UserInterfaceFactory.ContentLayoutEngine;

        public IContentLayoutEngine LocalizedContent => UserInterfaceFactory.LocalizedContent;
        public ThemeCollection Themes => UserInterfaceFactory.Themes;

        public UserInterfaceRenderer UserInterfaceRenderer => UserInterfaceFactory.Renderer;

        public IStyleConfigurator StyleConfigurator => UserInterfaceFactory.StyleConfigurator;

        public ThemeLoader ThemeLoader => UserInterfaceFactory.ThemeLoader;
    }

    public class ResoucesConfiguration
    {
        public string DefaultTheme { get; set; } = "Papyrus";
    }
}
