using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    public class ThemedAudio : IUserInterfaceAudio
    {
        public Desktop ActiveDesktop { get; set; }

        public bool PlaySound(object originator, UserInterfaceSound sound)
        {
            if (ActiveDesktop == null)
                return false;

            ITheme theme = ActiveDesktop.Styles.Theme(ActiveDesktop.Theme);

            if (originator is IRenderElement element)
            {
                theme = element.Display.Theme ?? theme;
            }

            if (theme.Model.Sounds.TryGetValue(sound, out string filename))
            {
                SoundEffect soundEffect = theme.LoadContent<SoundEffect>(ThemePathTypes.Sounds, filename);

                return soundEffect.Play();
            }

            return false;
        }
    }
}
