using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform;

namespace AgateLib.UserInterface.Venus.Hierarchy
{
	public class LayoutForDevice : ILayoutCondition
	{
		public LayoutForDevice(DeviceType deviceType)
		{
			DeviceType = deviceType;
		}

		public DeviceType DeviceType { get; set; }

		public bool ApplyLayoutModel(LayoutEnvironment environment, LayoutModel model)
		{
			return environment.DeviceType == DeviceType;
		}
	}
}
