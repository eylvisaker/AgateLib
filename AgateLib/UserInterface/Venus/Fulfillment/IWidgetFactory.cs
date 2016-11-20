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
		void RealizeFacetModel(FacetModel facetModel, Action<string, Widget> widgetCreated);
	}
}
