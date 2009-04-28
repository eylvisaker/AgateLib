using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	public class VertexLayout : IList<VertexElementDesc>
	{
		List<VertexElementDesc> items = new List<VertexElementDesc>();

		public int VertexSize
		{
			get
			{
				return this.Sum(x => x.ItemSize);
			}
		}
		
		public VertexElementDesc GetElement(VertexElement element)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].ElementType == element)
					return this[i];
			}

			throw new AgateException("Element {0} not found.", element);
		}

		public bool ContainsElement(VertexElement element)
		{
			return items.Any(x => x.ElementType == element);
		}
		public bool ContainsElement(string attributeName)
		{
			return items.Any(x => x.ElementType == VertexElement.Attribute && x.AttributeString == attributeName);
		}
		public int ElementByteIndex(VertexElement element)
		{
			int size = 0;

			for (int i = 0; i < Count; i++)
			{
				if (this[i].ElementType == element)
					return size;

				size += this[i].ItemSize;
			}

			throw new AgateException("Could not find the element {0} in the vertex layout.", element);
		}
		public int ElementByteIndex(string attributeName)
		{

			int size = 0;

			for (int i = 0; i < Count; i++)
			{
				if (this[i].ElementType == VertexElement.Attribute &&
					this[i].AttributeString == attributeName)
				{
					return size;
				}

				size += SizeOf(this[i].DataType);
			}

			throw new AgateException("Could not find the attribute {0} in the vertex layout.", attributeName);
		}

		public static int SizeOf(VertexElementDataType vertexElementType)
		{
			switch (vertexElementType)
			{
				case VertexElementDataType.Float1: return 1 * sizeof(float);
				case VertexElementDataType.Float2: return 2 * sizeof(float);
				case VertexElementDataType.Float3: return 3 * sizeof(float);
				case VertexElementDataType.Float4: return 4 * sizeof(float);

				default: throw new NotImplementedException();
			}
		}

		/*
		
		public VertexLayout PositionNormal
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Position),
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Normal),
				};
			}
		}
		public VertexLayout PositionTexture
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Position),
					new VertexMember(VertexMemberType.Float2, VertexMemberUsage.Texture),
				};
			}
		}
		*/

		#region --- IList<VertexMember> Members ---

		int IList<VertexElementDesc>.IndexOf(VertexElementDesc item)
		{
			return items.IndexOf(item);
		}
		void IList<VertexElementDesc>.Insert(int index, VertexElementDesc item)
		{
			items.Insert(index, item);
		}
		public void RemoveAt(int index)
		{
			items.RemoveAt(index);
		}
		public VertexElementDesc this[int index]
		{
			get { return items[index]; }
			set { items[index] = value; }
		}

		#endregion
		#region --- ICollection<VertexMember> Members ---

		public void Add(VertexElementDesc item)
		{
			items.Add(item);
		}
		public void Clear()
		{
			items.Clear();
		}
		bool ICollection<VertexElementDesc>.Contains(VertexElementDesc item)
		{
			return items.Contains(item);
		}
		public void CopyTo(VertexElementDesc[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return items.Count; }
		}

		bool ICollection<VertexElementDesc>.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<VertexElementDesc>.Remove(VertexElementDesc item)
		{
			return items.Remove(item);
		}

		#endregion
		#region --- IEnumerable<VertexMember> Members ---

		IEnumerator<VertexElementDesc> IEnumerable<VertexElementDesc>.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		#endregion
		#region --- IEnumerable Members ---

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		#endregion

	}
	public class VertexElementDesc
	{
		VertexElement mDef;
		string mAttributeString;

		public VertexElementDesc(VertexElementDataType type, VertexElement def)
		{
			if (def == VertexElement.Attribute)
				throw new AgateException("Use the (VertexMemberType, string) overload instead.");

			DataType = type;
			ElementType = def;
		}
		public VertexElementDesc(VertexElementDataType type, string attributeName)
		{
			DataType = type;
			ElementType = VertexElement.Attribute;
			AttributeString = attributeName;
		}

		public VertexElementDataType DataType { get; private set; }
		public VertexElement ElementType
		{
			get { return mDef; }
			private set
			{
				mDef = value;

				if (mDef != VertexElement.Attribute)
					mAttributeString = null;
			}
		}

		public string AttributeString
		{
			get { return mAttributeString; }
			private set
			{
				mAttributeString = value;
				mDef = VertexElement.Attribute;
			}
		}

		public int ItemSize
		{
			get { return VertexLayout.SizeOf(DataType); }
		}
	}

	public enum VertexElementDataType
	{
		Float1,
		Float2,
		Float3,
		Float4,
	}
	public enum VertexElement
	{
		Position,
		Normal,
		Tangent,
		Bitangent,
		Color,
		Texture,
		Texture1,
		Texture2,
		Texture3,
		Attribute,
	}
}
