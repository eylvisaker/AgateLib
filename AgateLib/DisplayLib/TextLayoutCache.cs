//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which contains a list of LayoutItem objects which
	/// is used to layout text and surfaces when drawn.
	/// </summary>
	public class TextLayoutCache : List<LayoutCacheItem>
	{
		/// <summary>
		/// Draws all the items in the list.
		/// </summary>
		public void DrawAll()
		{
			foreach (var x in this)
				x.Draw();
		}

		/// <summary>
		/// Translates all the objects in the TextLayout by the
		/// specified point.
		/// </summary>
		/// <param name="distance"></param>
		public void Translate(Vector2f distance)
		{
			foreach (LayoutCacheItem item in this)
			{
				item.Location += distance;
			}
		}
	}

	/// <summary>
	/// Base class for a LayoutItem stored in a TextLayout object.
	/// </summary>
	public abstract class LayoutCacheItem
	{
		/// <summary>
		/// Draws the object.
		/// </summary>
		public abstract void Draw();
		/// <summary>
		/// Location of the object.
		/// </summary>
		public abstract Vector2f Location { get; set; }

		/// <summary>
		/// Gets or sets the X position of the LayoutItem.
		/// </summary>
		public float X
		{
			get { return Location.X; }
			set { Location = new Vector2f(value, Location.Y); }
		}
		/// <summary>
		/// Gets or sets the Y position of the LayoutItem.
		/// </summary>
		public float Y
		{
			get { return Location.Y; }
			set { Location = new Vector2f(Location.X, value); }
		}

		/// <summary>
		/// Gets the index of the line where this item appears.
		/// </summary>
		public int LineIndex { get; set; }
	}

	/// <summary>
	/// Class which is used to indicate text layout.
	/// </summary>
	public class LayoutText : LayoutCacheItem
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
		public override Vector2f Location
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
	public class LayoutCacheSurface : LayoutCacheItem
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
		public override Vector2f Location { get; set; }

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
				Display.Primitives.DrawRect(Color.Blue, new Rectangle(Point.Round(Location), Surface.DisplaySize));
		}
	}

	/// <summary>
	/// Base class for classes used to alter the font state.
	/// </summary>
	public abstract class LayoutCacheAlterFont
	{
		/// <summary>
		/// The actual function doing the modification.
		/// </summary>
		/// <param name="state"></param>
		protected internal abstract void ModifyState(FontState state);

		/// <summary>
		/// A class which modifies the text color.
		/// </summary>
		class LayoutCacheAlterTextColor : LayoutCacheAlterFont
		{
			Color clr;

			/// <summary>
			/// Constructs an AlterTextColor object which changes
			/// the text color to the specified value.
			/// </summary>
			/// <param name="newColor"></param>
			public LayoutCacheAlterTextColor(Color newColor)
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
		class LayoutCacheAlterTextScale : LayoutCacheAlterFont
		{
			double width, height;

			/// <summary>
			/// Constructs an AlterTextScale object with the specified scale values.
			/// </summary>
			/// <param name="width"></param>
			/// <param name="height"></param>
			public LayoutCacheAlterTextScale(double width, double height)
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
		public static LayoutCacheAlterFont Color(Color newColor)
		{
			return new LayoutCacheAlterTextColor(newColor);
		}
		/// <summary>
		/// Creates and returns a new AlterTextScale object.
		/// </summary>
		/// <param name="scaleWidth"></param>
		/// <param name="scaleHeight"></param>
		/// <returns></returns>
		public static LayoutCacheAlterFont Scale(double scaleWidth, double scaleHeight)
		{
			return new LayoutCacheAlterTextScale(scaleWidth, scaleHeight);
		}
	}
}
