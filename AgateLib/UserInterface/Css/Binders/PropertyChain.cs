using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public class PropertyChain : IList<PropertyInfo>
	{
		List<PropertyInfo> mProperties = new List<PropertyInfo>();

		
		public int IndexOf(PropertyInfo item)
		{
			return mProperties.IndexOf(item);
		}

		public void Insert(int index, PropertyInfo item)
		{
			mProperties.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			mProperties.RemoveAt(index);
		}

		public PropertyInfo this[int index]
		{
			get { return mProperties[index]; }
			set { mProperties[index] = value; }
		}

		public void Add(PropertyInfo item)
		{
			mProperties.Add(item);
		}
		public void AddRange(IEnumerable<PropertyInfo> items)
		{
			mProperties.AddRange(items);
		}

		public void Clear()
		{
			mProperties.Clear();
		}

		public bool Contains(PropertyInfo item)
		{
			return mProperties.Contains(item);
		}

		public void CopyTo(PropertyInfo[] array, int arrayIndex)
		{
			mProperties.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mProperties.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(PropertyInfo item)
		{
			return mProperties.Remove(item);
		}

		public IEnumerator<PropertyInfo> GetEnumerator()
		{
			return mProperties.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
