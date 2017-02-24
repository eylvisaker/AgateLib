using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace AgateLib.OpenGL
{
	public abstract class PrimitiveRenderer
	{
		protected OpenTK.Graphics.OpenGL.PrimitiveType PrimitiveTypeOf(LineType type)
		{
			switch (type)
			{
				case LineType.LineSegments:
					return PrimitiveType.Lines;
				case LineType.Path:
					return PrimitiveType.LineStrip;
				case LineType.Polygon:
					return PrimitiveType.LineLoop;

				default:
					throw new ArgumentException($"Invalid LineType {type}");
			}
		}

	}
}
