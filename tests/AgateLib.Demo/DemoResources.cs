using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
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
    }

    public class DemoResources : IDemoResources
    {
        public GraphicsDevice GraphicsDevice { get; set; }

        public Rectangle ScreenArea { get; set; }

        public IContentProvider Content { get; set; }

        public IFontProvider Fonts { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public Texture2D WhiteTexture { get; set; }
        public ContentLayoutEngine ContentLayoutEngine { get; set; }

        public IContentLayoutEngine LocalizedContent { get; set; }
        public ThemeCollection Themes { get; set; }

        public UserInterfaceRenderer UserInterfaceRenderer { get; set; }
        public IStyleConfigurator StyleConfigurator { get; set; }
        public ThemeLoader ThemeLoader { get; internal set; }
    }
}
