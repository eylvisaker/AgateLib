//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which gets or sets render states for the Display.
	/// </summary>
	public sealed class RenderStateAdapter
	{
		void CheckDisplayInitialized()
		{
			if (Display.Impl == null)
				throw new AgateException("Display has not been initialized.");
		}

		/// <summary>
		/// Gets or sets whether the vertical blank should be waited for on each frame.
		/// </summary>
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
		/// <summary>
		/// Gets or sets whether alpha blending is enabled.
		/// </summary>
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
		/// <summary>
		/// Gets or sets whether to test the z-buffer when writing pixels, if it is available.
		/// </summary>
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
		/// <summary>
		/// Gets or sets whether to write to the z-buffer when writing pixels, if it is available.
		/// </summary>
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
		/// <summary>
		/// Gets or sets whether to test the stencil buffer when writing pixels.
		/// </summary>
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
	}

	/// <summary>
	/// Enum describing boolean render state values.
	/// </summary>
	public enum RenderStateBool
	{
		/// <summary>
		/// VSync
		/// </summary>
		WaitForVerticalBlank,
		/// <summary>
		/// Alpha blending
		/// </summary>
		AlphaBlend,
		/// <summary>
		/// Z Buffer Testing
		/// </summary>
		ZBufferTest,
		/// <summary>
		/// Z buffer writing
		/// </summary>
		ZBufferWrite,
		/// <summary>
		/// Stencil buffer testing
		/// </summary>
		StencilBufferTest,
	}
}
