using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsStoreCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.Windows.ApplicationModels
{
	public class SceneModelParameters : ModelParameters
	{
		public SceneModelParameters(IRenderTargetAdapter renderTarget)
		{
			this.RenderTarget = renderTarget;
		}

		public IRenderTargetAdapter RenderTarget { get; set; }
	}
}
