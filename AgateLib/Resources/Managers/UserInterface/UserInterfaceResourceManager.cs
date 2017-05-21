//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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
			string facetName = "unknown";

			try
			{
				facetName = facet.FacetName;

				Require.ArgumentNotNull(facetName, nameof(facet.FacetName),
					"The value of the facet's FacetName property must not be null.");

				Require.True<AgateUserInterfaceInitializationException>(
					data.Facets.ContainsKey(facetName), $"The facet '{facetName}' was not found.");

				var adapter = new AgateWidgetAdapter(fontProvider);
				var layoutEngine = new AgateLayoutEngine(adapter);
				var guiRenderer = new AgateUserInterfaceRenderer(adapter, 
					new DefaultImageProvider(data.FileProvider));

				adapter.FacetData = data.Facets;
				adapter.ThemeData = data.Themes;

				var facetModel = data.Facets[facet.FacetName];

				var gui = new FacetScene(guiRenderer, layoutEngine);

				RealizeFacetModel(facet, gui, facetModel);

				facet.InterfaceRoot = gui;
				gui.FacetName = facet.FacetName;

				adapter.InitializeStyleData(gui);

				layoutEngine.UpdateLayout(gui);
			}
			catch (Exception e) when (!(e is AgateException))
			{
				throw new AgateUserInterfaceInitializationException(
					$"Failed to initialize the facet {facetName}.", e);
			}
		}

		private void RealizeFacetModel(IUserInterfaceFacet facet, FacetScene facetScene, FacetModel facetModel)
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

			facetScene.Desktop.Windows.AddRange(widgets.Cast<Window>());
		}
	}
}
