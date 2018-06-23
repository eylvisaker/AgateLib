using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Display;
using AgateLib.Fakes;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Rendering;
using Moq;

namespace AgateLib.UserInterface
{
    public static class CommonMocks
    {
        /// <summary>
        /// Returns a Mock&lt;Widget&gt; object which doesn't override any behavior of the widget.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Mock<Widget> Widget(string name)
        {
            Mock<Widget> result = new Mock<Widget>(name);

            result.CallBase = true;

            return result;
        }

        /// <summary>
        /// Constructs a font provider, with a list of available fake fonts.
        /// </summary>
        /// <param name="fontNames">An array of font names to be available in the provider. The first one is the default font.</param>
        /// <returns></returns>
        public static Mock<IFontProvider> FontProvider(params string[] fontNames)
        {
            var fontProvider = new Mock<IFontProvider>();
            Font defaultFont = null;

            foreach (var fontName in fontNames)
            {
                var font = new Font(new FakeFontCore(fontName));

                defaultFont = defaultFont ?? font;

                fontProvider.Setup(x => x[fontName]).Returns(font);
            }

            fontProvider.SetupGet(x => x.Default).Returns(defaultFont);

            return fontProvider;
        }

        public static Mock<IWidgetRenderContext> RenderContext()
        {
            var styleRenderer = new Mock<IComponentStyleRenderer>();
            var uiRenderer = new UserInterfaceRenderer(styleRenderer.Object);

            var result = new Mock<IWidgetRenderContext>();

            result.SetupGet(x => x.UserInterfaceRenderer)
                .Returns(uiRenderer);

            return result;
        }
    }
}
