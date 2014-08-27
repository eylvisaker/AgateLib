using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStoreCommon
{
	public interface IRenderTargetAdapter
	{
		DisplayWindow Owner { get; set; }
		Size Size { get; }

		void AttachEvents();
		void DetachEvents();

		event EventHandler Disposed;
	}
}
