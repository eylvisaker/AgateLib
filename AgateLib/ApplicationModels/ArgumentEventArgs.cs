using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public class ArgumentEventArgs : EventArgs
	{
		private string arg;
		private IList<string> parm;

		public ArgumentEventArgs(string arg, IList<string> parm)
		{
			// TODO: Complete member initialization
			this.arg = arg;
			this.parm = parm;
		}

		public string Argument { get { return arg; } }
		public IEnumerable<string> Parameters { get { return parm; } }
		public bool Handled { get; set; }
	}
}
