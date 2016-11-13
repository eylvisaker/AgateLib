using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Venus.Fulfillment;
using AgateLib.UserInterface.Venus.LayoutModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class VenusLayoutEngine : IGuiLayoutEngine
	{
		private VenusWidgetAdapter adapter;

		public VenusLayoutEngine(VenusWidgetAdapter adapter)
		{
			this.adapter = adapter;
		}

		public void UpdateLayout(Gui gui)
		{

		}

		public void InitializeWidgets(string @namespace, IUserInterfaceContainer ui)
		{
			var currentModels = adapter.SelectModels(@namespace);
			var typeResolver = new TypeResolver();

			WidgetBuilder builder = new WidgetBuilder(typeResolver, currentModels);
			builder.BuildWidgets();

			ContainerInitializer init = new ContainerInitializer();
			init.Initialize(ui, builder);
		}
	}

	public interface IUserInterfaceContainer
	{
	}
}
