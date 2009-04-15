using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib.Cache
{
	public abstract class FontStateCache
	{
		protected internal abstract FontStateCache Clone();

		protected internal virtual void OnTextChanged(FontState fontState)
		{
		}
		protected internal virtual void OnLocationChanged(FontState fontState)
		{
		}
		protected internal virtual void OnDisplayAlignmentChanged(FontState fontState)
		{
		}
		protected internal virtual void OnColorChanged(FontState fontState)
		{
		}
		protected internal virtual void OnScaleChanged(FontState fontState)
		{
		}
	}
}
