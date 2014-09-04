using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AgateLib.Platform.WindowsStore
{
	public class SwapChainBackgroundPanelAdapter : RenderTargetAdapterBase
	{
		SwapChainBackgroundPanel mRenderTarget;

		public SwapChainBackgroundPanelAdapter(SwapChainBackgroundPanel renderTarget)
		{
			mRenderTarget = renderTarget;
			AttachEvents();
		}

		public SwapChainBackgroundPanel RenderTarget { get { return mRenderTarget; } }

		protected override Grid RenderTargetControl
		{
			get { return RenderTarget; }
		}

		public override void BindContextToRenderTarget(SharpDXContext context)
		{
			context.BindToControl(mRenderTarget);
		}
	}
}
