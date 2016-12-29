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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.ApplicationModels;
using AgateLib.Geometry;
using System;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// This interface is implemented by the FrameBuffer class. Its main purpose is
	/// to allow you to create a fake object implementing the interface in order to 
	/// write unit tests for drawing code.
	/// </summary>
	public interface IFrameBuffer
	{
		/// <summary>
		/// Height of the IFrameBuffer object.
		/// </summary>
		int Height { get; }
		/// <summary>
		/// Width of the IFrameBuffer object.
		/// </summary>
		int Width { get; }
		/// <summary>
		/// Size of the IFrameBuffer object. Should equal new Size(Width, Height).
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// Gets or sets the coordinate system for the render target.
		/// </summary>
		ICoordinateSystem CoordinateSystem { get; set; }
	}
}
