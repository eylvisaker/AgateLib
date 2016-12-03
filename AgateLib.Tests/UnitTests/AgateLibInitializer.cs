using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform.Test;

namespace AgateLib.UnitTests
{
	public class AgateLibInitializer
	{
		public FakeAgateFactory InitializeAgateLib()
		{
			var result = new FakeAgateFactory();

			Core.State = new StaticState.InternalState();
			Core.Initialize(result);

			return result;
		}

		public void InitializeDisplayWindow(int width, int height)
		{
			InitializeDisplayWindow(new Size(width, height));
		}

		public void InitializeDisplayWindow(Size size)
		{
			DisplayWindow.CreateWindowed("AgateLib", size);
		}
	}
}
