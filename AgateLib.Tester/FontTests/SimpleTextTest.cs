using AgateLib.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AgateLib.Tests.FontTests
{
	public class SimpleTextTest : ITest

	{
		private PaletteC64 palette = new PaletteC64();
		private SpriteBatch spriteBatch;
		private Font font;

		public string Name => "Simple Text";

		public string Category => "Font";

        public Action OnExit { get; set; }

        public Rectangle ScreenArea { get; set; }

        public void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();

			Vector2 dest = new Vector2();

			foreach(var color in palette)
			{
				font.Color = color;

				font.DrawText(spriteBatch, dest, "This is a test!");

				dest.X += font.FontHeight;
				dest.Y += font.FontHeight;
			}

			spriteBatch.End();
		}

		public void Initialize(ITestResources resources)
		{
			spriteBatch = new SpriteBatch(resources.GraphicsDevice);
			font = new Font(resources.Fonts.Default);
		}

		public void Update(GameTime gameTime)
		{
		}
	}
}
