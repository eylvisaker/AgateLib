using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Selectors
{
	public interface ICssMediaSelector
	{
		bool Matches(string type);
	}
}
