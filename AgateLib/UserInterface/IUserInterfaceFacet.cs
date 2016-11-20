using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface
{
	/// <summary>
	/// This class should be implemented by any class that needs user interface widgets.
	/// </summary>
	public interface IUserInterfaceFacet
	{
		/// <summary>
		/// This must be implemented to tell the resource manager what facet name should be used to 
		/// look up the configuration for the UI.
		/// </summary>
		string FacetName { get; }
		Gui InterfaceRoot { get; set; }
	}
}
