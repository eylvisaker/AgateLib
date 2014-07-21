using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using System;
namespace AgateOTK
{
	public interface IGL_Display
	{
		void Dispose();

		GLDrawBuffer DrawBuffer { get; }
	
		Surface WhiteSurface { get;}

		AgateShader Shader { get; set; }

		GLDrawBuffer CreateDrawBuffer();
	}
}
