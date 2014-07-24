using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{

	public class ConsoleDictionary : Dictionary<string, Delegate>
	{
		public void Add<T>(string key, Action<T> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2>(string key, Action<T1, T2> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3>(string key, Action<T1, T2, T3> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> value)
		{
			base.Add(key, value);
		}

		public void Add<TResult>(string key, Func<TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, TResult>(string key, Func<T1, TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, TResult>(string key, Func<T1, T2, TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3, TResult>(string key, Func<T1, T2, T3, TResult> value)
		{
			base.Add(key, value);
		}
		public void Add<T1, T2, T3, T4, TResult>(string key, Func<T1, T2, T3, T4, TResult> value)
		{
			base.Add(key, value);
		}
	}
}
