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

using AgateLib.Diagnostics.ConsoleAppearance;
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AgateLib.Diagnostics.Rendering
{
    public interface IConsoleRenderer
    {
        IConsoleTheme Theme { get; set; }

        ConsoleState State { get; set; }

        void Draw(GameTime time);

        void Update(GameTime time);
    }

    [Transient]
    public class ConsoleRenderer : IConsoleRenderer
    {
        private const double heightCoverage = 5 / 12.0;

        private readonly GraphicsDevice graphicsDevice;
        private readonly IConsoleTextEngine textEngine;
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D whiteTexture;

        private Size size;
        private int entryHeight;
        private long timeOffset;
        private double viewShiftPixels;

        private BlendState renderTargetBlendState = new BlendState
        {
            AlphaBlendFunction = BlendFunction.Add,
            AlphaDestinationBlend = Blend.InverseSourceAlpha,
            AlphaSourceBlend = Blend.SourceAlpha,

            ColorBlendFunction = BlendFunction.Add,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorSourceBlend = Blend.SourceAlpha,
        };

        [Obsolete("Use State.Theme instead")]
        private IConsoleTheme theme { get => State.Theme; set => State.Theme = value; }

        private RenderTarget2D renderTarget;
        private Size displaySize;
        private long CurrentTime;

        public ConsoleRenderer(GraphicsDevice graphicsDevice,
            IConsoleTextEngine textEngine,
            ITextureBuilder textureBuilder = null)
        {
            this.graphicsDevice = graphicsDevice;
            this.textEngine = textEngine;

            textureBuilder = textureBuilder ?? new TextureBuilder(graphicsDevice);

            spriteBatch = new SpriteBatch(graphicsDevice);
            whiteTexture = textureBuilder.SolidColor(10, 10, Color.White);

            ResizeRenderTarget();
            //console.KeyProcessed += (sender, e) => { timeOffset = CurrentTime; };
        }

        private IReadOnlyList<ConsoleMessage> Messages => State.Messages;

        public IConsoleTheme Theme
        {
            get => State.Theme;
            set
            {
                Require.ArgumentNotNull(value, nameof(Theme));
                State.Theme = value;

                foreach (var message in State.Messages)
                    message.ClearCache();
            }
        }

        public double Alpha { get; set; } = 0.95;

        public Font Font => textEngine.Font;

        public ConsoleState State { get; set; }

        private Rectangle ConsoleWindowDestRect => new Rectangle(0, 0,
                    graphicsDevice.PresentationParameters.BackBufferWidth,
                    (int)(graphicsDevice.PresentationParameters.BackBufferHeight * heightCoverage));

        public void Draw(GameTime time)
        {
            CurrentTime = (long)time.TotalGameTime.TotalMilliseconds;

            if (State.DisplayMode == ConsoleDisplayMode.None)
                return;

            if (State.DisplayMode == ConsoleDisplayMode.RecentMessagesOnly)
            {
                DrawRecentMessages();
                return;
            }

            Redraw(renderTarget);

            graphicsDevice.SetRenderTarget(null);
            BlitToScreen();
        }

        private void BlitToScreen()
        {
            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            spriteBatch.Draw(renderTarget, ConsoleWindowDestRect, new Color(Color.White, Theme.Opacity));

            spriteBatch.End();
        }

        public void Update(GameTime time)
        {
            CurrentTime = (long)time.TotalGameTime.TotalMilliseconds;

            if (State.Theme == null)
                State.Theme = ConsoleThemes.Default;

            UpdateViewShift(time.ElapsedGameTime.TotalSeconds);

            ResizeRenderTarget();
        }

        private void UpdateViewShift(double elapsedSeconds)
        {
            const int maxDelta = 100;
            const int viewShiftSpeed = 75;

            int targetViewShift = Font.FontHeight * State.ViewShift;
            double delta = targetViewShift - viewShiftPixels;

            if (Math.Abs(delta) > 0.001)
            {
                delta = Math.Sign(delta) * Math.Min(Math.Abs(delta), maxDelta);

                double amount = delta * viewShiftSpeed * elapsedSeconds;

                //if (Math.Abs(amount) > maxAmount)
                //	amount = Math.Sign(amount) * maxAmount;

                viewShiftPixels += amount;

                if ((viewShiftPixels - targetViewShift) * delta > 0)
                {
                    viewShiftPixels = targetViewShift;
                }
            }
        }

        private void ResizeRenderTarget()
        {
            displaySize = new Size(
                graphicsDevice.PresentationParameters.BackBufferWidth,
                graphicsDevice.PresentationParameters.BackBufferHeight);

            int width = displaySize.Width;
            int height = (int)(displaySize.Height * heightCoverage);

            if (width > 1920)
                width = 1920;
            if (height > 1080 * heightCoverage)
                height = (int)(1080 * heightCoverage);

            var newSize = new Size(width, height);

            if (renderTarget == null || newSize != size)
            {
                renderTarget?.Dispose();
                renderTarget = new RenderTarget2D(graphicsDevice, width, height);
                size = newSize;
            }
        }

        private void Redraw(RenderTarget2D renderTarget)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Theme.BackgroundColor);

            spriteBatch.Begin(blendState: renderTargetBlendState);

            DrawConsoleWindow();

            spriteBatch.End();
        }

        private void DrawRecentMessages()
        {
            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            long time = CurrentTime;
            int y = 0;
            Font.TextAlignment = OriginAlignment.TopLeft;
            Font.Color = Theme.RecentMessageColor;

            for (int i = 0; i < Messages.Count; i++)
            {
                if (time - Messages[i].Time > 5000)
                    continue;
                if (Messages[i].MessageType != ConsoleMessageType.Text)
                    continue;

                Font.DrawText(spriteBatch, new Vector2(0, y), Messages[i].Text);
                y += Font.FontHeight;
            }

            spriteBatch.End();
        }

        private void DrawConsoleWindow()
        {
            DrawUserEntry();

            DrawHistory();
        }

        private void DrawHistory()
        {
            var y = size.Height - entryHeight;

            //Display.PushClipRect(new Rectangle(0, 0, size.Width, y));

            y += (int)viewShiftPixels;

            for (int i = Messages.Count - 1; i >= 0; i--)
            {
                var message = Messages[i];
                var messageTheme = Theme.MessageTheme(message);

                if (message.Layout == null)
                {
                    var text = message.Text;

                    if (message.MessageType == ConsoleMessageType.UserInput)
                    {
                        text = Theme.EntryPrefix + message.Text;
                    }

                    Font.Color = messageTheme.ForeColor;
                    message.Layout = textEngine.LayoutContent(
                        text, size.Width);
                }

                y -= message.Layout.Size.Height;

                if (messageTheme.BackColor.A > 0)
                {
                    FillRect(messageTheme.BackColor,
                        new Rectangle(0, y, size.Width, message.Layout.Size.Height));
                }

                message.Layout.Draw(new Vector2(0, y), spriteBatch);

                if (y < 0)
                    break;
            }
        }

        private void DrawUserEntry()
        {
            int y = size.Height;
            Font.TextAlignment = OriginAlignment.BottomLeft;

            string currentLineText = Theme.EntryPrefix;

            currentLineText += EscapeText(State.InputText);

            entryHeight = Font.FontHeight;

            if (Theme.EntryBackgroundColor.A > 0)
            {
                FillRect(Theme.EntryBackgroundColor,
                    new Rectangle(0, size.Height - entryHeight, size.Width, entryHeight));
            }

            Font.Color = Theme.EntryColor;
            Font.DrawText(spriteBatch, 0, y, currentLineText);

            // draw insertion point
            if ((CurrentTime - timeOffset) % 1000 < 500)
            {
                int x = Font.MeasureString(currentLineText.Substring(0, Theme.EntryPrefix.Length + State.InsertionPoint)).Width;

                DrawLine(Theme.EntryColor,
                    new Vector2(x, y - Font.FontHeight),
                    new Vector2(x, y));
            }

            Font.TextAlignment = OriginAlignment.TopLeft;
        }

        private void DrawLine(Color color, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;

            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(whiteTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will stretch the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        private void FillRect(Color color, Rectangle rectangle)
        {

            spriteBatch.Draw(whiteTexture, rectangle, color);
        }

        private string EscapeText(string p)
        {
            return p?.Replace("{", "{{}");
        }
    }
}
