using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public class TextLayout : List<LayoutItem>
	{
		public void DrawAll()
		{
			foreach (LayoutItem item in this)
			{
				item.Draw();
			}
		}

		public void Translate(Point dist)
		{
			foreach (LayoutItem item in this)
			{
				item.Location = new PointF(
					item.Location.X + dist.X,
					item.Location.Y + dist.Y);
			}
		}
	}

	public abstract class LayoutItem
	{
		PointF mLocation;

		public abstract void Draw();
		public virtual PointF Location
		{
			get { return mLocation; }
			set { mLocation = value; }
		}
		public float X
		{
			get { return mLocation.X; }
			set { mLocation.X = value; }
		}
		public float Y
		{
			get { return mLocation.Y; }
			set { mLocation.Y = value; }
		}
		public int LineIndex { get; set; }
	}

	public class LayoutText : LayoutItem
	{
		public FontSurface Font { get; set; }
		public FontState State { get; set; }
		public string Text
		{
			get { return State.Text; }
			set { State.Text = value; }
		}
		public override PointF Location
		{
			get { return State.Location; }
			set { State.Location = value; }
		}
		public override void Draw()
		{
			Font.DrawText(State);
		}
	}
	public class LayoutSurface : LayoutItem
	{
		public ISurface Surface { get; set; }
		public SurfaceState State { get; set; }

		public static bool DebugRects;

		public override void Draw()
		{
			this.Surface.Draw(State);
			
			if (DebugRects)
				Display.DrawRect(new Rectangle(Point.Round(Location), Surface.DisplaySize), Color.Blue);
		}
	}
}
