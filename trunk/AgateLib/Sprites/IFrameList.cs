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
using System.Collections;
using System.Text;
using AgateLib.DisplayLib;

namespace AgateLib.Sprites
{
	/// <summary>
	/// Iterface implemented by a list of sprite frames.
	/// This provides a read-only view into the frames in a sprite.
	/// </summary>
	public interface IFrameList : IEnumerable
	{
		/// <summary>
		/// Returns the number of frames in the list.
		/// </summary>
		int Count { get; }
		/// <summary>
		/// Gets a frame by index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ISpriteFrame this[int index] { get; }

		/// <summary>
		/// Searches for a particular frame.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		int IndexOf(ISpriteFrame item);

		/// <summary>
		/// Returns a bool indicating whether the specified frame is
		/// contained in the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		bool Contains(ISpriteFrame item);

		/// <summary>
		/// Copies the list of frame to a target array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void CopyTo(ISpriteFrame[] array, int arrayIndex);
	}
}
