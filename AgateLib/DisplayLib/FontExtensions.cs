using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public static class FontExtensions
	{
		public static ContentLayout LayoutText(this IFont font, string text, int? maxWidth)
		{
			var result = new ContentLayout(font, maxWidth);

			result.Append(text);

			return result;
		}
	}
}
