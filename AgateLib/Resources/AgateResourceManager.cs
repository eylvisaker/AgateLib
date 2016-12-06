using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.Resources.DataModel;
using AgateLib.Resources.Managers.Display;
using AgateLib.Resources.Managers.UserInterface;
using AgateLib.UserInterface;

namespace AgateLib.Resources
{
	public class AgateResourceManager
	{
		ResourceDataModel data;
		IUserInterfaceResourceManager uiResourceManager;
		IDisplayResourceManager displayResourceManager;

		public AgateResourceManager(string filename) : this(new ResourceDataLoader().Load(filename))
		{ }
		public AgateResourceManager(ResourceDataModel data)
		{
			UserInterface = new UserInterfaceResourceManager(data);
			DisplayResourceManager = new DisplayResourceManager(data);
		}

		public IDisplayResourceManager DisplayResourceManager
		{
			get { return displayResourceManager; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(DisplayResourceManager));
				displayResourceManager = value;
			}
		}

		public IUserInterfaceResourceManager UserInterface
		{
			get { return uiResourceManager; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(UserInterface));
				uiResourceManager = value;
			}
		}

		public void InitializeContainer(object container)
		{
			var facet = container as IUserInterfaceFacet;

			if (container != null)
				UserInterface.InitializeFacet(facet);

			DisplayResourceManager.InitializeContainer(container);
		}
	}
}
