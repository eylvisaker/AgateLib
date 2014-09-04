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
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib.ImplementationBase;
using SharpDX.DXGI;

namespace AgateLib.Platform.WindowsStore.DisplayImplementation
{
	public abstract class SDX_FrameBuffer: FrameBufferImpl 
	{
		bool mHasDepth;
		bool mHasStencil;

		public SDX_FrameBuffer(ICoordinateSystem coords) : base(coords)
		{
		}

		protected void SetHasDepthStencil(Format fmt)
		{
			mHasDepth = false;
			mHasStencil = false;

			switch (fmt)
			{
				case Format.D16_UNorm:
				case Format.D32_Float:
					mHasDepth = true;
					break;

				case Format.D24_UNorm_S8_UInt:
				case Format.D32_Float_S8X24_UInt:
					mHasDepth = true;
					mHasStencil = true;
					break;
			}
		}

		public override bool HasDepthBuffer
		{
			get { return mHasDepth; }
		}
		public override bool HasStencilBuffer
		{
			get { return mHasStencil; }
		}

		public virtual SharpDX.Direct3D11.RenderTargetView RenderTargetView { get; protected set; }
		public virtual SharpDX.Direct3D11.DepthStencilView DepthStencilView { get; protected set; }
	}
}
