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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
	/// <summary>
	/// Implements a FontSurface
	/// </summary>
	public abstract class FontSurfaceImpl : IDisposable
	{
		private string mFontName = "Unknown";

		/// <summary>
		/// Returns the name/size of the font.
		/// </summary>
		public string FontName
		{
			get { return mFontName; }
			protected internal set { mFontName = value; }
		}

		/// <summary>
		/// Gets the height of a single line of text.
		/// </summary>
		public abstract int FontHeight { get; }

		/// <summary>
		/// Draws text to the screen.
		/// </summary>
		/// <param name="state"></param>
		public abstract void DrawText(FontState state);

		/// <summary>
		/// Disposes of unmanaged resources.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Measures the size of the given string.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public abstract Size MeasureString(FontState state, string text);
	}

}
