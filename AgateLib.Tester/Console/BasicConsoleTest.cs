using System;
using System.Collections.Generic;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.Rendering;
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.Console
{
    public class BasicConsoleTest : ITest
    {
        private Texture2D whiteTexture;
        private SpriteBatch spriteBatch;
        private AgateConsole console;
        private Font font;

        private readonly List<Point> points = new List<Point>();

        public string Name => "Basic Console";

        public string Category => "Console";

        public Action OnExit { get; set; }

        public Rectangle ScreenArea { get; set; }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            font.DrawText(spriteBatch, new Vector2(), "Press ~ to toggle console.");

            DrawBoxes();

            spriteBatch.End();

            console.Draw(gameTime);
        }

        private void DrawBoxes()
        {
            Size size = new Size(10, 10);

            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                Color clr = ColorX.FromHsv(i / 5.0, 1, 1);

                Rectangle dest = new Rectangle(point, size);

                spriteBatch.Draw(whiteTexture, dest, clr);
            }
        }

        public void Initialize(ITestResources resources)
        {
            whiteTexture = resources.WhiteTexture;
            spriteBatch = new SpriteBatch(resources.GraphicsDevice);
            console = new AgateConsole(new ConsoleRenderer(resources.GraphicsDevice,
                new ConsoleTextEngine(resources.Fonts)));

            font = new Font(resources.Fonts.Default);

            console.AddCommands(new BoxVocabulary(points));
        }

        public void Update(GameTime gameTime)
        {
            console.Update(gameTime);
        }
    }
}
