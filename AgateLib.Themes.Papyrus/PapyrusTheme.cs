using AgateLib.Display;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Styling.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Themes.Papyrus.Content
{
    public static class PapyrusTheme 
    {
        public static string[] ThemeKeys { get; } = new string[] { "Papyrus" };
        public static string[] Fonts { get; } = new string[] { "KingthingsPetrock" };

        public static void Register(ThemeCollection themes, FontProvider fonts, IContentProvider content, ThemeLoader themeLoader)
        {
            fonts.Add("KingthingsPetrock", Font.Load(content, "AgateLib.Themes.Papyrus/Fonts/KingthingsPetrock.afont"));

            themes["Papyrus"] = themeLoader.LoadTheme("AgateLib.Themes.Papyrus/UserInterface/Papyrus.atheme");
        }
    }
}
