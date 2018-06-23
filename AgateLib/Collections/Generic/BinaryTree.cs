//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;

namespace AgateLib.Collections.Generic
{
	/// <summary>
	/// Class representing a binary tree.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BinaryTree<T> : IBinaryTree<T>
	{
		private int count;

		class AscendingEnumerable : IEnumerable<BinaryTreeNode<T>>
		{
			private BinaryTreeNode<T> root;

			public AscendingEnumerable(BinaryTreeNode<T> root)
			{
				this.root = root;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
			{
				var current = root?.Min();

				while (current != null)
				{
					yield return current;

					current = current.Next();
				}
			}
		}

		class DescendingEnumerable : IEnumerable<BinaryTreeNode<T>>
		{
			private BinaryTreeNode<T> root;

			public DescendingEnumerable(BinaryTreeNode<T> root)
			{
				this.root = root;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
			{
				var current = root?.Max();

				while (current != null)
				{
					yield return current;

					current = current.Previous();
				}
			}
		}

		class PseudoBinaryTreeNode : IBinaryTreeNodeStructure<T>
		{
			private BinaryTreeNode<T> right;

			public BinaryTreeNode<T> Left
			{
				get => null;
				set => throw new InvalidOperationException("The PseudoBinaryTreeNode should never have a left value.");
			}

			public BinaryTreeNode<T> Right
			{
				get => right;
				set
				{
					right = value;

					if (value != null)
					{
						if (value.Parent?.Left == value) value.Parent.Left = null;
						if (value.Parent?.Right == value) value.Parent.Right = null;

						value.Parent = null;
					}
				}
			}
		}

		private readonly Comparison<T> comparison;

		private BinaryTreeNode<T> root;

		/// <summary>
		/// Constructs a binary tree using the default comparer. The default comparer uses
		/// the hash codes of the objects.
		/// </summary>
		public BinaryTree()
		{
			comparison = (x, y) => x.GetHashCode().CompareTo(y.GetHashCode());
		}

		/// <summary>
		/// Constructs a binary tree with a comparison lambda.
		/// </summary>
		/// <param name="comparison"></param>
		public BinaryTree(Comparison<T> comparison)
		{
			this.comparison = comparison;
		}

		/// <summary>
		/// Constructs a binary tree with an IComparer&lt;T&gt; object.
		/// </summary>
		/// <param name="comparer"></param>
		public BinaryTree(IComparer<T> comparer)
		{
			this.comparison = (x, y) => comparer.Compare(x, y);
		}

		public BinaryTreeNode<T> Root => root;

		/// <summary>
		/// Set this to true to balance the tree at every operation.
		/// </summary>
		public bool AutoBalance { get; set; }

		/// <summary>
		/// Set this to false to suppress exceptions from being thrown if an item is already in the
		/// tree during an insert operation.
		/// </summary>
		public bool ThrowIfExists { get; set; } = true;

		/// <summary>
		/// Enumerates all the items in the tree in an arbitrary order.
		/// </summary>
		public IEnumerable<BinaryTreeNode<T>> Items => EnumerateItems(root);

		private IEnumerable<BinaryTreeNode<T>> EnumerateItems(BinaryTreeNode<T> current)
		{
			if (current == null)
				yield break;

			yield return current;

			foreach (var item in EnumerateItems(current.Left))
				yield return item;

			foreach (var item in EnumerateItems(current.Right))
				yield return item;
		}

		/// <summary>
		/// Enumerates the items in the tree in an ascending order.
		/// </summary>
		public IEnumerable<BinaryTreeNode<T>> ItemsAscending => new AscendingEnumerable(root);

		/// <summary>
		/// Enumerates the items in the tree in a descending order.
		/// </summary>
		public IEnumerable<BinaryTreeNode<T>> ItemsDescending => new DescendingEnumerable(root);

		/// <summary>
		/// Returns the count of objects in the tree.
		/// </summary>
		public int Count
		{
			get => count;
			private set
			{
				Require.ArgumentInRange(value >= 0, nameof(Count), "Count must not be negative.");
				count = value;
			}
		}

		/// <summary>
		/// Returns true if the tree is balanced.
		/// </summary>
		public bool IsBalanced => IsBalancedAt(root);

		private bool IsBalancedAt(BinaryTreeNode<T> node)
		{
			if (node == null)
				return true;

			var depth = node.Depth;
			var diff = (node.Left?.MaximumDepth ?? depth) - (node.Right?.MaximumDepth ?? depth);

			if (Math.Abs(diff) > 1)
				return false;

			return IsBalancedAt(node.Left) && IsBalancedAt(node.Right);
		}

		/// <summary>
		/// Inserts a set of items into the tree.
		/// </summary>
		/// <param name="items"></param>
		public void Add(IEnumerable<T> items)
		{
			try
			{
				foreach (var item in items)
				{
					AddNoBalance(new BinaryTreeNode<T>(this, item));
				}
			}
			finally
			{
				DoAutoBalance();
			}
		}

		/// <summary>
		/// Add an item to the tree.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <remarks>Throws an argument not found exception if ThrowIfExists is true and the item already exists.</remarks>
		public BinaryTreeNode<T> Add(T item)
		{
			try
			{
				return AddNoBalance(item);
			}
			finally
			{
				if (AutoBalance)
					Balance();
			}
		}

		private BinaryTreeNode<T> AddNoBalance(T item)
		{
			var nodeToInsert = new BinaryTreeNode<T>(this, item);

			return AddNoBalance(nodeToInsert);
		}

		private BinaryTreeNode<T> AddNoBalance(BinaryTreeNode<T> nodeToInsert)
		{
			if (root == null)
			{
				root = nodeToInsert;
				root.Parent = null;

				Count = root.Count;

				return root;
			}

			return AddNoBalance(root, nodeToInsert);
		}

		private BinaryTreeNode<T> AddNoBalance(BinaryTreeNode<T> node, BinaryTreeNode<T> item)
		{
			Require.ArgumentNotNull(node, "Cannot insert below a null node.");

			var comp = comparison(item.Value, node.Value);

			if (comp < 0)
			{
				if (node.Left != null)
					return AddNoBalance(node.Left, item);

				node.Left = item;
				node.Left.Parent = node;
				Count += item.Count;

				return node.Left;
			}
			if (comp > 0)
			{
				if (node.Right != null)
					return AddNoBalance(node.Right, item);

				node.Right = item;
				node.Right.Parent = node;
				Count += item.Count;

				return node.Right;
			}

			if (ThrowIfExists)
			{
				throw new ArgumentException("The item already exists in the tree.");
			}

			return node;
		}

		#region --- Balancing Algorithm ---

		/// <summary>
		/// Balances the binary tree.
		/// </summary>
		/// <remarks>Implements the algorithm in the paper
		/// Stout, Q.F. and Warren, B.L. Tree Rebalancing in Optimal Time and Space by 
		/// <em>Commun. ACM</em> 29, 9 (Sept. 1986) 902-908
		/// http://web.eecs.umich.edu/~qstout/pap/CACM86.pdf
		/// This algorithm is O(N) in time and O(1) in space to rebalance the binary tree.
		/// And it was really easy to implement! The pseudocode in the paper was very clear.
		/// </remarks>
		public void Balance()
		{
			var pseudoRoot = new PseudoBinaryTreeNode();
			pseudoRoot.Right = root;

			TreeToVine(pseudoRoot);
			VineToTree(pseudoRoot, Count);

			root = pseudoRoot.Right;
		}

		private static void TreeToVine(IBinaryTreeNodeStructure<T> start)
		{
			var vineTail = start;
			var remainder = vineTail.Right;

			while (remainder != null)
			{
				if (remainder.Left == null)
				{
					// move vineTail down one
					vineTail = remainder;
					remainder = remainder.Right;
				}
				else
				{
					// rotate items
					var temp = remainder.Left;
					remainder.Left = temp.Right;
					temp.Right = remainder;
					remainder = temp;
					vineTail.Right = temp;
				}
			}
		}

		private static void VineToTree(IBinaryTreeNodeStructure<T> start, int size)
		{
			var currentSize = size;
			int leafCount = currentSize + 1 - (int)Math.Pow(2, Math.Floor(Math.Log(currentSize + 1, 2)));

			VineToTree_Compression(start, leafCount);
			currentSize -= leafCount;

			while (currentSize > 1)
			{
				VineToTree_Compression(start, currentSize / 2);

				currentSize /= 2;
			}
		}

		private static void VineToTree_Compression(IBinaryTreeNodeStructure<T> start, int count)
		{
			var scanner = start;

			for (int i = 0; i < count; i++)
			{
				var child = scanner.Right;
				scanner.Right = child.Right;
				scanner = scanner.Right;
				child.Right = scanner.Left;
				scanner.Left = child;
			}
		}

		#endregion

		/// <summary>
		/// Finds an item in the tree and returns the node it sits on.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public BinaryTreeNode<T> Find(T item)
		{
			return Find(root, item);
		}

		private BinaryTreeNode<T> Find(BinaryTreeNode<T> node, T item)
		{
			if (node == null)
				return null;

			var comp = comparison(item, node.Value);

			if (comp < 0)
				return Find(node.Left, item);

			if (comp > 0)
				return Find(node.Right, item);

			return node;
		}

		/// <summary>
		/// Removes a node from the tree.
		/// </summary>
		/// <param name="node"></param>
		public void Remove(BinaryTreeNode<T> node)
		{
			Require.ArgumentNotNull(node, nameof(node));
			Require.That<InvalidOperationException>(node?.Owner == this, "Cannot remove a node which does not belong to this tree.");

			try
			{
				if (root == node)
				{
					root = null;
					Count = 0;
				}
				else
				{
					if (node.Parent?.Left == node) node.Parent.Left = null;
					if (node.Parent?.Right == node) node.Parent.Right = null;

					// We just removed this whole section of the tree, so subtract that
					// number from the count.
					Count -= node.Count;
				}

				// Insert the children back in the tree.
				if (node.Left != null) AddNoBalance(node.Left);
				if (node.Right != null) AddNoBalance(node.Right);
			}
			finally
			{
				DoAutoBalance();
			}
		}

		/// <summary>
		/// Removes an item from the tree.
		/// </summary>
		/// <param name="item"></param>
		public bool Remove(T item)
		{
			var node = Find(item);

			if (node == null)
				return false;

			Remove(node);

			return true;
		}

		private void DoAutoBalance()
		{
			if (AutoBalance)
				Balance();
		}

		/// <summary>
		/// Clears the tree.
		/// </summary>
		public void Clear()
		{
			root = null;
			Count = 0;
		}
	}

	/// <summary>
	/// Provides an interface for a binary tree.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IBinaryTree<T> : IReadOnlyBinaryTree<T>
	{
		/// <summary>
		/// Adds an item to the tree.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		BinaryTreeNode<T> Add(T item);

		/// <summary>
		/// Removes an item from the tree.
		/// </summary>
		/// <param name="item"></param>
		bool Remove(T item);
	}

	/// <summary>
	/// Provides a readonly interface to a binary tree.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IReadOnlyBinaryTree<T>
	{
		/// <summary>
		/// Root element in the tree.
		/// </summary>
		BinaryTreeNode<T> Root { get; }

		IEnumerable<BinaryTreeNode<T>> Items { get; }

		/// <summary>
		/// Enumerates the items in the tree in an ascending order.
		/// </summary>
		IEnumerable<BinaryTreeNode<T>> ItemsAscending { get; }

		/// <summary>
		/// Enumerates the items in the tree in a descending order.
		/// </summary>
		IEnumerable<BinaryTreeNode<T>> ItemsDescending { get; }

		/// <summary>
		/// Finds an item in the tree and returns the node it sits on.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		BinaryTreeNode<T> Find(T item);
	}
}
