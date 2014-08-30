using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	class ContextInfo : IDisposable
	{
		public OpenGL.ContextFB FrameBuffer { get; set; }

		public OpenTK.Graphics.GraphicsContext Context { get; set; }

		public void Dispose()
		{
			FrameBuffer.Dispose();
			Context.Dispose();
		}
	}
}
