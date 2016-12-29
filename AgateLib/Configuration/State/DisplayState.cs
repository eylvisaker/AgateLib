using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DefaultAssets;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace AgateLib.Configuration.State
{
	class DisplayState
	{
		internal EventHandler PackAllSurfacesEvent;
		internal Action DisposeDisplay;

		internal DisplayImpl Impl;
		internal DisplayWindow CurrentWindow;
		internal SurfacePacker SurfacePacker;
		internal Rectangle CurrentClipRect;
		internal Stack<Rectangle> ClipRects = new Stack<Rectangle>();
		internal RenderStateAdapter RenderState = new RenderStateAdapter();
		internal DisplayCapsInfo CapsInfo = new DisplayCapsInfo();
		internal DefaultResources DefaultResources;
	}
}
