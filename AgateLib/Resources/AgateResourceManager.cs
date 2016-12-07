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
		private readonly ResourceDataModel data;
		private IUserInterfaceResourceManager uiResourceManager;
		private IDisplayResourceManager displayResourceManager;

		public AgateResourceManager(string filename) : this(new ResourceDataLoader().Load(filename))
		{ }

		public AgateResourceManager(ResourceDataModel data)
		{
			DisplayResourceManager = new DisplayResourceManager(data);
			UserInterface = new UserInterfaceResourceManager(data, DisplayResourceManager);
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

		/// <summary>
		/// Initializes an object by setting all its fields that correspond to resource types
		/// with matching resources.
		/// </summary>
		/// <remarks>If the object is a user interface facet, this will initialize the facet.</remarks>
		/// <param name="container"></param>
		public void InitializeContainer(object container)
		{
			var facet = container as IUserInterfaceFacet;

			if (facet != null)
				UserInterface.InitializeFacet(facet);

			DisplayResourceManager.InitializeContainer(container);
		}
	}
}
