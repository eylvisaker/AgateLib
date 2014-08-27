using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AgateLib.Platform.WindowsStoreCommon
{
	public class SwapChainBackgroundPanelAdapter : IRenderTargetAdapter
	{
		SwapChainBackgroundPanel mRenderTarget;

		public SwapChainBackgroundPanelAdapter(SwapChainBackgroundPanel renderTarget)
		{
			mRenderTarget = renderTarget;
		}

	}
}
