using AgateLib.DisplayLib;
using AgateLib.Geometry;
using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStore
{
	public interface IRenderTargetAdapter
	{
		DisplayWindow Owner { get; set; }
		Size Size { get; }

		void AttachEvents();
		void DetachEvents();

		event EventHandler Disposed;

		void BindContextToRenderTarget(SharpDXContext context);
	}
}
