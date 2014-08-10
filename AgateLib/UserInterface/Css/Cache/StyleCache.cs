using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Cache
{
	class StyleCache
	{
		public StyleCache()
		{
			CssClasses = new List<string>();
		}

		public List<string> CssClasses { get; private set; }
		public string ObjectType { get; set; }
		public string Id { get; set; }

		public CssPseudoClass PseudoClass { get; set; }
	}
}
