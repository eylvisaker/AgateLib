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
		public Resolution(Size size, IRenderMode mode = null)
		{
			Size = size;
			RenderMode = mode;
		}

		public Resolution(int width, int height, IRenderMode mode = null)
		{
			Size = new Size(width, height);
			RenderMode = mode;
		}

		public Resolution Clone()
		{
			return new Resolution(Size, RenderMode);
		}

		public Size Size
		{
			get { return new Size(Width, Height); }
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		public int Width { get; set; }
		public int Height { get; set; }

		public IRenderMode RenderMode { get; set; }

		public override string ToString()
		{
			return $"{Size.Width} x {Size.Height} - {RenderMode?.Name ?? "None"}";
		}

	}
}
