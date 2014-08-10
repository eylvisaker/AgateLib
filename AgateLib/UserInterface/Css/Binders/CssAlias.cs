using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css.Binders
{
	public class CssAliasAttribute : Attribute
	{
		public CssAliasAttribute(string alias)
		{
			this.Alias = alias;
		}

		public string Alias { get; set; }
	}
}
