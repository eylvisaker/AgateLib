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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Sprites
{
	/// <summary>
	/// Basic interface implemented by a particular frame in a sprite.
	/// </summary>
	public interface ISpriteFrame
	{
		/// <summary>
		/// Draws the frame.
		/// </summary>
		/// <param name="dest_x"></param>
		/// <param name="dest_y"></param>
		/// <param name="rotationCenterX"></param>
		/// <param name="rotationCenterY"></param>
		void Draw(float dest_x, float dest_y, float rotationCenterX, float rotationCenterY);

		/// <summary>
		/// Gets the surface object the frame is drawn from
		/// </summary>
		Surface Surface { get; }

		/// <summary>
		/// Gets the source rectangle on the surface the frame is drawn from.
		/// </summary>
		Rectangle SourceRect { get; }
	}
}
