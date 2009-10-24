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
		public abstract void Draw();
		public abstract PointF Location { get; set; }
		public float X
		{
			get { return Location.X; }
			set { Location = new PointF(value, Location.Y); }
		}
		public float Y
		{
			get { return Location.Y; }
			set { Location = new PointF(Location.X, value); }
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

		public override string ToString()
		{
			return string.Format(
				"LayoutText: {0}; {1}", Location, Text);
		}
	}
	public class LayoutSurface : LayoutItem
	{
		public ISurface Surface { get; set; }
		public SurfaceState State { get; set; }
		public override PointF Location { get; set; }

		public static bool DebugRects;

		public override void Draw()
		{
			if (State == null)
				State = Surface.State.Clone();

			State.DrawInstances.SetCount(1);
			State.DrawInstances[0] = new SurfaceDrawInstance(Location);

			this.Surface.Draw(State);
			
			if (DebugRects)
				Display.DrawRect(new Rectangle(Point.Round(Location), Surface.DisplaySize), Color.Blue);
		}
	}

	public abstract class AlterFont
	{
		protected internal abstract void ModifyState(FontState state);

		class AlterTextColor : AlterFont 
		{
			Color clr;
			public AlterTextColor(Color newColor)
			{
				clr = newColor;
			}

			protected internal override void ModifyState(FontState state)
			{
				state.Color = clr;
			}
		}
		class AlterTextScale : AlterFont
		{
			double width, height;

			public AlterTextScale(double width, double height)
			{
				this.width = width;
				this.height = height;
			}
			protected internal override void ModifyState(FontState state)
			{
				state.ScaleWidth = width;
				state.ScaleHeight = height;
			}
		}

		public static AlterFont Color(Color newColor)
		{
			return new AlterTextColor(newColor);
		}
		public static AlterFont Scale(double scaleWidth, double scaleHeight)
		{
			return new AlterTextScale(scaleWidth, scaleHeight);
		}
	}

}
