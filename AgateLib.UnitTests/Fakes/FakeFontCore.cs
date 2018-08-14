//
//    Copyright (c) 2006-2018
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Display;
using AgateLib.Display.BitmapFont;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.Fakes
{
    public class FakeFontCore : IFontCore
    {
        public FakeFontCore(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the list of arguments passed for each call to IFontCore.DrawText.
        /// </summary>
        public List<FontDrawCall> DrawCalls { get; } = new List<FontDrawCall>();

        public string Name { get; set; }

        public int FontHeight { get; set; } = 10;

        int IFontCore.FontHeight(FontState state)
        {
            return CalcFontHeight(state);
        }

        private int CalcFontHeight(FontState state)
        {
            return (int)(FontHeight * state.Size / 10.0);
        }

        public IReadOnlyDictionary<FontSettings, IFontTexture> FontItems => throw new NotImplementedException();

        public void AddFontSurface(FontSettings settings, IFontTexture fontSurface)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public void DrawText(FontState state, string text)
        {
            LogDrawText(Vector2.Zero, state.Color, text);
        }

        public void DrawText(FontState state, Vector2 dest, string text)
        {
            LogDrawText(dest, state.Color, text);
        }

        public void DrawText(FontState state, SpriteBatch spriteBatch, Vector2 dest, string text)
        {
            LogDrawText(dest, state.Color, text);
        }

        public void DrawText(FontState state, Vector2 dest, string text, params object[] parameters)
        {
            LogDrawText(dest, state.Color, text);
        }

        public IFontTexture FontSurface(FontState fontState)
        {
            throw new NotImplementedException();
        }

        public FontSettings GetClosestFontSettings(FontSettings settings)
        {
            throw new NotImplementedException();
        }

        public Size MeasureString(FontState state, string text)
        {
            if (text.Length == 0)
                return Size.Empty;

            int lineLength = 0;
            int longestLine = 0;
            int lineCount = 1;
            var fontHeight = CalcFontHeight(state);

            for (int i = 0; i < text.Length; i++)
            {
                lineLength++;
                if (text[i] == '\n')
                {
                    lineLength = 0;
                    lineCount++;
                }

                longestLine = Math.Max(lineLength, longestLine);
            }

            return new Size(fontHeight / 2 * longestLine, fontHeight * lineCount);
        }

        private void LogDrawText(Vector2 dest, Color color, string text, params object[] parameters)
        {
            DrawCalls.Add(new FontDrawCall
            {
                Dest = dest,
                Color = color,
                Text = text,
                Parameters = parameters
            });

            System.Console.WriteLine($"{Name} draw to ({dest}): {color}");
            System.Console.WriteLine(text);
            LogParameters(parameters);
        }

        private void LogParameters(object[] parameters)
        {
            foreach (var p in parameters)
            {
                System.Console.WriteLine("    " + p);
            }
        }
    }

    public class FontDrawCall
    {
        public Vector2 Dest { get; set; }
        public Color Color { get; set; }
        public string Text { get; set; }
        public object[] Parameters { get; set; }
    }
}
