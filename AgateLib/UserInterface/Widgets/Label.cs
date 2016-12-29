//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class Label : Widget, ITextAlignment
	{
		string mText = string.Empty;
		bool mWrapText;
		List<int> mWrapPositions = new List<int>();
		int mSlowReadPosition = 0;
		double mSlowReadTime;
		const double mSlowReadTextPeriod = 0.05;

		public Label()
		{
			TextAlign = OriginAlignment.TopLeft;
			WrapText = true;
		}

		public Label(string text)
			: this()
		{
			this.Text = text;
		}

		public string Text
		{
			get { return mText; }
			set
			{
				if (value == null)
					value = string.Empty;

				value = value.Replace("\r", "");
				if (value == mText)
					return;

				LayoutDirty = true;
				mText = value;

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
			if (Font == null)
				return;

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

				if (width > maxWidth)
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
			get { return mWrapPositions.Count * Font.FontHeight; }
		}
		int WrappedWidth
		{
			get
			{
				int lastPos = 0;
				int largestWidth = 0;

				for(int i = 0; i <= mWrapPositions.Count; i++)
				{
					string text;

					if (i == mWrapPositions.Count)
						text = mText.Substring(lastPos);
					else
					{
						text = mText.Substring(lastPos, mWrapPositions[i] - lastPos);
						lastPos = mWrapPositions[i];
					}

					largestWidth = Math.Max(largestWidth, Font.MeasureString(text).Width);
				}

				return largestWidth;
			}
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

		public override void DrawImpl(Rectangle screenRect)
		{
			Font.Color = FontColor;
			Font.DisplayAlignment = TextAlign;

			Point dest = screenRect.Location;

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

		internal override Size ComputeSize(int? maxWidth, int? maxHeight)
		{
			if (WrapText == false)
			{
				return Font.MeasureString(Text);
			}

			if (maxWidth != null)
			{
				RewrapText(maxWidth.Value);

				if (mWrapPositions.Count > 0)
					return new Size(WrappedWidth, WrappedHeight);
			}

			var result = Font.MeasureString(Text);
			return result;
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
