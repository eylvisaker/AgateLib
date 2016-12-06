using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Resources.DataModel;

namespace AgateLib.Resources.Managers.Display
{
	public class DisplayResourceManager : IDisplayResourceManager
	{
		private ResourceDataModel data;

		private Dictionary<string, Font> fonts = new Dictionary<string, Font>();

		public DisplayResourceManager(ResourceDataModel data)
		{
			this.data = data;
		}

		public Font GetFont(string name)
		{
			throw new NotImplementedException();
		}

		public void InitializeContainer(object container)
		{
			var type = container.GetType();
			var info = type.GetTypeInfo();
			var fontType = typeof(IFont).GetTypeInfo();

			foreach (var property in from property in info.DeclaredProperties
									 where fontType.IsAssignableFrom(property.PropertyType.GetTypeInfo())
									 select property)
			{
				var name = BindingName(property);

				property.SetValue(container, GetFont(name));
			}
		}

		private string BindingName(PropertyInfo property)
		{
			throw new NotImplementedException();
		}
	}
}
