using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	class DisplayControlContext : ApplicationContext
	{
		private List<Form> forms = new List<Form>();

		public DisplayControlContext()
		{
		}

		public void AddForm(Form frm)
		{
			forms.Add(frm);
		}

		public void RunMessageLoop()
		{
			Application.Run(this);
		}
	}
}
