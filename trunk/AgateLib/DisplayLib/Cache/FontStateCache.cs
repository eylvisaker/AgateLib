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
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib.Cache
{
	/// <summary>
	/// Base class for cache objects used for FontState.
	/// </summary>
	public abstract class FontStateCache
	{
		/// <summary>
		/// Performs a deep clone of the FontStateCache-derived object.
		/// </summary>
		/// <returns></returns>
		protected internal abstract FontStateCache Clone();
		
		/// <summary>
		/// Function called when the text is changed.
		/// </summary>
		/// <param name="fontState"></param>
		protected internal virtual void OnTextChanged(FontState fontState)
		{
		}
		/// <summary>
		/// Function called when the location of text is changed.
		/// </summary>
		/// <param name="fontState"></param>
		protected internal virtual void OnLocationChanged(FontState fontState)
		{
		}
		/// <summary>
		/// Function called when the display alignment of the text is changed.
		/// </summary>
		/// <param name="fontState"></param>
		protected internal virtual void OnDisplayAlignmentChanged(FontState fontState)
		{
		}
		/// <summary>
		/// Function called when the color of the text is changed.
		/// </summary>
		/// <param name="fontState"></param>
		protected internal virtual void OnColorChanged(FontState fontState)
		{
		}
		/// <summary>
		/// Function called when the scale of the text is changed.
		/// </summary>
		/// <param name="fontState"></param>
		protected internal virtual void OnScaleChanged(FontState fontState)
		{
		}
	}
}
