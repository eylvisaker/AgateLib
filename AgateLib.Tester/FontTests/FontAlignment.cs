using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Display;
using AgateLib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.Tests.FontTests
{
    class FontAlignment : ITest
    {
        private readonly KeyboardInput keyboard = new KeyboardInput();

        private readonly int[] numbers = 
            { 0, 0, 1, 11, 22, 33, 44, 99, 100, 111, 222, 333, 444, 555, 666, 777, 888, 999 };

        private SpriteBatch spriteBatch;
        private List<Font> fonts = new List<Font>();
        private Texture2D blank;

        private int fontIndex;

        public FontAlignment()
        {
            keyboard.KeyDown += Keyboard_KeyDown;
        }
        
        public string Name => "Font Alignment";

        public string Category => "Fonts";

        public Action OnExit { get; set; }

        public Rectangle ScreenArea { get; set; }

        public void Initialize(ITestResources resources)
        {
            fonts = new List<Font>();

            fonts.AddRange(resources.Fonts.Select(x => new Font(x)));
            fonts.AddRange(resources.Fonts.Select(x => new Font(x) { Size = 16 }));
            fonts.AddRange(resources.Fonts.Select(x => new Font(x) { Size = 14, Style = FontStyles.Bold }));

            spriteBatch = new SpriteBatch(resources.GraphicsDevice);

            blank = new TextureBuilder(resources.GraphicsDevice).SolidColor(10, 10, Color.White);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            var firstLineFont = fonts.First();
            var firstLineHeight = firstLineFont.FontHeight;

            IFont f = fonts[fontIndex];
            
            spriteBatch.Draw(blank, new Rectangle(0, firstLineHeight, 300, 600), Color.DarkSlateGray);
            spriteBatch.Draw(blank, new Rectangle(300, firstLineHeight, 300, 600), Color.DarkBlue);

            firstLineFont.TextAlignment = OriginAlignment.TopLeft;
            firstLineFont.Color = Color.White;
            firstLineFont.DrawText(spriteBatch, 0, 0, "Press space to cycle fonts.");

            f.Color = Color.White;
            f.TextAlignment = OriginAlignment.TopLeft;
            f.DrawText(spriteBatch, 0, firstLineHeight, "Left-aligned numbers");

            for (int i = 1; i < numbers.Length; i++)
            {
                f.DrawText(spriteBatch, 0, firstLineHeight + i * f.FontHeight, numbers[i].ToString());
            }

            f.TextAlignment = OriginAlignment.TopRight;
            f.DrawText(spriteBatch, 600, firstLineHeight, "Right-aligned numbers");

            for (int i = 1; i < numbers.Length; i++)
            {
                f.DrawText(spriteBatch, 600.0f, firstLineHeight + i * f.FontHeight, numbers[i].ToString());
            }

            spriteBatch.End();
        }
        
        public void Update(GameTime gameTime)
        {
            keyboard.Update(gameTime);

            if (fontIndex >= fonts.Count)
                fontIndex = 0;
        }


        private void Keyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Space)
                fontIndex++;
        }
    }
}
