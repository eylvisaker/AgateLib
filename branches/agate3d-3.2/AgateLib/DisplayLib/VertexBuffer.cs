using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public sealed class VertexBuffer
	{
		VertexBufferImpl impl;

		public VertexBuffer(VertexLayout layout, int vertexCount)
		{
			if (layout == null)
				throw new ArgumentNullException(
					"The supplied VertexLayout must not be null.  " +
					"You may wish to use one of the static members of VertexLayout.");
			if (layout.Count == 0)
				throw new ArgumentException(
					"The supplied VertexLayout has no items in it.  You must supply a valid layout.");

			impl = Display.Impl.CreateVertexBuffer(layout, vertexCount);
		}

		public void WriteVertexData(Vector3[] data)
		{
			impl.WriteVertexData(data);
		}
		public void WriteTextureCoords(Vector2[] texCoords)
		{
			impl.WriteTextureCoords(texCoords);
		}
		public void WriteNormalData(Vector3[] data)
		{
			impl.WriteNormalData(data);
		}
		public void WriteIndices(short[] indices)
		{
			impl.WriteIndices(indices);
			impl.Indexed = true;
		}
		public void WriteAttributeData(string attributeName, Vector3[] data)
		{
			impl.WriteAttributeData(attributeName, data);
		}

		public bool Indexed
		{
			get { return impl.Indexed; }
			set { impl.Indexed = value; }
		}
		public void Draw()
		{
			impl.Draw();
		}
		public void Draw(int start, int count)
		{
			impl.Draw(start, count);
		}

		public int VertexCount
		{
			get { return impl.VertexCount; }
		}
		public int IndexCount
		{
			get { return impl.IndexCount; }
		}
		public PrimitiveType PrimitiveType
		{
			get { return impl.PrimitiveType; }
			set { impl.PrimitiveType = value; }
		}
		public TextureList Textures
		{
			get { return impl.Textures; }
		}

	}

	public class TextureList
	{
		Surface[] surfs = new Surface[4];

		public Surface this[int index]
		{
			get { return surfs[index]; }
			set { surfs[index] = value; }
		}
		public int Count
		{
			get { return surfs.Length; }
		}
		public int ActiveTextures
		{
			get
			{
				int activeCount = 0;
				for (int i = 0; i < Count; i++)
				{
					if (this[i] == null)
						continue;

					activeCount++;
				}

				return activeCount;
			}
		}
	}

	public class VertexLayout : IList<VertexMember>
	{
		List<VertexMember> items = new List<VertexMember>();

		int VertexSize
		{
			get
			{
				return this.Sum(x => x.ItemSize);
			}
		}

		public static VertexLayout PositionNormalTexture
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Position),
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Normal),
					new VertexMember(VertexMemberType.Float2, VertexMemberUsage.Texture),
				};
			}
		}
		public static VertexLayout PositionNormalTangentBitangentTexture
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Position),
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Normal),
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Tangent),
					new VertexMember(VertexMemberType.Float3, VertexMemberUsage.Bitangent),
					new VertexMember(VertexMemberType.Float2, VertexMemberUsage.Texture),
				};
			}
		}
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

		#region --- IList<VertexMember> Members ---

		int IList<VertexMember>.IndexOf(VertexMember item)
		{
			return items.IndexOf(item);
		}
		void IList<VertexMember>.Insert(int index, VertexMember item)
		{
			items.Insert(index, item);
		}
		public void RemoveAt(int index)
		{
			items.RemoveAt(index);
		}
		public VertexMember this[int index]
		{
			get { return items[index]; }
			set { items[index] = value; }
		}

		#endregion
		#region --- ICollection<VertexMember> Members ---

		public void Add(VertexMember item)
		{
			items.Add(item);
		}
		public void Clear()
		{
			items.Clear();
		}
		bool ICollection<VertexMember>.Contains(VertexMember item)
		{
			return items.Contains(item);
		}
		public void CopyTo(VertexMember[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return items.Count; }
		}

		bool ICollection<VertexMember>.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection<VertexMember>.Remove(VertexMember item)
		{
			return items.Remove(item);
		}

		#endregion
		#region --- IEnumerable<VertexMember> Members ---

		IEnumerator<VertexMember> IEnumerable<VertexMember>.GetEnumerator()
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

		public bool Contains(VertexMemberUsage vertexMemberUsage)
		{
			return items.Any(x => x.Usage == vertexMemberUsage);
		}
		public bool Contains(string attributeName)
		{
			return items.Any(x => x.Usage == VertexMemberUsage.Attribute && x.AttributeString == attributeName);
		}
	}

	public class VertexMember
	{
		VertexMemberUsage mDef;
		string mAttributeString;

		public VertexMember(VertexMemberType type, VertexMemberUsage def)
		{
			if (def == VertexMemberUsage.Attribute)
				throw new AgateException("Use the (VertexMemberType, string) overload instead.");

			MemberType = type;
			Usage = def;
		}

		public VertexMember(VertexMemberType type, string attributeName)
		{
			MemberType = type;
			Usage = VertexMemberUsage.Attribute;
			AttributeString = attributeName;
		}

		public VertexMemberType MemberType { get; private set; }
		public VertexMemberUsage Usage
		{
			get { return mDef; }
			private set
			{
				mDef = value;

				if (mDef != VertexMemberUsage.Attribute)
					mAttributeString = null;
			}
		}
		public string AttributeString
		{
			get { return mAttributeString; }
			private set
			{
				mAttributeString = value;
				mDef = VertexMemberUsage.Attribute;
			}
		}

		public int ItemSize
		{
			get
			{
				return 1;
			}
		}
	}

	public enum VertexMemberType
	{
		Float1,
		Float2,
		Float3,
		Float4,
	}

	public enum VertexMemberUsage
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
