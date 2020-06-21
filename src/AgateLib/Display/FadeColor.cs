using Microsoft.Xna.Framework;
using System;

namespace AgateLib.Display
{
    public class FadeColor
    {
        private enum FadeMode
        {
            Inactive,
            FadingIn,
            Active,
            FadingOut,
        }

        private FadeMode mode;
        private float fade;
        private float fadeTime_s = 0.5f;

        public TimeSpan FadeTime
        {
            get => TimeSpan.FromSeconds(fadeTime_s);
            set => fadeTime_s = (float)value.TotalSeconds;
        }

        public Color ActiveColor { get; set; } = Color.Black * 0.25f;

        public Color InactiveColor { get; set; } = new Color(0, 0, 0, 0);

        public Color CurrentColor => new Color(
            (byte)(InactiveColor.R * (1 - fade) + ActiveColor.R * fade),
            (byte)(InactiveColor.G * (1 - fade) + ActiveColor.G * fade),
            (byte)(InactiveColor.B * (1 - fade) + ActiveColor.B * fade),
            (byte)(InactiveColor.A * (1 - fade) + ActiveColor.A * fade));

        public void Update(GameTime gameTime)
        {
            switch (mode)
            {
                case FadeMode.Inactive:
                    fade = 0;
                    return;

                case FadeMode.Active:
                    fade = 1;
                    return;
            }

            float step = (float)gameTime.ElapsedGameTime.TotalSeconds / fadeTime_s;

            switch (mode)
            {
                case FadeMode.FadingIn:
                    fade += step;
                    if (fade >= 1)
                    {
                        mode = FadeMode.Active;
                        fade = 1;
                    }
                    break;

                case FadeMode.FadingOut:
                    fade -= step;
                    if (fade <= 0)
                    {
                        mode = FadeMode.Inactive;
                        fade = 0;
                    }
                    break;
            }
        }

        public void FadeIn()
        {
            mode = FadeMode.FadingIn;
        }

        public void FadeOut()
        {
            mode = FadeMode.FadingOut;
        }
    }
}
