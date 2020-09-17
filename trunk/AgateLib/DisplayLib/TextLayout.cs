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
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which contains a list of LayoutItem objects which
	/// is used to layout text and surfaces when drawn.
	/// </summary>
	public class TextLayout : List<LayoutItem>
	{
		/// <summary>
		/// Draws all the items in the list.
		/// </summary>
		public void DrawAll()
		{
			ForEach(x => x.Draw());
		}

		/// <summary>
		/// Translates all the objects in the TextLayout by the
		/// specified point.
		/// </summary>
		/// <param name="dist"></param>
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

	/// <summary>
	/// Base class for a LayoutItem stored in a TextLayout object.
	/// </summary>
	public abstract class LayoutItem
	{
		/// <summary>
		/// Draws the object.
		/// </summary>
		public abstract void Draw();
		/// <summary>
		/// Location of the object.
		/// </summary>
		public abstract PointF Location { get; set; }

		/// <summary>
		/// Gets or sets the X position of the LayoutItem.
		/// </summary>
		public float X
		{
			get { return Location.X; }
			set { Location = new PointF(value, Location.Y); }
		}
		/// <summary>
		/// Gets or sets the Y position of the LayoutItem.
		/// </summary>
		public float Y
		{
			get { return Location.Y; }
			set { Location = new PointF(Location.X, value); }
		}

		/// <summary>
		/// Gets the index of the line where this item appears.
		/// </summary>
		public int LineIndex { get; set; }
	}

	/// <summary>
	/// Class which is used to indicate text layout.
	/// </summary>
	public class LayoutText : LayoutItem
	{
		/// <summary>
		/// Gets or sets the FontSurface object used to render the text.
		/// </summary>
		public FontSurface Font { get; set; }
		/// <summary>
		/// Gets or sets the FontState object used to render the text.
		/// </summary>
		public FontState State { get; set; }
		/// <summary>
		/// Gets or sets the text displayed.
		/// </summary>
		public string Text
		{
			get { return State.Text; }
			set { State.Text = value; }
		}
		/// <summary>
		/// Gets or sets the location of the text.
		/// </summary>
		public override PointF Location
		{
			get { return State.Location; }
			set { State.Location = value; }
		}
		/// <summary>
		/// Draws the text.
		/// </summary>
		public override void Draw()
		{
			Font.DrawText(State);
		}

		/// <summary>
		/// Returns a text representation of the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(
				"LayoutText: {0}; {1}", Location, Text);
		}
	}
	/// <summary>
	/// Layout object for a surface.
	/// </summary>
	public class LayoutSurface : LayoutItem
	{
		/// <summary>
		/// The surface to draw.
		/// </summary>
		public ISurface Surface { get; set; }
		/// <summary>
		/// The state information used to draw the surface.
		/// </summary>
		public SurfaceState State { get; set; }
		/// <summary>
		/// The location to draw the surface.
		/// </summary>
		public override PointF Location { get; set; }

		/// <summary>
		/// Indicates whether or not to draw a rectangle around the surface.
		/// Useful for debugging.
		/// </summary>
		public static bool DebugRects;

		/// <summary>
		/// Draws the surface.
		/// </summary>
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

	/// <summary>
	/// Base class for classes used to alter the font state.
	/// </summary>
	public abstract class AlterFont
	{
		/// <summary>
		/// The actual function doing the modification.
		/// </summary>
		/// <param name="state"></param>
		protected internal abstract void ModifyState(FontState state);

		/// <summary>
		/// A class which modifies the text color.
		/// </summary>
		class AlterTextColor : AlterFont 
		{
			Color clr;

			/// <summary>
			/// Constructs an AlterTextColor object which changes
			/// the text color to the specified value.
			/// </summary>
			/// <param name="newColor"></param>
			public AlterTextColor(Color newColor)
			{
				clr = newColor;
			}

			/// <summary>
			/// Changes the color.
			/// </summary>
			/// <param name="state"></param>
			protected internal override void ModifyState(FontState state)
			{
				state.Color = clr;
			}
		}
		/// <summary>
		/// A class which modifies the text scale.
		/// </summary>
		class AlterTextScale : AlterFont
		{
			double width, height;

			/// <summary>
			/// Constructs an AlterTextScale object with the specified scale values.
			/// </summary>
			/// <param name="width"></param>
			/// <param name="height"></param>
			public AlterTextScale(double width, double height)
			{
				this.width = width;
				this.height = height;
			}
			/// <summary>
			/// Modifies the scale.
			/// </summary>
			/// <param name="state"></param>
			protected internal override void ModifyState(FontState state)
			{
				state.ScaleWidth = width;
				state.ScaleHeight = height;
			}
		}

		/// <summary>
		/// Creates and returns a new AlterTextColor object.
		/// </summary>
		/// <param name="newColor"></param>
		/// <returns></returns>
		public static AlterFont Color(Color newColor)
		{
			return new AlterTextColor(newColor);
		}
		/// <summary>
		/// Creates and returns a new AlterTextScale object.
		/// </summary>
		/// <param name="scaleWidth"></param>
		/// <param name="scaleHeight"></param>
		/// <returns></returns>
		public static AlterFont Scale(double scaleWidth, double scaleHeight)
		{
			return new AlterTextScale(scaleWidth, scaleHeight);
		}
	}
}
