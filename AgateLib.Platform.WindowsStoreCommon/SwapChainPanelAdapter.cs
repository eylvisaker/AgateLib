using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AgateLib.Platform.WindowsStore
{
	public class SwapChainPanelAdapter : RenderTargetAdapterBase
	{
		SwapChainPanel mRenderTarget;
		static int count = 0;
		int id = 0;

		public SwapChainPanelAdapter(SwapChainPanel renderTarget)
		{
			id = count;
			count++;

			mRenderTarget = renderTarget;
			AttachEvents();
		}


		public SwapChainPanel RenderTarget { get { return mRenderTarget; } }

		protected override Grid RenderTargetControl
		{
			get { return RenderTarget; }
		}




		public override void BindContextToRenderTarget(SharpDX.SimpleInitializer.SharpDXContext context)
		{
			context.BindToControl(mRenderTarget);
		}
	}
}
