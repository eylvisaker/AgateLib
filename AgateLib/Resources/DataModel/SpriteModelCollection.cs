using System.Collections;
using System.Collections.Generic;
using AgateLib.Resources.DataModel.Sprites;

namespace AgateLib.Resources.DataModel
{
	public class SpriteModelCollection : IDictionary<string, SpriteModel>
	{
		Dictionary<string, SpriteModel> spriteModels = new Dictionary<string, SpriteModel>();

		public SpriteModel this[string key]
		{
			get
			{
				return ((IDictionary<string, SpriteModel>)spriteModels)[key];
			}

			set
			{
				((IDictionary<string, SpriteModel>)spriteModels)[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return ((IDictionary<string, SpriteModel>)spriteModels).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, SpriteModel>)spriteModels).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, SpriteModel>)spriteModels).Keys;
			}
		}

		public ICollection<SpriteModel> Values
		{
			get
			{
				return ((IDictionary<string, SpriteModel>)spriteModels).Values;
			}
		}

		public void Add(KeyValuePair<string, SpriteModel> item)
		{
			((IDictionary<string, SpriteModel>)spriteModels).Add(item);
		}

		public void Add(string key, SpriteModel value)
		{
			((IDictionary<string, SpriteModel>)spriteModels).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary<string, SpriteModel>)spriteModels).Clear();
		}

		public bool Contains(KeyValuePair<string, SpriteModel> item)
		{
			return ((IDictionary<string, SpriteModel>)spriteModels).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, SpriteModel>)spriteModels).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, SpriteModel>[] array, int arrayIndex)
		{
			((IDictionary<string, SpriteModel>)spriteModels).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, SpriteModel>> GetEnumerator()
		{
			return ((IDictionary<string, SpriteModel>)spriteModels).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, SpriteModel> item)
		{
			return ((IDictionary<string, SpriteModel>)spriteModels).Remove(item);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, SpriteModel>)spriteModels).Remove(key);
		}

		public bool TryGetValue(string key, out SpriteModel value)
		{
			return ((IDictionary<string, SpriteModel>)spriteModels).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, SpriteModel>)spriteModels).GetEnumerator();
		}
	}
}