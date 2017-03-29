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
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Mathematics.Geometry.VertexTypes
{
	/// <summary>
	/// Class which describes how vertex information is layed out in memory.
	/// </summary>
	public class VertexLayout : IList<VertexElementDesc>
	{
		List<VertexElementDesc> items = new List<VertexElementDesc>();

		/// <summary>
		/// Gets the size of a vertex structure in bytes.
		/// </summary>
		public int VertexSize
		{
			get
			{
				return this.Sum(x => x.ItemSize);
			}
		}
		/// <summary>
		/// Gets the element description for the specified vertex element.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public VertexElementDesc GetElement(VertexElement element)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].ElementType == element)
					return this[i];
			}

			throw new AgateException("Element {0} not found.", element);
		}
		/// <summary>
		/// Gets whether the described vertex structure contains the specified element.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public bool ContainsElement(VertexElement element)
		{
			return items.Any(x => x.ElementType == element);
		}
		/// <summary>
		/// Gets the byte offset into the vertex structure the specified element is at.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
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
		/// <summary>
		/// Gets the size of a particular vertex element data type.
		/// </summary>
		/// <param name="vertexElementType"></param>
		/// <returns></returns>
		public static int SizeOf(VertexElementDataType vertexElementType)
		{
			switch (vertexElementType)
			{
				case VertexElementDataType.Float1: return 1 * sizeof(float);
				case VertexElementDataType.Float2: return 2 * sizeof(float);
				case VertexElementDataType.Float3: return 3 * sizeof(float);
				case VertexElementDataType.Float4: return 4 * sizeof(float);
				case VertexElementDataType.Int: return sizeof(int);

				default: throw new NotImplementedException();
			}
		}


		#region --- IList<VertexElementDesc> Members ---

		int IList<VertexElementDesc>.IndexOf(VertexElementDesc item)
		{
			return items.IndexOf(item);
		}
		void IList<VertexElementDesc>.Insert(int index, VertexElementDesc item)
		{
			items.Insert(index, item);
		}
		/// <summary>
		/// Removes an element by its index.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			items.RemoveAt(index);
		}
		/// <summary>
		/// Gets or sets an element by its index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public VertexElementDesc this[int index]
		{
			get { return items[index]; }
			set { items[index] = value; }
		}

		#endregion
		#region --- ICollection<VertexElementDesc> Members ---

		/// <summary>
		/// Adds a vertex element to the vertex layout.
		/// </summary>
		/// <param name="item"></param>
		public void Add(VertexElementDesc item)
		{
			items.Add(item);
		}
		/// <summary>
		/// Removes all items from the vertex layout.
		/// </summary>
		public void Clear()
		{
			items.Clear();
		}
		bool ICollection<VertexElementDesc>.Contains(VertexElementDesc item)
		{
			return items.Contains(item);
		}
		void ICollection<VertexElementDesc>.CopyTo(VertexElementDesc[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns the number of items in the vertex layout.
		/// </summary>
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
		#region --- IEnumerable<VertexElementDesc> Members ---

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
	/// <summary>
	/// A class which describes a particular element of a vertex structure.
	/// </summary>
	public class VertexElementDesc
	{
		VertexElement mDef;

		/// <summary>
		/// Constructs a VertexElementDesc structure.
		/// </summary>
		/// <param name="type">The data type of the vertex element.</param>
		/// <param name="def">The semantic type of the vertex element.</param>
		public VertexElementDesc(VertexElementDataType type, VertexElement def)
		{
			DataType = type;
			ElementType = def;
		}
		/// <summary>
		/// Gets debugging string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ElementType.ToString();
		}
		/// <summary>
		/// Gets the data type of the vertex element.
		/// </summary>
		public VertexElementDataType DataType { get; private set; }
		/// <summary>
		/// Gets the semantic type of the vertex element.
		/// </summary>
		public VertexElement ElementType
		{
			get { return mDef; }
			private set
			{
				mDef = value;
			}
		}
		/// <summary>
		/// Returns the size in bytes of the vertex element.
		/// </summary>
		public int ItemSize
		{
			get { return VertexLayout.SizeOf(DataType); }
		}
	}

	/// <summary>
	/// Enumeration for describing the data types of vertex elements.
	/// </summary>
	public enum VertexElementDataType
	{
		/// <summary>
		/// A single 4-byte float.
		/// </summary>
		Float1,
		/// <summary>
		/// Two 4-byte floats.
		/// </summary>
		Float2,
		/// <summary>
		/// Three 4-byte floats.
		/// </summary>
		Float3,
		/// <summary>
		/// Four 4-byte floats.
		/// </summary>
		Float4,
		/// <summary>
		/// A single 4-byte integer.
		/// </summary>
		Int,
	}
	/// <summary>
	/// The various vertex element types.
	/// </summary>
	public enum VertexElement
	{
		/// <summary>
		/// Vertex position.
		/// </summary>
		Position,
		/// <summary>
		/// Vertex normal.
		/// </summary>
		Normal,
		/// <summary>
		/// Vertex tangent.
		/// </summary>
		Tangent,
		/// <summary>
		/// Vertex bitangent (often called binormal).
		/// </summary>
		Bitangent,
		/// <summary>
		/// The diffuse color of the vertex.
		/// </summary>
		DiffuseColor,
		/// <summary>
		/// Texture coordinates for the vertex.
		/// </summary>
		Texture,
		/// <summary>
		/// Secondary texture coordinates for the vertex.
		/// </summary>
		Texture1,
		/// <summary>
		/// Tertiary texture coordinates for the vertex.
		/// </summary>
		Texture2,
		/// <summary>
		/// Tertiary texture coordinates for the vertex.
		/// </summary>
		Texture3,
	}
}
