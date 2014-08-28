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
using SlimDX.Direct3D9;

namespace AgateSDX
{
	public abstract class SDX_FrameBuffer: FrameBufferImpl 
	{
		bool mHasDepth;
		bool mHasStencil;

		protected void SetHasDepthStencil(Format fmt)
		{
			switch (fmt)
			{
				case Format.D16:
				case Format.D16Lockable:
				case Format.D24X8:
				case Format.D32:
				case Format.D32Lockable:
				case Format.D32SingleLockable:
					mHasDepth = true;
					break;

				case Format.D15S1:
				case Format.D24S8:
				case Format.D24SingleS8:
				case Format.D24X4S4:
					mHasDepth = true;
					mHasStencil = true;
					break;

				case Format.S8Lockable:
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
	}
}
