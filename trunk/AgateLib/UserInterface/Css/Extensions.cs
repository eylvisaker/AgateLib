using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	static class Extensions
	{
		public static TKey FindKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			foreach (KeyValuePair<TKey, TValue> pair in dictionary)
				if (value.Equals(pair.Value)) return pair.Key;

			throw new Exception("the value is not found in the dictionary");
		}

		public static T FindExactMatch<T>(this IEnumerable<T> list, CssSelectorGroup selector) where T : ICssCanSelect
		{
			return list.SingleOrDefault(x => x.Selector.Equals(selector));
		}

		internal static readonly char[] WhiteSpace = new char[] { ' ', '\r', '\n', '\t' };
	}
}
