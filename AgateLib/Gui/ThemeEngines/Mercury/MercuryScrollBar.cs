using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	using Cache;

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

		public int FixedBarSize { get; set; }

		public MercuryScrollBar(MercuryScheme scheme)
			: base(scheme)
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

		ScrollBarCache GetCache(ScrollBar bar)
		{
			if (bar.Cache == null)
				bar.Cache = new ScrollBarCache();

			return (ScrollBarCache)bar.Cache;
		}
		public void MouseDownInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			var cache = GetCache(scrollBar);
			Rectangle thumb = ThumbRegion(scrollBar);

			cache.LastUpdate = Timing.TotalSeconds + 0.25;

			if (DecreaseRegion(scrollBar).Contains(clientLocation))
			{
				cache.DownInDecrease = true;
				scrollBar.Value -= scrollBar.SmallChange;
			}
			else if (IncreaseRegion(scrollBar).Contains(clientLocation)) 
			{
				cache.DownInIncrease = true;
				scrollBar.Value += scrollBar.SmallChange;
			}
			else if (thumb.Contains(clientLocation))
			{
				cache.DragThumb = true;
				cache.ThumbGrabSpot = new Point(clientLocation.X - thumb.X, clientLocation.Y - thumb.Y);
			}
			else if (PageDecreaseRegion(scrollBar).Contains(clientLocation))
			{
				cache.DownInPageDecrease = true;
				scrollBar.Value -= scrollBar.LargeChange;
			}
			else if (PageIncreaseRegion(scrollBar).Contains(clientLocation))
			{
				cache.DownInPageIncrease = true;
				scrollBar.Value += scrollBar.LargeChange;
			}
		}
		public void MouseMoveInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
		}
		public void MouseUpInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			var cache = GetCache(scrollBar);
			
			cache.DownInDecrease = false;
			cache.DownInIncrease = false;
			cache.DownInPageDecrease = false;
			cache.DownInPageIncrease = false;
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
			return new Rectangle(0, 0, 0, 0);
		}
		private Rectangle PageDecreaseRegion(ScrollBar scrollBar)
		{
			throw new NotImplementedException();
		}
		private Rectangle PageIncreaseRegion(ScrollBar scrollBar)
		{
			throw new NotImplementedException();
		}
	}
}
