using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			ContainerInitializer init = new ContainerInitializer(new TypeResolver(), models.Where(model => model.Namespace == @namespace));

			var widgets = init.Initialize(ui);
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
