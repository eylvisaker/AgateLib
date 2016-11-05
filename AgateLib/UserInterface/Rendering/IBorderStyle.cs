using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Rendering
{
	public interface IBorderStyle
	{

		string Image { get; }

		Rectangle ImageSlice { get; }

		IBorderSideStyle Top { get; }
		IBorderSideStyle Left { get; }
		IBorderSideStyle Bottom { get; }
		IBorderSideStyle Right { get; }
	}

	public interface IBorderSideStyle
	{
		Color Color { get; }
		int Width { get; }
	}
}
