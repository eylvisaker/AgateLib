using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources.Managers
{
	public class PropertyMapValue<T>
	{
		public Action<T> Assign { get; set; }
	}
}
