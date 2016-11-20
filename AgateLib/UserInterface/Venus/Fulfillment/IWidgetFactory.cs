using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public interface IWidgetFactory
	{
		/// <summary>
		/// Realizes each item and its children in the model.
		/// </summary>
		/// <param name="facetModel"></param>
		/// <param name="widgetCreated"></param>
		/// <returns>The list of root level widgets created.</returns>
		IEnumerable<Widget> RealizeFacetModel(FacetModel facetModel, Action<string, Widget> widgetCreated);
	}
}
