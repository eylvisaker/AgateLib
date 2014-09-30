using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib
{
	public interface IDrawable
	{		
		/// <summary>
		/// Draws the surface at the specified point.
		/// </summary>
		/// <param name="destPt"></param>
		void Draw(Point destPt);
		/// <summary>
		/// Gets or sets the display size of the surface, in pixels.
		/// </summary>
		Size DisplaySize { get; set; }
	}
}
