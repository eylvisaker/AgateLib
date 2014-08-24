using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AgateLib.Platform.WindowsPhone.ApplicationModels
{
	public class SceneModelParameters : ModelParameters
	{
		public SceneModelParameters(DrawingSurfaceBackgroundGrid renderTarget)
		{
			this.RenderTarget = renderTarget;
		}

		public DrawingSurfaceBackgroundGrid RenderTarget { get; set; }
	}
}
