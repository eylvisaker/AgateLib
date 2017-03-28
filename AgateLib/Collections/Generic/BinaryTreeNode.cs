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
using AgateLib.Quality;

namespace AgateLib.Collections.Generic
{
	/// <summary>
	/// Class representing a single node on the binary tree.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BinaryTreeNode<T> : IBinaryTreeNodeStructure<T>
	{
		private BinaryTreeNode<T> left, right;

		/// <summary>
		/// Constructs a BinaryTreeNode
		/// </summary>
		/// <param name="item"></param>
		public BinaryTreeNode(IBinaryTree<T> owner, T item)
		{
			Require.True(item != null, $"{nameof(item)} must not be null.");
			Require.ArgumentNotNull(owner, nameof(owner));

			this.Value = item;
			this.Owner = owner;
		}

		public IBinaryTree<T> Owner { get; private set; }

		/// <summary>
		/// The value stored in the node.
		/// </summary>
		public T Value { get; private set; }

		/// <summary>
		/// The node to the left.
		/// </summary>
		public BinaryTreeNode<T> Left
		{
			get { return left; }
			internal set
			{
				left = value;

				if (value != null)
				{
					if (value.Parent?.Left == value) value.Parent.Left = null;
					if (value.Parent?.Right == value) value.Parent.Right = null;

					left.Parent = this;
				}
			}
		}

		BinaryTreeNode<T> IBinaryTreeNodeStructure<T>.Left { get { return Left; } set { Left = value; } }

		/// <summary>
		/// The node to the right.
		/// </summary>
		public BinaryTreeNode<T> Right
		{
			get { return right; }
			internal set
			{
				right = value;

				if (value != null)
				{
					if (value.Parent?.Left == value) value.Parent.Left = null;
					if (value.Parent?.Right == value) value.Parent.Right = null;

					right.Parent = this;
				}
			}
		}

		BinaryTreeNode<T> IBinaryTreeNodeStructure<T>.Right { get { return Right; } set { Right = value; } }

		/// <summary>
		/// The parent of this node.
		/// </summary>
		public BinaryTreeNode<T> Parent { get; internal set; }

		/// <summary>
		/// Returns the number of nodes in this section of the tree, including this one.
		/// </summary>
		public int Count => 1 + (Left?.Count ?? 0) + (Right?.Count ?? 0);

		/// <summary>
		/// Gets the depth of this node. The root node has a depth of 0, the children of a node with depth d are at depth d + 1.
		/// </summary>
		public int Depth
		{
			get
			{
				int count = 0;
				var current = Parent;

				while (current != null)
				{
					current = current.Parent;
					count++;
				}

				return count;
			}
		}

		/// <summary>
		/// Gets the maximum depth of the children of this node. If this node has no children, returns the same
		/// value as the Depth property.
		/// </summary>
		public int MaximumDepth => Depth + Math.Max(DepthFrom(Left), DepthFrom(Right));

		private static int DepthFrom(BinaryTreeNode<T> node)
		{
			if (node == null)
				return 0;

			return 1 + Math.Max(DepthFrom(node.Left), DepthFrom(node.Right));
		}

		/// <summary>
		/// Gets the minimum value node from this point down the tree.
		/// </summary>
		/// <returns></returns>
		public BinaryTreeNode<T> Min()
		{
			if (Left == null)
				return this;

			return Left.Min();
		}

		/// <summary>
		/// Gets the maximum value node from this point down the tree.
		/// </summary>
		/// <returns></returns>
		public BinaryTreeNode<T> Max()
		{
			if (Right == null)
				return this;

			return Right.Max();
		}

		/// <summary>
		/// Returns the next value in sequence after this one. Returns null
		/// if this node is the maximum value in the tree.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public BinaryTreeNode<T> Next()
		{
			if (Right != null)
				return Right.Min();

			var search = this;

			while (search?.Parent != null)
			{
				if (search == search.Parent.Left)
					return search.Parent;

				search = search.Parent;
			}

			return search?.Parent;
		}

		/// <summary>
		/// Returns the previous value in the sequence before this one. Returns null
		/// if this node is the minimum value in the tree.
		/// </summary>
		/// <returns></returns>
		public BinaryTreeNode<T> Previous()
		{
			if (Left != null)
				return Left.Max();

			var search = this;

			while (search?.Parent != null)
			{
				if (search == search.Parent.Right)
					return search.Parent;

				search = search.Parent;
			}

			return search?.Parent;
		}

		/// <summary>
		/// Prints debugging output.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string leftValue = Left?.Value.ToString();
			string rightValue = Right?.Value.ToString();

			return $"{Value}: Left({leftValue}) : Right({rightValue})";
		}
	}

	internal interface IBinaryTreeNodeStructure<T>
	{
		BinaryTreeNode<T> Left { get; set; }
		BinaryTreeNode<T> Right { get; set; }
	}
}