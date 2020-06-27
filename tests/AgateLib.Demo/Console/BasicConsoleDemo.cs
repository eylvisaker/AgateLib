using AgateLib.Diagnostics;
using AgateLib.Diagnostics.Rendering;
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics.TwoDimensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AgateLib.Demo.Console
{
    public class BasicConsoleDemo : IDemo
    {
        private Texture2D whiteTexture;
        private SpriteBatch spriteBatch;
        private AgateConsole console;
        private Font font;
        private PhysicalParticle player;
        private Texture2D playerImage;
        private readonly List<Point> points = new List<Point>();

        public string Name => "Basic Console";

        public string Category => "Console";

        public event Action OnExit;

        public Rectangle ScreenArea { get; set; }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            DrawBoxes();

            string text = "Press ~ to toggle console.";
            font.TextAlignment = OriginAlignment.BottomCenter;
            font.Size = 26;

            font.DrawText(spriteBatch, new Vector2(ScreenArea.Width / 2, ScreenArea.Height - 50), text);

            spriteBatch.Draw(playerImage,
                             player.Position,
                             null,
                             Color.White,
                             player.Angle,
                             new Vector2(playerImage.Width, playerImage.Height) * 0.5f,
                             1,
                             SpriteEffects.None,
                             0);

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

        public void Initialize(IDemoResources resources)
        {
            whiteTexture = resources.WhiteTexture;
            spriteBatch = new SpriteBatch(resources.GraphicsDevice);
            console = new AgateConsole(new ConsoleRenderer(resources.GraphicsDevice,
                new ConsoleTextEngine(resources.Fonts)));

            console.Initialize(new Size(resources.GraphicsDevice.Viewport.Width, resources.GraphicsDevice.Viewport.Height));
            console.Quit += () => OnExit?.Invoke();

            font = new Font(resources.Fonts.Default);

            console.AddCommands(new BoxVocabulary(points));

            player = new PhysicalParticle
            {
                Position = ScreenArea.Size.ToVector2() * 0.5f,
                AngularVelocity = 1,
            };

            playerImage = resources.Content.Load<Texture2D>("agatelogo");
        }

        public void Update(GameTime gameTime)
        {
            console.Update(gameTime);

            if (console.PauseGame)
                return;

            player.Integrate((float)gameTime.ElapsedGameTime.TotalSeconds);

            var gp = GamePad.GetState(PlayerIndex.One);

            player.Force = 250 * gp.ThumbSticks.Left;
            player.Force.Y *= -1;

            if (player.Position.X < 50 || player.Position.X > ScreenArea.Width - 50)
                player.Velocity.X *= -1;
            if (player.Position.Y < 50 || player.Position.Y > ScreenArea.Height - 50)
                player.Velocity.Y *= -1;
        }
    }
}
