using AgateLib.UserInterface.Styling.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderElementStyle
    {
        //Overflow Overflow { get; }
    }

    public class RenderElementStyle : IRenderElementStyle
    {
        //public Overflow Overflow => Overflow.Visible;
    }

    public enum Overflow
    {
        Visible,
        Hidden,
        Scroll,
    }

    public static class RenderElementStyleExtensions
    {
        public static IRenderElementStyle ToElementStyle(this ThemeStyle themeStyle)
        {
            return new ThemeRenderElementStyle(themeStyle);
        }
    }

    public class ThemeRenderElementStyle : IRenderElementStyle
    {
        private ThemeStyle themeStyle;

        public ThemeRenderElementStyle(ThemeStyle themeStyle)
        {
            this.themeStyle = themeStyle;
        }
    }
}
