using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssRuleBlock : ICssCanSelect
	{
		CssPropertyMap map;

		public CssRuleBlock(CssPropertyMap map)
		{
			this.map = map;

			Properties = new Dictionary<string, string>();
			Bindings = new Dictionary<string, Binding>();
		}

		public Dictionary<string, Binding> Bindings { get; private set; }
		public Dictionary<string, string> Properties { get; private set; }

		public CssSelectorGroup Selector { get; set; }

		public override string ToString()
		{
			return Selector.ToString() + " { "
				+ string.Join(" ",
				Properties.Select(x => x.Key + ":" + x.Value + ";")) + "}";
		}

		public void AddProperty(string name, string value)
		{
			Properties.Add(name, value);

			Bindings.Add(name, new Binding(map, name, value));
		}


		public void ApplyProperties(CssStyleData cssStyleData)
		{
			foreach(var binding in Bindings.Values)
			{
				binding.Apply(cssStyleData);
			}
		}
	}
}