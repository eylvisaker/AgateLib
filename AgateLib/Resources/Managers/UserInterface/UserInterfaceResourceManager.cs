//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Fulfillment;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources.Managers.UserInterface
{
	public class UserInterfaceResourceManager : IUserInterfaceResourceManager
	{
		private readonly IFontProvider fontProvider;
		private IWidgetFactory widgetFactory;
		private ITypeInspector<Widget> facetInspector;

		private ResourceDataModel data;

		public UserInterfaceResourceManager(ResourceDataModel data, IFontProvider fontProvider)
		{
			this.fontProvider = fontProvider;

			this.data = data;

			Initialize();
		}

		public void Dispose()
		{

		}

		private void Initialize()
		{
			facetInspector = new TypeInspector<Widget>();
			widgetFactory = new WidgetFactory(AssemblyDiscoveryWidgetActivator.ForAgateLib());
		}

		public void InitializeFacet(IUserInterfaceFacet facet)
		{
			try
			{
				var adapter = new AgateWidgetAdapter(fontProvider);
				var layoutEngine = new AgateLayoutEngine(adapter);
				var guiRenderer = new AgateUserInterfaceRenderer(adapter);

				adapter.FacetData = data.Facets;
				adapter.ThemeData = data.Themes;

				Condition.RequireArgumentNotNull(facet.FacetName, nameof(facet.FacetName), 
					"The value of the facet's FacetName property must not be null.");

				var facetModel = data.Facets[facet.FacetName];

				var gui = new Gui(guiRenderer, layoutEngine);

				RealizeFacetModel(facet, gui, facetModel);

				facet.InterfaceRoot = gui;
				gui.FacetName = facet.FacetName;

				adapter.InitializeStyleData(gui);
			}
			catch (Exception e) when (!(e is AgateException))
			{
				throw new AgateUserInterfaceInitializationException("Failed to initialize the facet.", e);
			}
		}

		private void RealizeFacetModel(IUserInterfaceFacet facet, Gui gui, FacetModel facetModel)
		{
			var propertyMap = facetInspector.BuildPropertyMap(facet);
			List<PropertyMapValue<Widget>> assigned = new List<PropertyMapValue<Widget>>();

			var widgets = widgetFactory.RealizeFacetModel(facetModel, (name, widget) =>
			{
				if (name == null)
					return;

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

			gui.Desktop.Windows.AddRange(widgets.Cast<Window>());
		}
	}
}
