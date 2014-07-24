using System;
namespace AgateLib.OpenGL
{
	interface IGL_Surface : IDisposable
	{
		AgateLib.Geometry.Size SurfaceSize { get; }

		bool FlipVertical { get; set; }

		int GLTextureID { get; }

		void Draw(AgateLib.DisplayLib.SurfaceState s);
	}
}
