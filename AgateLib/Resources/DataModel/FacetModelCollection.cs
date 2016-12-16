using System;
using System.Collections;
using System.Collections.Generic;
using AgateLib.UserInterface.DataModel;

namespace AgateLib.Resources.DataModel
{
	public class FacetModelCollection : IDictionary<string, FacetModel>
	{
		Dictionary<string, FacetModel> facets = new Dictionary<string, FacetModel>();

		public FacetModel this[string key]
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets)[key];
			}

			set
			{
				((IDictionary<string, FacetModel>)facets)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).Keys;
			}
		}

		public ICollection<FacetModel> Values
		{
			get
			{
				return ((IDictionary<string, FacetModel>)facets).Values;
			}
		}
		
		public void Add(KeyValuePair<string, FacetModel> item)
		{
			((IDictionary<string, FacetModel>)facets).Add(item);
		}

		public void Add(string key, FacetModel value)
		{
			((IDictionary<string, FacetModel>)facets).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, FacetModel>)facets).Clear();
		}

		public bool Contains(KeyValuePair<string, FacetModel> item)
		{
			return ((IDictionary<string, FacetModel>)facets).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, FacetModel>)facets).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, FacetModel>[] array, int arrayIndex)
		{
			((IDictionary<string, FacetModel>)facets).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, FacetModel>> GetEnumerator()
		{
			return ((IDictionary<string, FacetModel>)facets).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, FacetModel> item)
		{
			return ((IDictionary<string, FacetModel>)facets).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, FacetModel>)facets).Remove(key);
		}

		public bool TryGetValue(string key, out FacetModel value)
		{
			return ((IDictionary<string, FacetModel>)facets).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, FacetModel>)facets).GetEnumerator();
		}

		internal void Validate()
		{
			foreach(var facet in this.Values)
			{
				facet.Validate();
			}
		}
	}
}