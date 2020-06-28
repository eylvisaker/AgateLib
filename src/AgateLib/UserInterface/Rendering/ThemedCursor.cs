//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics.TwoDimensions;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface.Styling.Themes.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;

namespace AgateLib.UserInterface.Rendering
{
    public class ThemedCursor : ICursor
    {
        private Texture2D texture;
        private readonly ITheme theme;
        private readonly Dictionary<string, CursorTheme> cursorModels;

        private Vector2 startPosition, targetPosition;
        private Rectangle targetFocusRect;

        private CursorTheme activeModel;
        private readonly CursorTheme defaultModel;
        private PhysicalParticle physics = new PhysicalParticle();
        private IRenderElement lastFocus;
        private StepAnimator movementAnimator = new StepAnimator();

        public ThemedCursor(ITheme theme, string defaultCursor = null)
        {
            this.theme = theme;

            this.cursorModels = theme.Model.Cursors ?? new Dictionary<string, CursorTheme>();

            defaultCursor = defaultCursor ?? Defaults.Instance.Cursor;

            if (cursorModels.Count == 0)
                defaultModel = new CursorTheme();
            else if (cursorModels.ContainsKey(defaultCursor))
                defaultModel = cursorModels[defaultCursor];
            else
                defaultModel = cursorModels.FirstOrDefault().Value;

            ActivateModel(defaultModel);
        }

        private void ActivateModel(CursorTheme newModel)
        {
            activeModel = newModel;

            if (activeModel.Image?.Filename == null)
            {
                texture = null;
            }
            else
            {
                texture = theme.LoadContent<Texture2D>(ThemePathTypes.Cursors | ThemePathTypes.Images,
                                    activeModel.Image.Filename);
            }
        }

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public Vector2 Velocity
        {
            get => physics.Velocity;
            set => physics.Velocity = value;
        }

        public Vector2 Position
        {
            get => physics.Position;
            set
            {
                physics.Position = value;
                targetPosition = value;
            }
        }

        /// <summary>
        /// Point direction in screen-2d coordinates (origin top-left, +x is right, +y is down)
        /// </summary>
        public Vector2 PointDirection { get; set; } = new Vector2(1, 0);

        /// <summary>
        /// Color to apply to cursor.
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Draws the pointer onscreen.
        /// </summary>
        public void Draw(GameTime gameTime, ICanvas canvas, float scale)
        {
            DrawPointer(canvas, scale: scale);
        }

        /// <summary>
        /// Notifies the pointer that it should move to hover over the focus widget.
        /// </summary>
        /// <param name="activeWorkspace"></param>
        /// <param name="focusAnimatedContentRect"></param>
        /// <param name="focusElement">The element that has focus.</param>
        /// <param name="focusContentRect">The screen coordinates of the content area of the focus element.</param>
        public void MoveToFocus(Workspace activeWorkspace, IRenderElement focusElement, Rectangle focusContentRect, Rectangle focusAnimatedContentRect)
        {
            const int overlap = 1;

            if (focusElement == lastFocus)
                return;

            startPosition = Position;

            if (texture == null)
            {
                targetPosition = focusContentRect.CenterPointAsVector();
            }
            else
            {
                targetPosition = new Vector2(focusContentRect.Left + overlap,
                                             (focusContentRect.Top + focusContentRect.Bottom) / 2);
            }

            targetFocusRect = focusAnimatedContentRect;

            movementAnimator.InputValue = 0;
            movementAnimator.Rate = activeModel.Speed * focusElement.Display.VisualScaling
                                                      / DisplacementToTarget.Length();

            movementAnimator.EasingFunction = Ease.Interpolate(Ease.SmoothStart2, Ease.SmoothStop4);

            Velocity = Vector2.Zero;
        }

        private bool AtTarget => (Position - targetPosition).LengthSquared() < 1;

        private Vector2 DisplacementToTarget
            => targetPosition - Position;

        private Vector2 DirectionToTarget
        {
            get
            {
                Vector2 dir = targetPosition - Position;
                dir.Normalize();
                return dir;
            }
        }

        public void Update(GameTime gameTime)
        {
            movementAnimator.Update(gameTime);

            const int bounce = 10;

            physics.Position = movementAnimator.T * (targetPosition - startPosition) + startPosition;
            physics.Position.Y -= movementAnimator.S * bounce;
        }

        protected virtual void DrawPointer(ICanvas canvas,
                                           Texture2D useTexture = null,
                                           int layerDepth = 0,
                                           float scale = 1)
        {
            useTexture = useTexture ?? texture;

            if (useTexture == null)
            {
                const int outer_radius = 20;
                const int inner_radius = 5;
                const int line_radius = 25;
                const int line_thick = 2;

                canvas.DrawEllipse(Color,
                    new RectangleF(Position.X - outer_radius / 2,
                                   Position.Y - outer_radius / 2,
                                   outer_radius * 2,
                                   outer_radius * 2));

                canvas.DrawEllipse(Color,
                    new RectangleF(Position.X - inner_radius / 2,
                                   Position.Y - inner_radius / 2,
                                   inner_radius * 2,
                                   inner_radius * 2));

                canvas.FillRect(
                    Color,
                    RectangleF.FromLTRB(Position.X - line_radius, 
                                        Position.Y - line_thick,
                                        Position.X + line_radius, 
                                        Position.Y + line_thick));

                canvas.FillRect(
                    Color, 
                    RectangleF.FromLTRB(Position.X - line_thick, 
                                        Position.Y - line_radius,
                                        Position.X + line_thick, 
                                        Position.Y + line_radius));

                return;
            }

            Rectangle pointerDest = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                texture.Width,
                texture.Height);

            var angle = physics.Rotation;

            angle += activeModel.Image.Angle * AgateLib.Mathematics.MathX.DegreesToRadians;

            canvas.Draw(texture,
                        Position,
                        activeModel.Image.SourceRect,
                        Color,
                        angle,
                        activeModel.Image.Anchor.ToVector2(),
                        scale * activeModel.Image.Scale,
                        SpriteEffects.None,
                        layerDepth);
        }
    }
}