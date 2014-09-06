using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib
{
	public interface IDrawable
	{
		void Draw(Point point);

		Size DisplaySize { get; set; }
	}
}
