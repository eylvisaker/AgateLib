using AgateLib.Display;
using AgateLib.Tests.Fakes;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Tests
{
    public static class CommonMocks
    {
        /// <summary>
        /// Returns a pair of Mock&lt;Widget&gt; and Mock&lt;RenderElement&gt; 
        /// objects.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static (Mock<Widget<WidgetProps, WidgetState>>, Mock<RenderElement<RenderElementProps>>)
            Widget(string name, bool elementCanHaveFocus = false)
        {
            var elementProps = new RenderElementProps();
            var renderResult = new Mock<RenderElement<RenderElementProps>>(elementProps);

            renderResult.CallBase = true;
            renderResult.Setup(x => x.CanHaveFocus).Returns(elementCanHaveFocus);
            renderResult.Setup(x => x.Children).Returns(new List<IRenderElement>());

            var widgetProps = new WidgetProps { Name = name };
            Mock<Widget<WidgetProps, WidgetState>> widgetResult =
                new Mock<Widget<WidgetProps, WidgetState>>(widgetProps);

            widgetResult.CallBase = true;
            widgetResult.Setup(x => x.Render()).Returns(renderResult.Object);

            return (widgetResult, renderResult);
        }


        /// <summary>
        /// Constructs a font provider, with an optional list of available fake fonts.
        /// If no values are supplied, a single default font will be available in the font provider.
        /// </summary>
        /// <param name="fontNames">An array of font names to be available in the provider. The first one is the default font.</param>
        /// <returns></returns>
        public static Mock<IFontProvider> FontProvider(params string[] fontNames)
        {
            var fontProvider = new Mock<IFontProvider>();
            var fonts = fontNames.ToList();
            Font defaultFont = null;

            if (fonts.Count == 0)
                fonts.Add("default");

            foreach (var fontName in fonts)
            {
                var font = new Font(new FakeFontCore(fontName));

                defaultFont = defaultFont ?? font;

                fontProvider.Setup(x => x[fontName]).Returns(font);
            }

            fontProvider.SetupGet(x => x.Default).Returns(defaultFont);

            return fontProvider;
        }

        public static Mock<IUserInterfaceRenderContext> RenderContext(IContentLayoutEngine contentLayoutEngine = null)
        {
            var styleRenderer = new Mock<IComponentStyleRenderer>();
            var uiRenderer = new UserInterfaceRenderer(styleRenderer.Object);

            var result = new Mock<IUserInterfaceRenderContext>();

            result.SetupGet(x => x.UserInterfaceRenderer)
                .Returns(uiRenderer);

            contentLayoutEngine = contentLayoutEngine ??
                new ContentLayoutEngine(FontProvider().Object);

            result
                .Setup(x => x.CreateContentLayout(It.IsAny<string>(), It.IsAny<ContentLayoutOptions>(), It.IsAny<bool>()))
                .Returns<string, ContentLayoutOptions, bool>((text, options, localize) => contentLayoutEngine.LayoutContent(text, options, localize));

            result.SetupGet(x => x.GameTime).Returns(new GameTime());

            return result;
        }

        public static Mock<IRenderElementStyle> RenderElementStyle()
        {
            return new Mock<IRenderElementStyle>();
        }

        public static Mock<IStyleConfigurator> StyleConfigurator()
        {
            return new Mock<IStyleConfigurator>();
        }
    }
}
