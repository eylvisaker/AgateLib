using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public interface ICssCanSelect
	{
		CssSelectorGroup Selector { get; set; }
	}
}
