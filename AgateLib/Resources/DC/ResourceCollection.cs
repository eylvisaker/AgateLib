using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AgateLib.Resources.DC
{
	[DataContract(Namespace="")]
	public class ResourceCollection : IDictionary<string, AgateResource>
	{
		[DataMember]
		Dictionary<string, AgateResource> mResources = new Dictionary<string, AgateResource>();

		public void Add(string key, AgateResource value)
		{
			mResources.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return mResources.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return mResources.Keys; }
		}

		public bool Remove(string key)
		{
			return mResources.Remove(key);
		}

		public bool TryGetValue(string key, out AgateResource value)
		{
			return mResources.TryGetValue(key, out value);
		}

		public ICollection<AgateResource> Values
		{
			get { return mResources.Values; }
		}

		public AgateResource this[string key]
		{
			get { return mResources[key]; }
			set { mResources[key] = value; }
		}

		void ICollection<KeyValuePair<string, AgateResource>>.Add(KeyValuePair<string, AgateResource> item)
		{
			mResources.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			mResources.Clear();
		}

		bool ICollection<KeyValuePair<string, AgateResource>>.Contains(KeyValuePair<string, AgateResource> item)
		{
			return mResources.Contains(item);
		}

		void ICollection<KeyValuePair<string, AgateResource>>.CopyTo(KeyValuePair<string, AgateResource>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, AgateResource>>)mResources).CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mResources.Count; }
		}

		bool ICollection<KeyValuePair<string, AgateResource>>.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<KeyValuePair<string, AgateResource>>.Remove(KeyValuePair<string, AgateResource> item)
		{
			return mResources.Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<string, AgateResource>> GetEnumerator()
		{
			return mResources.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
