using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib
{
	public sealed class RenderStateAdapter
	{
		void CheckDisplayInitialized()
		{
			if (Display.Impl == null)
				throw new AgateException("Display has not been initialized.");
		}

		public bool WaitForVerticalBlank
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.WaitForVerticalBlank);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.WaitForVerticalBlank, value);
			}
		}

		public bool AlphaBlend
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.AlphaBlend);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.AlphaBlend, value);
			}
		}

		public bool ZBufferTest
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.ZBufferTest);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.ZBufferTest, value);
			}
		}
		public bool ZBufferWrite
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.ZBufferWrite);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.ZBufferWrite, value);
			}
		}
		public bool StencilBufferTest
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.StencilBufferTest);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.StencilBufferTest, value);
			}
		}
		public bool StencilBufferWrite
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.StencilBufferWrite);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.StencilBufferWrite, value);
			}
		}
	}

	public enum RenderStateBool
	{
		WaitForVerticalBlank,

		AlphaBlend,

		ZBufferTest,
		ZBufferWrite,

		StencilBufferTest,
		StencilBufferWrite,
	}
}
