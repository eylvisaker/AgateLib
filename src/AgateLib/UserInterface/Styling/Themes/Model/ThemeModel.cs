using AgateLib.Display.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Styling.Themes.Model
{
    [SerializationType]
    public class ThemeModel
    {
        public Dictionary<string, CursorTheme> Cursors { get; set; }
        public ThemeModelPaths Paths { get; set; }
        public ThemeBackgrounds Backgrounds { get; set; }
        public ThemeBorders Borders { get; set; }

        public ThemeStylePatternList Styles { get; set; } = new ThemeStylePatternList();


        public class ThemeModelPaths
        {
            public string Images { get; set; }
            public string Cursors { get; set; }
        }

        public class ThemeBackgrounds : Dictionary<string, BackgroundStyle>
        {
        }

        public class ThemeBorders : Dictionary<string, BorderModel>
        {
        }
    }

    [SerializationType]
    public class CursorTheme
    {
        public CursorImage Image { get; set; }
        public List<CursorParticleModel> Particles { get; set; }

        public float Speed { get; set; } = 1500;

        /// <summary>
        /// The point inside the source-rect which is to be always painted at this point on screen. Rotations will all happen
        /// around this point.
        /// </summary>
        public Point Anchor { get; set; }

        public class CursorImage
        {
            public string Filename { get; set; }
            public Rectangle SourceRect { get; set; }

            /// <summary>
            /// The angle the pointer is pointing in the image, in the physics-2D coordinate system.
            /// (That means origin in the center of your paper, +x is to the right and +y is up,
            /// as opposed to the screen-2D coordinate system, where origin is in top-left, +x is to the right and +y is down the screen).
            /// </summary>
            public float Angle { get; set; }

            public float Scale { get; set; }

            public Point Anchor { get; set; }
        }

        public class CursorParticleModel
        {
            public FrameOrder FrameOrder { get; set; }

            public List<ParticleSpriteFrame> Frames { get; set; }

            public class ParticleSpriteFrame
            {
                public Rectangle SourceRect { get; set; }
            }
        }


        public enum FrameOrder
        {
            Lifetime,
            Random,
        }
    }
}
