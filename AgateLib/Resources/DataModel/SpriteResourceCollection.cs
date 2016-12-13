using System.Collections;
using System.Collections.Generic;

namespace AgateLib.Resources.DataModel
{
	public class SpriteResourceCollection : IDictionary<string, SpriteResource>
	{
		Dictionary<string, SpriteResource> spriteModels = new Dictionary<string, SpriteResource>();

		public SpriteResource this[string key]
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels)[key];
			}

			set
			{
				((IDictionary<string, SpriteResource>)spriteModels)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).Keys;
			}
		}

		public ICollection<SpriteResource> Values
		{
			get
			{
				return ((IDictionary<string, SpriteResource>)spriteModels).Values;
			}
		}

		public void Add(KeyValuePair<string, SpriteResource> item)
		{
			((IDictionary<string, SpriteResource>)spriteModels).Add(item);
		}

		public void Add(string key, SpriteResource value)
		{
			((IDictionary<string, SpriteResource>)spriteModels).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, SpriteResource>)spriteModels).Clear();
		}

		public bool Contains(KeyValuePair<string, SpriteResource> item)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, SpriteResource>[] array, int arrayIndex)
		{
			((IDictionary<string, SpriteResource>)spriteModels).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, SpriteResource>> GetEnumerator()
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, SpriteResource> item)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).Remove(key);
		}

		public bool TryGetValue(string key, out SpriteResource value)
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, SpriteResource>)spriteModels).GetEnumerator();
		}
	}
}