using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Venus.Fulfillment;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources
{
	public class ResourceManager
	{
		#region --- Static Members ---

		static IGuiLayoutEngine layoutEngine;
		static IGuiRenderer guiRenderer;
		static IWidgetFactory widgetFactory;
		static IFacetInspector facetInspector;

		static VenusWidgetAdapter adapter;

		static bool initialized;

		private static void Initialize()
		{
			if (initialized)
				return;

			adapter = new VenusWidgetAdapter();
			layoutEngine = new VenusLayoutEngine(adapter);
			guiRenderer = new AgateUserInterfaceRenderer(adapter);
			facetInspector = new FacetInspector();
			widgetFactory = new WidgetFactory(AssemblyDiscoveryWidgetActivator.ForAgateLib());

			initialized = true;
		}

		private static IGuiLayoutEngine CreateLayoutEngine()
		{
			Initialize();

			return layoutEngine;
		}

		private static IGuiRenderer CreateRenderer()
		{
			Initialize();

			return guiRenderer;
		}

		#endregion

		ResourceDataModel data;


		public ResourceManager(string filename) : this(new ResourceDataLoader().Load(filename))
		{ }
		public ResourceManager(ResourceDataModel data) : this(data, CreateRenderer(), CreateLayoutEngine())
		{ }

		public ResourceManager(ResourceDataModel data, IGuiRenderer guiRenderer, IGuiLayoutEngine layoutEngine)
		{
			this.data = data;
		}

		public IGuiRenderer Renderer
		{
			get { return guiRenderer; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(value));
				guiRenderer = value;
			}
		}

		public IGuiLayoutEngine LayoutEngine
		{
			get { return layoutEngine; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(value));
				layoutEngine = value;
			}
		}

		public void InitializeFacet(IUserInterfaceFacet facet)
		{
			try
			{
				Condition.RequireArgumentNotNull(facet.FacetName, nameof(facet.FacetName), "The value of the facet's FacetName property must not be null.");

				var facetModel = data.Facets[facet.FacetName];

				var gui = new Gui(guiRenderer, layoutEngine);

				RealizeFacetModel(facet, gui, facetModel);

				facet.InterfaceRoot = gui;

			}
			catch (Exception e)
			{
				throw new AgateUserInterfaceInitializationException("Failed to initialize the facet.", e);
			}
		}

		private void RealizeFacetModel(IUserInterfaceFacet facet, Gui gui, FacetModel facetModel)
		{
			var propertyMap = facetInspector.BuildPropertyMap(facet);

			widgetFactory.RealizeFacetModel(facetModel, (name, widget) =>
			{
				propertyMap[name].Assign(widget);
			});
		}
	}
}
