using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
	public class DefaultStyleCollection
	{
		Dictionary<Type, IDefaultStyleSetter> mSetters = new Dictionary<Type, IDefaultStyleSetter>();
		IDefaultStyleSetter mDefaultSetter = new BlankDefaultStyle();

		public DefaultStyleCollection()
		{
			mSetters[typeof(Window)] = new WindowDefaultStyle();
			mSetters[typeof(Menu)] = new MenuDefaultStyle();
		}

		public void Add(IDefaultStyleSetter styleSetter, params Type[] types)
		{
			if (types.Length == 0)
				throw new ArgumentException("You must supply at least one type to use.");

			foreach (var t in types)
				mSetters[t] = styleSetter;
		}

		public IDefaultStyleSetter this[Type type]
		{
			get
			{
				if (mSetters.ContainsKey(type))
					return mSetters[type];
				else
					return mDefaultSetter;
			}
		}
	}
}
