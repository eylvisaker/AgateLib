using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css.Parser
{
	public class CssPromotePropertiesAttribute : Attribute
	{
		public CssPromotePropertiesAttribute(string prefix = "")
		{
			Prefix = prefix;
		}

		public string Prefix { get; private set; }
	}
}
