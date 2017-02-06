using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public class Resolution : IResolution
	{
		public Resolution(Size size, IRenderMode mode = null)
		{
			Size = size;
			RenderMode = mode ?? DisplayLib.RenderMode.RetainAspectRatio;
		}

		public Resolution(int width, int height, IRenderMode mode = null)
		{
			Size = new Size(width, height);
			RenderMode = mode;
		}

		public IResolution Clone(Size? newSize = null)
		{
			return new Resolution(newSize ?? Size, RenderMode);
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
			return $"{Size.Width} x {Size.Height} - {RenderMode?.ToString() ?? "None"}";
		}

	}

	public interface IResolution
	{
		Size Size { get; }

		int Width { get; }

		int Height { get; }

		IRenderMode RenderMode { get; }

		IResolution Clone(Size? newSize = null);
	}
}
