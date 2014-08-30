using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStore.ApplicationModels
{
	public class WindowsStoreSceneModelParameters : SceneModelParameters
	{
		public WindowsStoreSceneModelParameters(IRenderTargetAdapter renderTarget)
		{
			this.RenderTarget = renderTarget;
		}

		public IRenderTargetAdapter RenderTarget { get; set; }
	}
}
