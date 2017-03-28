//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace AgateLib.UserInterface.DataModel
{
	public class ThemeModel : IDictionary<string, WidgetThemeModel>
	{
		Dictionary<string, WidgetThemeModel> widgets =
			new Dictionary<string, WidgetThemeModel>(StringComparer.OrdinalIgnoreCase);

		public WidgetThemeModel this[string key]
		{
			get { return ((IDictionary<string, WidgetThemeModel>)widgets)[key]; }
			set { ((IDictionary<string, WidgetThemeModel>)widgets)[key] = value; }
		}

		public int Count => widgets.Count;

		public bool IsReadOnly => false;

		public ICollection<string> Keys => widgets.Keys;

		public ICollection<WidgetThemeModel> Values => widgets.Values;

		public void ApplyPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return;

			foreach(var themeModel in widgets.Values)
			{
				ApplyPathToState(path, themeModel);

				foreach (var state in themeModel.State.Values)
				{
					ApplyPathToState(path, state);
				}
			}
		}

		private void ApplyPathToState(string path, WidgetStateModel stateModel)
		{
			if (stateModel.Background != null && !string.IsNullOrWhiteSpace(stateModel.Background.Image))
				stateModel.Background.Image = $"{path}/{stateModel.Background.Image}";

			if (stateModel.Border != null && !string.IsNullOrWhiteSpace(stateModel.Border.Image))
				stateModel.Border.Image = $"{path}/{stateModel.Border.Image}";
		}

		public void Add(KeyValuePair<string, WidgetThemeModel> item)
		{
			((IDictionary<string, WidgetThemeModel>)widgets).Add(item);
		}

		public void Add(string key, WidgetThemeModel value)
		{
			((IDictionary<string, WidgetThemeModel>)widgets).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, WidgetThemeModel>)widgets).Clear();
		}

		public bool Contains(KeyValuePair<string, WidgetThemeModel> item)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, WidgetThemeModel>[] array, int arrayIndex)
		{
			((IDictionary<string, WidgetThemeModel>)widgets).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, WidgetThemeModel>> GetEnumerator()
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, WidgetThemeModel> item)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).Remove(key);
		}

		public bool TryGetValue(string key, out WidgetThemeModel value)
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, WidgetThemeModel>)widgets).GetEnumerator();
		}
	}
}