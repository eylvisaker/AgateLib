using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry.VertexTypes;

namespace AgateOTK.GL3.Shaders
{
	interface IGL3Shader
	{
		void SetVertexAttributes(VertexLayout layout);

		void SetTexture(int p);
	}
}
