//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{

	public class ConsoleDictionary : Dictionary<string, Delegate>
	{
		public ConsoleDictionary() : base(StringComparer.OrdinalIgnoreCase)
		{

		}

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
