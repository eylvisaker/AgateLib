using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Css.Documents
{
	public class CssText
	{
		[CssAlias("text-align")]
		public TextAlign Align { get; set; }
	}
}
