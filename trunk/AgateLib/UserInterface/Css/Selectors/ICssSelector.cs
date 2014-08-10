using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Selectors
{
	public interface ICssSelector
	{
		bool Matches(Widget control, string id, CssPseudoClass pseudoClass, IEnumerable<string> classes);
	}
}
