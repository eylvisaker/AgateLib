using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public class Resolution
	{
		public Resolution(int width, int height, IRenderMode mode = null)
		{
			Size = new Size(width, height);
			RenderMode = mode;
		}

		public Size Size { get; set; }
		
		public int Width => Size.Width;
		public int Height => Size.Height;

		public IRenderMode RenderMode { get; set; }

		public override string ToString()
		{
			return $"{Size} - {RenderMode?.Name ?? "None"}";
		}
	}
}
