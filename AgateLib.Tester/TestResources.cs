using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.Scenes;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests
{
    public interface ITestResources
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
    }

    public class TestResources : ITestResources
    {
        public GraphicsDevice GraphicsDevice { get; set; }

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
