using System.Collections;
using System.Collections.Generic;

namespace AgateLib.Resources.DataModel
{
	public class SurfaceModelCollection : IDictionary<string, SurfaceModel>
	{
		Dictionary<string, SurfaceModel> surfaces = new Dictionary<string, SurfaceModel>();

		public SurfaceModel this[string key]
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces)[key];
			}

			set
			{
				((IDictionary<string, SurfaceModel>)surfaces)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).Keys;
			}
		}

		public ICollection<SurfaceModel> Values
		{
			get
			{
				return ((IDictionary<string, SurfaceModel>)surfaces).Values;
			}
		}

		public void Add(KeyValuePair<string, SurfaceModel> item)
		{
			((IDictionary<string, SurfaceModel>)surfaces).Add(item);
		}

		public void Add(string key, SurfaceModel value)
		{
			((IDictionary<string, SurfaceModel>)surfaces).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, SurfaceModel>)surfaces).Clear();
		}

		public bool Contains(KeyValuePair<string, SurfaceModel> item)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, SurfaceModel>[] array, int arrayIndex)
		{
			((IDictionary<string, SurfaceModel>)surfaces).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, SurfaceModel>> GetEnumerator()
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, SurfaceModel> item)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).Remove(key);
		}

		public bool TryGetValue(string key, out SurfaceModel value)
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, SurfaceModel>)surfaces).GetEnumerator();
		}
	}
}