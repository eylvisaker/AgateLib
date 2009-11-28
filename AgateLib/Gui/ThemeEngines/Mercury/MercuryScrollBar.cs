using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryScrollBar : MercurySchemeCommon
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


		public void DrawScrollBar(ScrollBar scrollBar)
		{
			if (scrollBar is VerticalScrollBar)
				DrawVerticalScrollBar(scrollBar);
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
			else
				return new Size(int.MaxValue, FixedBarSize);

			throw new ArgumentException();
		}


		public void MouseDownInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			throw new NotImplementedException();
		}
		public void MouseMoveInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			throw new NotImplementedException();
		}
		public void MouseUpInScrollBar(ScrollBar scrollBar, Point clientLocation)
		{
			throw new NotImplementedException();
		}
	}
}
