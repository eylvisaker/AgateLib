using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssMedia : ICssCanSelect
	{
		public CssMedia()
		{
			RuleBlocks = new List<CssRuleBlock>();
		}

		public CssSelectorGroup Selector { get;set;}
		public List<CssRuleBlock> RuleBlocks { get; private set; }
	}
}
