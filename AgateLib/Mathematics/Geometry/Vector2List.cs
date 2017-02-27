using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// List of Vector2 objects.
	/// </summary>
	public class Vector2List : IVector2List
	{
		private List<Vector2> list = new List<Vector2>();

		/// <summary>
		/// Constructs a Vector2List object.
		/// </summary>
		public Vector2List()
		{
			
		}

		/// <summary>
		/// Constructs a Vector2List object, adding the vectors passed in.
		/// </summary>
		/// <param name="vectors"></param>
		public Vector2List(IEnumerable<Vector2> vectors)
		{
			list = vectors.ToList();
		}

		internal bool Dirty { get; set; }

		/// <summary>
		/// Gets or sets the count of items in this list.
		/// </summary>
		public int Count
		{
			get { return list.Count; }
			set
			{
				var oldList = list;
				list = new List<Vector2>(value);
				list.AddRange(oldList.Take(value));

				while (list.Count < value)
					list.Add(Vector2.Zero);

				Dirty = true;
			}
		}

		/// <summary>
		///  Returns false.
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Gets or sets a vector at a specific index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Vector2 this[int index]
		{
			get { return list[index]; }
			set
			{
				list[index] = value;
				Dirty = true;
			}
		}

		public override string ToString()
		{
			return $"Count = {Count}";
		}

		/// <summary>
		/// Enumerates the Vector2 values.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Vector2> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		/// <summary>
		/// Enumerates the Vector2 values.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)list).GetEnumerator();
		}

		/// <summary>
		/// Adds a point to the list.
		/// </summary>
		/// <param name="item"></param>
		public void Add(Vector2 item)
		{
			list.Add(item);
			Dirty = true;
		}

		/// <summary>
		/// Adds a point to the list.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Add(double x, double y)
		{
			list.Add(new Vector2(x, y));
			Dirty = true;
		}

		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			list.Clear();
			Dirty = true;
		}

		/// <summary>
		/// Returns true if the list contains the item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(Vector2 item)
		{
			return list.Contains(item);
		}

		/// <summary>
		/// Copies the items to an array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(Vector2[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes an item from this list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(Vector2 item)
		{
			Dirty = true;
			return list.Remove(item);
		}

		/// <summary>
		/// Gets the index of the specified vector.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(Vector2 item)
		{
			return list.IndexOf(item);
		}

		/// <summary>
		/// Inserts a vector into the list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, Vector2 item)
		{
			Dirty = true;
			list.Insert(index, item);
		}

		/// <summary>
		/// Removes a vector from the list.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			Dirty = true;
			list.RemoveAt(index);
		}
	}

	/// <summary>
	/// Interface for a list of Vector2 values.
	/// </summary>
	public interface IVector2List : IList<Vector2>, IReadOnlyList<Vector2>
	{
		/// <summary>
		/// Adds a point to the list.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		void Add(double x, double y);
	}
}