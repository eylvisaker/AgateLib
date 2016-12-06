using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Venus.Fulfillment;
using AgateLib.UserInterface.Venus.Layout;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources.Managers
{
	public class UserInterfaceResourceManager : IUserInterfaceResourceManager
	{
		private IWidgetFactory widgetFactory;
		private IFacetInspector facetInspector;

		private ResourceDataModel data;

		public UserInterfaceResourceManager(ResourceDataModel data)
		{
			this.data = data;

			Initialize();
		}

		private void Initialize()
		{
			facetInspector = new FacetInspector();
			widgetFactory = new WidgetFactory(AssemblyDiscoveryWidgetActivator.ForAgateLib());
		}

		public void InitializeFacet(IUserInterfaceFacet facet)
		{
			try
			{
				var adapter = new VenusWidgetAdapter();
				var layoutEngine = new VenusLayoutEngine(adapter);
				var guiRenderer = new AgateUserInterfaceRenderer(adapter);

				adapter.FacetData = data.Facets;
				adapter.ThemeData = data.Themes;

				Condition.RequireArgumentNotNull(facet.FacetName, nameof(facet.FacetName), "The value of the facet's FacetName property must not be null.");

				var facetModel = data.Facets[facet.FacetName];

				var gui = new Gui(guiRenderer, layoutEngine);

				RealizeFacetModel(facet, gui, facetModel);

				facet.InterfaceRoot = gui;
				gui.FacetName = facet.FacetName;

				adapter.InitializeStyleData(gui);
			}
			catch (Exception e) when (!(e is AgateUserInterfaceInitializationException))
			{
				throw new AgateUserInterfaceInitializationException("Failed to initialize the facet.", e);
			}
		}

		private void RealizeFacetModel(IUserInterfaceFacet facet, Gui gui, FacetModel facetModel)
		{
			var propertyMap = facetInspector.BuildPropertyMap(facet);
			List<FacetWidgetPropertyMapValue> assigned = new List<FacetWidgetPropertyMapValue>();

			var widgets = widgetFactory.RealizeFacetModel(facetModel, (name, widget) =>
			{
				if (propertyMap.ContainsKey(name) == false)
					return;

				var mapValue = propertyMap[name];

				if (assigned.Contains(mapValue))
					throw new AgateUserInterfaceInitializationException($"Widget {name} has multiple entries in facet data.");

				mapValue.Assign(widget);
				assigned.Add(mapValue);
			});

			var missing = propertyMap.Where(x => assigned.Contains(x.Value) == false);

			if (missing.Any())
			{
				var missingList = string.Join(",", missing.Select(x => x.Key));

				throw new AgateUserInterfaceInitializationException($"While initializing facet {facet.FacetName} the following properties were unfulfilled: {missingList}");
			}

			gui.Desktop.Children.AddRange(widgets);
		}
	}
}
