using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public class CssPropertyMap
	{
		Dictionary<string, PropertyChain> mPropertyBinding = new Dictionary<string, PropertyChain>();

		public PropertyChain this[string prop]
		{
			get { return mPropertyBinding[prop]; }
		}

		public void AddChain(string name, PropertyChain chain)
		{
			mPropertyBinding.Add(name, chain);
		}

		public bool ContainsKey(string property)
		{
			return mPropertyBinding.ContainsKey(property);
		}
	}
}
