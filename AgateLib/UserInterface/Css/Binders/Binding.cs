using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public class Binding
	{
		private string name;
		private string value;
		PropertyChain chain;
		object convertedValue;

		public Binding(CssPropertyMap map, string name, string value)
		{
			// TODO: Complete member initialization
			this.name = name;
			this.value = value;

			chain = map[name];

			var property = FinalProperty;

			if (typeof(ICssPropertyFromText).IsAssignableFrom(property.PropertyType))
			{
				convertedValue = value;
			}
			else
			{
				convertedValue = CssTypeConverter.ChangeType(property.PropertyType, value);
			}
		}

		PropertyInfo FinalProperty { get { return chain[chain.Count - 1]; } }

		public void Apply(CssStyleData cssStyleData)
		{
			object obj = cssStyleData;

			for (int i = 0; i < chain.Count - 1; i++)
			{
				obj = chain[i].GetValue(obj, null);
			}

			var property = chain[chain.Count - 1];

			if (typeof(ICssPropertyFromText).IsAssignableFrom(property.PropertyType))
			{
				var rfs = (ICssPropertyFromText)property.GetValue(obj, null);
				rfs.SetValueFromText(value);
			}
			else
			{
				property.SetValue(obj, convertedValue, null);
			}
		}
	}
}
