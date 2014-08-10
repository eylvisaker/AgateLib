using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public class CssBindingMapper
	{
		Dictionary<Type, ReflectionPropertyBinder> mPropertyBinders = new Dictionary<Type, ReflectionPropertyBinder>();
		private CssPropertyMap map;

		public CssBindingMapper(CssPropertyMap destMap)
		{
			map = destMap;
		}
		public ReflectionPropertyBinder GetPropertyBinder(Type type)
		{
			if (mPropertyBinders.ContainsKey(type) == false)
				mPropertyBinders.Add(type, CreatePropertyBinder(type));

			return mPropertyBinders[type];
		}

		private ReflectionPropertyBinder CreatePropertyBinder(Type type)
		{
			return new ReflectionPropertyBinder(type, this);
		}

		public PropertyChain GetCssPropertyChain(Type objType, string name)
		{
			var binder = GetPropertyBinder(objType);

			return binder.GetCssPropertyChain(objType, name);
		}

		public bool FindPropertyChain(string property)
		{
			if (map.ContainsKey(property))
				return true;

			var result = GetCssPropertyChain(typeof(CssStyleData), property);

			if (result == null)
				return false;

			if (result.Count > 0)
			{
				map.AddChain(property, result);
				return true;
			}

			return false;
		}
	}
}
