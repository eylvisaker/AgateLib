using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.Resources.DataModel;
using AgateLib.Resources.Managers;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Venus.Fulfillment;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources
{
	public class AgateResourceManager
	{
		ResourceDataModel data;
		UserInterfaceResourceManager uiResourceManager;

		public AgateResourceManager(string filename) : this(new ResourceDataLoader().Load(filename))
		{ }
		public AgateResourceManager(ResourceDataModel data)
		{
			UserInterface = new UserInterfaceResourceManager(data);
		}

		public UserInterfaceResourceManager UserInterface
		{
			get { return uiResourceManager; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(UserInterface));
				uiResourceManager = value;
			}
		}
	}
}
