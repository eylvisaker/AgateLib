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
using AgateLib.Quality;
using AgateLib.UserInterface.Content.LayoutItems;
using AgateLib.UserInterface.Content.TextTokenizer;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface.Content
{
    public class LayoutContext
    {
        private List<string> tokens;
        private int currentTokenIndex = -1;
        private List<IContentLayoutItem> layoutItems = new List<IContentLayoutItem>();
        private readonly ContentLayoutOptions options;
        private readonly PrecendenceBasedTokenizer tokenizer = new PrecendenceBasedTokenizer();
        private int defaultLineHeight;

        public LayoutContext(List<string> tokens, ContentLayoutOptions layoutOptions)
        {
            Require.ArgumentNotNull(tokens, nameof(tokens));
            Require.ArgumentNotNull(layoutOptions, nameof(layoutOptions));

            this.tokens = tokens;
            this.options = layoutOptions;
            this.defaultLineHeight = layoutOptions.Font.FontHeight;

        }

        public bool AnyTokensLeft => currentTokenIndex < tokens.Count - 1;

        public string CurrentToken => currentTokenIndex < tokens.Count ? tokens[currentTokenIndex] : null;

        public IContentLayout Layout => new ContentLayout(layoutItems, defaultLineHeight);

        public ContentLayoutOptions Options => options;

        public int LineHeight { get; set; }

        public Font Font => options.Font;

        public string ReadNextToken()
        {
            currentTokenIndex++;

            return CurrentToken;
        }

        public void ExpandLineHeight(int height)
        {
            if (LineHeight < height)
                LineHeight = height;
        }

        private void ReserveWhiteSpace(string whiteSpace)
        {
            int length = whiteSpace.Count(x => x == ' ');
            int tabs = whiteSpace.Count(x => x == '\t');

            length += tabs * 4;

            GetLastItem().ExtraWhiteSpace += length * Font.MeasureString(" ").Width;
        }

        public void NewLine()
        {
            GetLastItem().NewLinesAfter++;
        }

        private IContentLayoutItem GetLastItem()
        {
            if (layoutItems.Count == 0)
            {
                layoutItems.Add(new ContentText("", new Font(Font)));
            }

            return layoutItems.Last();
        }

        //public void ApplyAlignment()
        //{
        //    if (options.TextAlign == TextAlign.Left)
        //        return;

        //    int maxLineWidth = FindMaxLineWidth();

        //    ApplyAlignment(maxLineWidth);
        //}

        //private int FindMaxLineWidth()
        //{
        //    int index = 0;

        //    float result = 0;

        //    while (index < layoutItems.Count)
        //    {
        //        int endOfLine = FindEndOfLine(index);
        //        IContentLayoutItem lastItem = layoutItems[endOfLine];

        //        float right = lastItem.Location.X + lastItem.Size.Width;

        //        result = Math.Max(result, right);

        //        index = endOfLine + 1;
        //    }

        //    return (int)Math.Ceiling(result);
        //}

        //private void ApplyAlignment(int maxLineWidth)
        //{
        //    if (options.TextAlign == TextAlign.Left)
        //        return;

        //    int index = 0;

        //    while (index < layoutItems.Count)
        //    {
        //        int endOfLine = FindEndOfLine(index);
        //        IContentLayoutItem lastItem = layoutItems[endOfLine];

        //        float right = lastItem.Location.X + lastItem.Size.Width;
        //        float space = maxLineWidth - right;

        //        if (options.TextAlign == TextAlign.Center)
        //            space /= 2;

        //        Vector2 displacement = new Vector2(space, 0);

        //        for (int i = index; i <= endOfLine; i++)
        //            layoutItems[i].Location += displacement;

        //        index = endOfLine + 1;
        //    }
        //}

        //int FindEndOfLine(int startIndex)
        //{
        //    float startY = layoutItems[startIndex].Location.Y;

        //    for (int i = startIndex; i < layoutItems.Count; i++)
        //    {
        //        if (layoutItems[i].Location.Y > startY)
        //            return i - 1;
        //    }

        //    return layoutItems.Count - 1;
        //}

        public void AppendText(string text, int recursion = 0)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var tokens = tokenizer.Tokenize(text);

            var font = new Font(Font);

            foreach (var token in tokens)
            {
                switch (token.TokenType)
                {
                    case TokenType.Word:
                        Add(new ContentText(token.Value, font));
                        break;

                    case TokenType.WhiteSpace:
                        ReserveWhiteSpace(token.Value);
                        break;

                    case TokenType.NewLine:
                        NewLine();
                        break;
                }
            }

            //while (text.StartsWith("\n") || text.StartsWith("\r\n"))
            //{
            //    NewLine();

            //    if (text.StartsWith("\n"))
            //    {
            //        if (text.Length == 1)
            //            return;

            //        text = text.Substring(1);
            //    }
            //    else if (text.StartsWith("\r\n"))
            //    {
            //        if (text.Length == 2)
            //            return;

            //        text = text.Substring(2);
            //    }
            //}

            //var newLineIndex = text.IndexOf('\n');

            //if (newLineIndex > 0)
            //{
            //    if (text[newLineIndex - 1] == '\r')
            //        newLineIndex--;

            //    AppendText(text.Substring(0, newLineIndex), recursion + 1);
            //    AppendText(text.Substring(newLineIndex), recursion + 1);
            //    return;
            //}

            //string thisLineText = text;
            //int advance = thisLineText.Length;

            //var textSize = font.MeasureString(thisLineText);

            ////while (InsertionPoint.X + textSize.Width > MaxWidth)
            ////{
            ////    var whiteSpaceIndex = thisLineText.LastIndexOf(' ');

            ////    if (whiteSpaceIndex <= 0)
            ////    {
            ////        if (InsertionPoint.X == 0)
            ////        {
            ////            // insert the word anyway, since it's too long for the full width.
            ////            break;
            ////        }

            ////        NewLine();

            ////        thisLineText = thisLineText.TrimStart();
            ////        textSize = font.MeasureString(thisLineText);
            ////        continue;
            ////    }

            ////    thisLineText = thisLineText.Substring(0, whiteSpaceIndex);
            ////    advance = whiteSpaceIndex;
            ////    textSize = font.MeasureString(thisLineText);
            ////}

            //var item = new ContentText(thisLineText, font);

            //Add(item);

            //if (advance < text.Length)
            //{
            //    var remainingText = text.Substring(advance);

            //    AppendText(remainingText, recursion + 1);
            //}
        }

        public void Add(IContentLayoutItem item)
        {
            layoutItems.Add(item);
        }

        public Size ScaleToLineHeight(Size size)
        {
            var ratio = Font.FontHeight / (float)size.Height;

            return new Size((int)(size.Width * ratio), (int)(size.Height * ratio));
        }
    }
}
