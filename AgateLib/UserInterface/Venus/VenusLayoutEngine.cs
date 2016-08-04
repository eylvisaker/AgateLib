using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Venus.Fulfillment;
using AgateLib.UserInterface.Venus.Hierarchy;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class VenusLayoutEngine : IGuiLayoutEngine
	{
		private List<LayoutModel> models = new List<LayoutModel>();

		public void UpdateLayout(Gui gui)
		{
			
		}

		public void InitializeWidgets(string @namespace, IUserInterfaceContainer ui)
		{
			var currentModels = models.Where(model => model.Namespace == @namespace);
			var typeResolver = new TypeResolver();

			WidgetBuilder builder = new WidgetBuilder(typeResolver, currentModels);
			builder.BuildWidgets();

			ContainerInitializer init = new ContainerInitializer();
			init.Initialize(ui, builder);
		}

		public void AddLayoutModel(LayoutModel layoutModel)
		{
			models.Add(layoutModel);
		}
	}

	public interface IUserInterfaceContainer
	{
	}
}
