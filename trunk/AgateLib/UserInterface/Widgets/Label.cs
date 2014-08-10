﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class Label : Widget
	{
		string mText = string.Empty;
		bool mWrapText;
		List<int> mWrapPositions = new List<int>();
		int mSlowReadPosition = 0;
		double mSlowReadTime;
		const double mSlowReadTextPeriod = 0.05;

		public string Text
		{
			get { return mText; }
			set
			{
				if (value == null)
					value = string.Empty;

				mText = value.Replace("\r", "");

				if (WrapText)
				{
					RewrapText(Width);
				}

				mSlowReadPosition = 0;
				mSlowReadTime = 0;
			}
		}
		public object[] Parameters
		{
			get;set;	
		}
		
		private void RewrapText(int maxWidth)
		{
			mWrapPositions.Clear();

			List<int> spacePositions = new List<int>();

			for (int i = 0; i < mText.Length; i++)
				if (mText[i] == ' ' || mText[i] == '\n')
					spacePositions.Add(i);

			spacePositions.Add(mText.Length);

			int lineStart = 0;
			for (int i = 1; i < spacePositions.Count; i++)
			{
				if (spacePositions[i] < mText.Length)
				{
					char ch = mText[spacePositions[i]];
					if (ch == '\n')
					{
						lineStart = spacePositions[i] + 1;
						mWrapPositions.Add(spacePositions[i]+1);
						continue;
					}
				}

				int length = spacePositions[i] - lineStart;
				string text = mText.Substring(lineStart, length);
				int width = Font.MeasureString(text).Width;

				if (width > this.Width)
				{
					lineStart = spacePositions[i - 1] + 1;
					mWrapPositions.Add(lineStart);
				}
			}

			mWrapPositions.Add(mText.Length);
			mWrapPositions.Sort();
		}
		int WrappedHeight
		{
			get { return (mWrapPositions.Count + 1) * Font.FontHeight; }
		}

		public Label()
		{
			TextAlign = OriginAlignment.TopLeft;
		}
		public Label(string text)
			: this()
		{
			this.Text = text;
		}

		public override void Update(double delta_t, ref bool processInput)
		{
			if (SlowRead)
			{
				mSlowReadTime += delta_t;
				
				double period = mSlowReadTextPeriod;
				if (AccelerateSlowReading) period /= 3;

				if (mSlowReadTime >= period)
				{
					mSlowReadTime -= period;

					if (mSlowReadPosition < Text.Length)
					{
						do
						{
							mSlowReadPosition++;
						} while (mSlowReadPosition < Text.Length && IsWhiteSpace(Text[mSlowReadPosition]));
						
						if (mSlowReadPosition + 1 < Text.Length && Text[mSlowReadPosition - 1] == '{')
						{
							for (int i = 2; i < 4; i++)
							{
								if (mSlowReadPosition + i < Text.Length &&
									Text[mSlowReadPosition + i - 1] == '}')
								{
									mSlowReadPosition += i;
									break;
								}
							}
						}
					}
				}
			}
		}

		private bool IsWhiteSpace(char p)
		{
			if (p == ' ') return true;
			if (p == '\n') return true;
			if (p == '\r') return true;
			if (p == '\t') return true;

			return false;
		}

		public override void DrawImpl()
		{
			Font.Color = FontColor;
			Font.DisplayAlignment = TextAlign;

			Point dest = ClientToScreen(Point.Empty);

			switch (Font.DisplayAlignment)
			{
				case OriginAlignment.Center:
				case OriginAlignment.CenterLeft:
				case OriginAlignment.CenterRight:
					dest.Y += Height / 2;
					break;

				case OriginAlignment.BottomCenter:
				case OriginAlignment.BottomLeft:
				case OriginAlignment.BottomRight:
					dest.Y += Height;
					break;
			}
			switch (Font.DisplayAlignment)
			{
				case OriginAlignment.TopCenter:
				case OriginAlignment.Center:
				case OriginAlignment.BottomCenter:
					dest.X += Width / 2;
					break;

				case OriginAlignment.TopRight:
				case OriginAlignment.CenterRight:
				case OriginAlignment.BottomRight:
					dest.X += Width;
					break;
			}

			Surface image = null;

			if (WrapText == false || mWrapPositions.Count == 0)
			{
				if (image != null)
				{
					Font.DrawText(dest.X, dest.Y, "{0} " + Text, image);
				}
				else
					Font.DrawText(dest, Text);
			}
			else
			{
				RewrapText(Width);

				int lineStart = 0;
				for (int i = 0; i < mWrapPositions.Count; i++)
				{
					int length = mWrapPositions[i] - lineStart;
					
					if (SlowRead && lineStart + length > mSlowReadPosition)
						length = mSlowReadPosition - lineStart;

					string text = Text.Substring(lineStart, length);
					
					if (Parameters == null)
						Font.DrawText(dest.X, dest.Y, text);
					else 
						Font.DrawText(dest.X, dest.Y, text, Parameters);
					
					dest.Y += Font.FontHeight;

					lineStart = mWrapPositions[i];

					if (SlowRead && lineStart >= mSlowReadPosition)
						break;
				}
			}
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(Name))
			{
				return "Label: \"" + Text + "\"";
			}
			else
				return base.ToString();
		}

		internal override Size ComputeSize(int? minWidth, int? minHeight, int? maxWidth, int? maxHeight)
		{
			if (maxWidth != null)
			{
				RewrapText(maxWidth.Value);

				if (mWrapPositions.Count > 0)
					return new Size(maxWidth.Value, WrappedHeight);
			}

			var retval = Font.MeasureString(Text);
			return retval;
		}

		public OriginAlignment TextAlign { get; set; }

		public bool WrapText
		{
			get { return mWrapText; }
			set
			{
				mWrapText = value;

				if (mWrapText)
					RewrapText(Width);
			}
		}

		public bool SlowRead { get; set; }
		public bool IsSlowReading
		{
			get { return SlowRead && mSlowReadPosition < Text.Length; }
		}
		public bool AccelerateSlowReading { get; set; }
	}
}
