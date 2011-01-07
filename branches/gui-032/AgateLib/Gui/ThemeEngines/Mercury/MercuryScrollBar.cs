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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
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

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	/// <summary>
	/// Class which draws scroll bars for the Mercury theme engine.
	/// </summary>
	public class MercuryScrollBar : MercuryWidget
	{
		public Surface Decrease { get; set; }
		public Surface Increase { get; set; }

		public Surface DecreaseHover { get; set; }
		public Surface IncreaseHover { get; set; }

		public Surface DecreasePressed { get; set; }
		public Surface IncreasePressed { get; set; }

		public Surface DecreaseDisabled { get; set; }
		public Surface IncreaseDisabled { get; set; }

		public Surface Bar { get; set; }
		public Surface BarDisabled { get; set; }

		public Surface Thumb { get; set; }
		public Surface ThumbHover { get; set; }

		public Rectangle BarStretchRegion { get; set; }
		public Rectangle ThumbStretchRegion { get; set; }

		public bool DownInDecrease { get; set; }
		public bool DownInIncrease { get; set; }
		public bool DownInPageDecrease { get; set; }
		public bool DownInPageIncrease { get; set; }

		public bool MouseInDecrease { get; set; }
		public bool MouseInIncrease { get; set; }
		public bool MouseInPageDecrease { get; set; }
		public bool MouseInPageIncrease { get; set; }

		public double LastUpdate { get; set; }

		public bool DraggingThumb { get; set; }
		public Point ThumbGrabSpot { get; set; }

		public int FixedBarSize { get; set; }

		public MercuryScrollBar(MercuryScheme scheme, Widget widget)
			: base(scheme, widget)
		{
			FixedBarSize = 16;
		}

		public override void DrawWidget(Widget w)
		{
			if (w is VerticalScrollBar)
				DrawVerticalScrollBar((ScrollBar)w);
		}

		private void DrawVerticalScrollBar(ScrollBar scrollBar)
		{
			Point location = scrollBar.PointToScreen(Point.Empty);

			Decrease.Draw(location);
			Increase.Draw(location.X, location.Y + scrollBar.Height - Decrease.DisplayHeight);

			Point barLoc = location;
			barLoc.Y += Increase.DisplayHeight;
			Size sz = new Size(Increase.DisplayHeight,
				scrollBar.Height - Increase.DisplayHeight - Decrease.DisplayHeight);

			DrawStretchImage(barLoc, sz, Bar, BarStretchRegion);

			if (Mercury.DebugOutlines)
			{
				DrawRect(scrollBar, PageDecreaseRegion(scrollBar), Color.LightGreen);
				DrawRect(scrollBar, PageIncreaseRegion(scrollBar), Color.Green);
				DrawRect(scrollBar, ThumbRegion(scrollBar), DraggingThumb ? Color.LightBlue: Color.Blue);
			}
		}

		private static void DrawRect(ScrollBar scrollBar, Rectangle rect, Color clr)
		{
			rect.Location = scrollBar.PointToScreen(rect.Location);

			Display.DrawRect(rect, clr);
		}

		public override Size MinSize(Widget w)
		{
			return CalcMinScrollBarSize((ScrollBar)w);
		}
		public override Size MaxSize(Widget w)
		{
			return CalcMaxScrollBarSize((ScrollBar)w);
		}
		public Size CalcMinScrollBarSize(ScrollBar scrollBar)
		{
			if (scrollBar is VerticalScrollBar)
				return new Size(FixedBarSize, FixedBarSize * 3);
			else
				return new Size(FixedBarSize * 3, FixedBarSize);

			throw new ArgumentException();
		}
		public Size CalcMaxScrollBarSize(ScrollBar scrollBar)
		{
			if (scrollBar is VerticalScrollBar)
				return new Size(FixedBarSize, int.MaxValue);
			else if (scrollBar is HorizontalScrollBar)
				return new Size(int.MaxValue, FixedBarSize);

			throw new ArgumentException();
		}

		public override void MouseDownInWidget(Widget widget, Point clientLocation)
		{
			MouseDownInScrollBar((ScrollBar)widget, clientLocation);
		}
		public override void MouseMoveInWidget(Widget widget, Point clientLocation)
		{
			MouseMoveInScrollBar((ScrollBar)widget, clientLocation);
		}
		public override void MouseUpInWidget(Widget widget, Point clientLocation)
		{
			MouseUpInScrollBar((ScrollBar)widget, clientLocation);
		}

		public override void Update(Widget widget)
		{
			UpdateScrollBar((ScrollBar)widget);
		}
		void UpdateScrollBar(ScrollBar bar)
		{
			if (LastUpdate + 0.1 > Timing.TotalSeconds)
				return;

			UpdateMouseLocation(bar, bar.PointToClient(AgateLib.InputLib.Mouse.Position));

			if (DownInDecrease && MouseInDecrease)
				SafeMoveScrollBar(bar, -bar.SmallChange);
			if (DownInIncrease && MouseInIncrease)
				SafeMoveScrollBar(bar, bar.SmallChange);
			if (DownInPageDecrease && MouseInPageDecrease)
				SafeMoveScrollBar(bar, -bar.LargeChange);
			if (DownInPageIncrease && MouseInPageIncrease)
				SafeMoveScrollBar(bar, bar.LargeChange);

			LastUpdate = Timing.TotalSeconds;
		}
		private void UpdateMouseLocation(ScrollBar scrollBar, Point clientLocation)
		{

			MouseInDecrease = false;
			MouseInIncrease = false;
			MouseInPageDecrease = false;
			MouseInPageIncrease = false;

			if (DecreaseRegion(scrollBar).Contains(clientLocation))
				MouseInDecrease = true;
			else if (IncreaseRegion(scrollBar).Contains(clientLocation))
				MouseInIncrease = true;
			else if (PageDecreaseRegion(scrollBar).Contains(clientLocation))
				MouseInPageDecrease = true;
			else if (PageIncreaseRegion(scrollBar).Contains(clientLocation))
				MouseInPageIncrease = true;
		}

		private void SafeMoveScrollBar(ScrollBar scrollBar, int change)
		{
			int newValue = scrollBar.Value + change;
			newValue = SafeSetScrollBar(scrollBar, newValue);
		}
		private int SafeSetScrollBar(ScrollBar scrollBar, int newValue)
		{
			if (newValue < scrollBar.MinValue) newValue = scrollBar.MinValue;
			if (newValue > scrollBar.MaxValue) newValue = scrollBar.MaxValue;

			scrollBar.Value = newValue;

			return newValue;
		}

		public void MouseDownInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			Rectangle thumb = ThumbRegion(scrollBar);

			LastUpdate = Timing.TotalSeconds + 0.25;

			Scheme.RegisterUpdater(this, scrollBar);

			if (DecreaseRegion(scrollBar).Contains(clientLocation))
			{
				DownInDecrease = true;
				SafeMoveScrollBar(scrollBar, -scrollBar.SmallChange);
			}
			else if (IncreaseRegion(scrollBar).Contains(clientLocation)) 
			{
				DownInIncrease = true;
				SafeMoveScrollBar(scrollBar, scrollBar.SmallChange);
			}
			else if (thumb.Contains(clientLocation))
			{
				DraggingThumb = true;
				ThumbGrabSpot = new Point(clientLocation.X - thumb.X, clientLocation.Y - thumb.Y);
			}
			else if (PageDecreaseRegion(scrollBar).Contains(clientLocation))
			{
				DownInPageDecrease = true;
				SafeMoveScrollBar(scrollBar, -scrollBar.LargeChange);
			}
			else if (PageIncreaseRegion(scrollBar).Contains(clientLocation))
			{
				DownInPageIncrease = true;
				SafeMoveScrollBar(scrollBar, scrollBar.LargeChange);
			}

			MouseMoveInScrollBar(scrollBar, clientLocation);
		}
		public void MouseMoveInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			UpdateMouseLocation(scrollBar, clientLocation);

			if (DraggingThumb)
			{
				Point newThumbPos = new Point(clientLocation.X - ThumbGrabSpot.X,
					clientLocation.Y - ThumbGrabSpot.Y);

				int newThumbStart = scrollBar is VerticalScrollBar ? newThumbPos.Y : newThumbPos.X;
				int thumbSize = ThumbSize(scrollBar);
				int barSize = ScrollRegionSize(scrollBar) - thumbSize;

				newThumbStart -= FixedBarSize;

				int newValue = (int)
					((scrollBar.MaxValue - scrollBar.MinValue) * newThumbStart / (double)barSize + scrollBar.MinValue + 0.5);

				SafeSetScrollBar(scrollBar, newValue);

			}
		}
		public void MouseUpInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			DownInDecrease = false;
			DownInIncrease = false;
			DownInPageDecrease = false;
			DownInPageIncrease = false;
			DraggingThumb = false;

			Scheme.RemoveUpdater(this, scrollBar);
		}

		private Rectangle DecreaseRegion(ScrollBar scrollBar)
		{
			return new Rectangle(0, 0, FixedBarSize, FixedBarSize);
		}
		private Rectangle IncreaseRegion(ScrollBar scrollBar)
		{
			if (scrollBar is VerticalScrollBar)
				return new Rectangle(0, scrollBar.Height - FixedBarSize, FixedBarSize, FixedBarSize);
			else
				return new Rectangle(scrollBar.Width - FixedBarSize, 0, FixedBarSize, FixedBarSize);
		}
		private Rectangle ThumbRegion(ScrollBar scrollBar)
		{
			int size = ThumbSize(scrollBar);
			int start = ThumbStart(scrollBar, size);

			if (scrollBar is VerticalScrollBar)
			{
				return new Rectangle(0, start + FixedBarSize, FixedBarSize, size);
			}
			else
				return new Rectangle(start + FixedBarSize, 0, size, FixedBarSize);
		}
		private Rectangle PageDecreaseRegion(ScrollBar scrollBar)
		{
			return PageDecreaseRegion(scrollBar, ThumbRegion(scrollBar));
		}
		private Rectangle PageDecreaseRegion(ScrollBar scrollBar, Rectangle thumbRegion)
		{
			if (scrollBar is VerticalScrollBar)
			{
				return new Rectangle(0, FixedBarSize, FixedBarSize, thumbRegion.Top - FixedBarSize);
			}
			else
				return new Rectangle(FixedBarSize, 0, thumbRegion.Left - FixedBarSize, FixedBarSize);
		}
		private Rectangle PageIncreaseRegion(ScrollBar scrollBar)
		{
			return PageIncreaseRegion(scrollBar, ThumbRegion(scrollBar));
		}
		private Rectangle PageIncreaseRegion(ScrollBar scrollBar, Rectangle thumbRegion)
		{
			if (scrollBar is VerticalScrollBar)
				return Rectangle.FromLTRB(0, thumbRegion.Bottom, FixedBarSize, scrollBar.Height - FixedBarSize);
			else
				return Rectangle.FromLTRB(thumbRegion.Right, 0, scrollBar.Width - FixedBarSize, FixedBarSize);
		}

		private int ThumbStart(ScrollBar scrollBar)
		{
			int size = ThumbSize(scrollBar);
			return ThumbStart(scrollBar, size);
		}
		private int ThumbStart(ScrollBar scrollBar, int thumbSize)
		{
			int barSize = ScrollRegionSize(scrollBar);

			barSize -= thumbSize;

			return (int)(barSize * (scrollBar.Value - scrollBar.MinValue) /
								  (scrollBar.MaxValue - scrollBar.MinValue));
		}

		private int ThumbSize(ScrollBar scrollBar)
		{
			int size = ScrollRegionSize(scrollBar);

			int value = (int)(scrollBar.LargeChange * size / (double)(scrollBar.MaxValue - scrollBar.MinValue));

			if (value < 5)
				value = 5;

			return value;
		}

		private int ScrollRegionSize(ScrollBar scrollBar)
		{
			int size = (scrollBar is VerticalScrollBar) ? scrollBar.Height : scrollBar.Width;
			size -= FixedBarSize * 2;
			return size;
		}
		
	}
}
