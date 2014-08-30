using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStore.ApplicationModels
{
	public class WindowsStoreSerialModelParameters : SerialModelParameters
	{
		public WindowsStoreSerialModelParameters(IRenderTargetAdapter renderTarget)
		{
			this.RenderTarget = renderTarget;
		}

		public IRenderTargetAdapter RenderTarget { get; set; }
	}
}
