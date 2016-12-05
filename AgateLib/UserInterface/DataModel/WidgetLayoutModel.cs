using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetLayoutModel
	{
		/// <summary>
		/// The direction to arrange child elements in.
		/// </summary>
		public LayoutDirection? Direction { get; set; }

		/// <summary>
		/// Whether to wrap child elements within the container if they overflow the boundaries.
		/// </summary>
		public LayoutWrap? Wrap { get; set; }
	}
}
