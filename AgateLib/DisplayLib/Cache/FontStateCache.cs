//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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

		protected internal virtual void OnSizeChanged(FontState fontState)
		{
		}

		protected internal void OnStyleChanged(FontState fontState)
		{
		}

	}
}
