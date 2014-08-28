using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Fakes
{
	public class FakeFontSurface : FontSurfaceImpl
	{
		public int Height { get; set; }

		public override int FontHeight
		{
			get { return Height; }
		}

		public override void DrawText(FontState state)
		{
		}

		public override void Dispose()
		{
		}

		public override Size MeasureString(FontState state, string text)
		{
			return new Size(Height * text.Length, Height);
		}
	}

}
