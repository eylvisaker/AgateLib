using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
	/// <summary>
	/// This class is here to spoof the Windows.Forms.Application static object.
	/// This allows it to look like the examples are calling windows forms application 
	/// methods without getting errors in the launcher because some of these methods
	/// must only be called once.
	/// </summary>
	class Application
	{
		internal static void EnableVisualStyles()
		{
		}

		internal static void SetCompatibleTextRenderingDefault(bool v)
		{
		}

		internal static void Run(System.Windows.Forms.Form form)
		{
			form.ShowDialog();
		}
	}
}
