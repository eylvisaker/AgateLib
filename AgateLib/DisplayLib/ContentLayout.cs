using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public class ContentLayout
	{
		class TextContent
		{
			public Point Destination;
			public string Text;
			public IFont Font;
			public int Size;
			public FontStyles Style;
			public Color Color;

			public void Draw(Point origin)
			{
				Point target = new Point(Destination.X + origin.X, Destination.Y + origin.Y);

				Font.Color = Color;
				Font.Size = Size;
				Font.Style = Style;

				Font.DrawText(target, Text);
			}
		}

		private IFont font;
		private List<TextContent> layouts = new List<TextContent>();
		private Point insertionPoint;

		public ContentLayout(IFont font, int? maxWidth)
		{
			this.font = font;
			MaxWidth = maxWidth;
		}

		public int? MaxWidth { get; private set; }

		public int Height
		{
			get
			{
				var lastTop = layouts.Max(x => x.Destination.Y);
				var items = layouts.Where(x => x.Destination.Y == lastTop);

				return items.Max(x => x.Destination.Y + font.MeasureString(x.Text).Height);
			}
		}

		private void Add(string text)
		{
			layouts.Add(new TextContent
			{
				Destination = insertionPoint,
				Font = font,
				Size = font.Size,
				Style = font.Style,
				Color = font.Color,
				Text = text
			});

			var size = font.MeasureString(text);

			insertionPoint.X += size.Width;
		}

		public void Append(string text)
		{
			LayoutText(text);
		}

		private void LayoutText(string text)
		{
			List<int> spacePositions = new List<int>();
			List<int> wrapPositions = new List<int>();

			for (int i = 0; i < text.Length; i++)
				if (text[i] == ' ' || text[i] == '\n')
					spacePositions.Add(i);

			spacePositions.Add(text.Length);

			int lineStart = 0;
			int x = insertionPoint.X;
			for (int i = 1; i < spacePositions.Count; i++)
			{
				if (spacePositions[i] < text.Length)
				{
					char ch = text[spacePositions[i]];
					if (ch == '\n')
					{
						lineStart = spacePositions[i] + 1;
						wrapPositions.Add(spacePositions[i] + 1);
						continue;
					}
				}

				int length = spacePositions[i] - lineStart;
				string subText = text.Substring(lineStart, length);
				int right = font.MeasureString(subText).Width + x;

				if (right > MaxWidth)
				{
					lineStart = spacePositions[i - 1] + 1;
					wrapPositions.Add(lineStart);
					x = 0;
				}
			}

			wrapPositions.Add(text.Length);
			wrapPositions.Sort();

			int writeLineStart = 0;

			Point dest = insertionPoint;

			for (int i = 0; i < wrapPositions.Count; i++)
			{
				if (i > 0)
				{
					insertionPoint.Y += font.FontHeight;
					insertionPoint.X = 0;
				}

				int length = wrapPositions[i] - writeLineStart;

				string subText = text.Substring(writeLineStart, length);

				Add(subText);

				writeLineStart = wrapPositions[i];
			}
		}

		public void Draw(Point dest)
		{
			foreach(var item in layouts)
			{
				item.Draw(dest);
			}
		}
	}
}
