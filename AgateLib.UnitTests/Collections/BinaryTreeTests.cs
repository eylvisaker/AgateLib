using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Collections.Generic;
using AgateLib.Quality;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Collections
{
	[TestClass]
	public class BinaryTreeTests
	{
		[TestMethod]
		public void BinaryTree_Empty()
		{
			var tree = new BinaryTree<int>();

			tree.Add(new[] { 1, 2, 3, 4 });
			tree.Clear();

			Assert.AreEqual(0, tree.Count);
			Assert.AreEqual(0, tree.Items.Count());
			Assert.AreEqual(0, tree.ItemsAscending.Count());
			Assert.AreEqual(0, tree.ItemsDescending.Count());
			Assert.IsNull(tree.Find(1));
			Assert.IsNull(tree.Find(-1));
			Assert.IsNull(tree.Find(0));
		}

		[TestMethod]
		public void BinaryTree_Remove()
		{
			var tree = new BinaryTree<int>();

			tree.Add(new[] { 1, 2, 3, 4 });

			Assert.AreEqual(4, tree.Count);

			tree.Remove(3);

			Assert.AreEqual(3, tree.Count);
			CollectionAssert.AreEquivalent(new[] { 1, 2, 4 }, tree.Items.Select(x => x.Value).ToList());
			CollectionAssert.AreEquivalent(new[] { 1, 2, 4 }, tree.ItemsAscending.Select(x => x.Value).ToList());
			CollectionAssert.AreEquivalent(new[] { 1, 2, 4 }, tree.ItemsDescending.Select(x => x.Value).ToList());
			Assert.IsNotNull(tree.Find(1));
			Assert.IsNull(tree.Find(-1));
			Assert.IsNull(tree.Find(0));
			Assert.IsNull(tree.Find(3));
		}

		[TestMethod]
		public void BinaryTree_RemoveRoot()
		{
			var tree = new BinaryTree<int>();

			tree.Add(new[] { 1, 2, 3, 4 });

			Assert.AreEqual(4, tree.Count);

			tree.Remove(1);

			Assert.AreEqual(3, tree.Count);
			CollectionAssert.AreEquivalent(new[] { 2, 3, 4 }, tree.Items.Select(x => x.Value).ToList());
			CollectionAssert.AreEquivalent(new[] { 2, 3, 4 }, tree.ItemsAscending.Select(x => x.Value).ToList());
			CollectionAssert.AreEquivalent(new[] { 2, 3, 4 }, tree.ItemsDescending.Select(x => x.Value).ToList());
			Assert.IsNull(tree.Find(1));
			Assert.IsNull(tree.Find(-1));
			Assert.IsNull(tree.Find(0));
			Assert.IsNotNull(tree.Find(3));
		}

		[TestMethod]
		public void BinaryTree_ThrowIfExists()
		{
			var items = CreateItems(25);
			var tree = CreateTree(items, true);

			AssertX.Throws<ArgumentException>(() => tree.Add(14));
		}

		[TestMethod]
		public void BinaryTree_ItemsCollection()
		{
			const int tests = 25;

			BinaryTreeTester(tests, (testIndex, items, tree) =>
			{
				var result = tree.Items.Select(node => node.Value).ToList();

				Assert.AreEqual(items.Count, tree.Count);
				Assert.AreEqual(items.Count, result.Count);

				CollectionAssert.AreEquivalent(items.ToList(), result,
					$"Equivalence failed for sequence #{testIndex}: {string.Join(",", items.Select(x => x.ToString()))}");
			});
		}

		[TestMethod]
		public void BinaryTree_AscendingOrder()
		{
			const int tests = 25;

			BinaryTreeTester(tests, (testIndex, items, tree) =>
			{
				int expected = 0;

				var result = tree.ItemsAscending.Select(node => node.Value).ToList();

				Assert.AreEqual(items.Count, tree.Count);
				Assert.AreEqual(items.Count, result.Count);

				foreach (var item in result)
				{
					Assert.AreEqual(expected, item,
						$"Order failed for sequence #{testIndex}: {string.Join(",", items.Select(x => x.ToString()))}");

					expected++;
				}

			});
		}

		[TestMethod]
		public void BinaryTree_DescendingOrder()
		{
			const int tests = 25;

			BinaryTreeTester(tests, (testIndex, items, tree) =>
			{
				int expected = items.Max();

				var result = tree.ItemsDescending.Select(node => node.Value).ToList();

				Assert.AreEqual(items.Count, tree.Count);
				Assert.AreEqual(items.Count, result.Count);

				foreach (var item in result)
				{
					Assert.AreEqual(expected, item,
						$"Order failed for sequence #{testIndex}: {string.Join(",", items.Select(x => x.ToString()))}");

					expected--;
				}
			});
		}

		[TestMethod]
		public void BinaryTree_Next()
		{
			const int tests = 25;

			BinaryTreeTester(tests, (testIndex, items, tree) =>
			{
				int expected = 0;

				var node = tree.Find(expected);

				while (node != null)
				{
					Console.WriteLine(node.ToString());
					Assert.AreEqual(expected, node.Value);

					node = node.Next();
					expected++;
				}

				// Check if we went all the way through the array.
				Assert.AreEqual(items.Count, expected);
			});
		}

		[TestMethod]
		public void BinaryTree_Previous()
		{
			const int tests = 25;

			BinaryTreeTester(tests, (testIndex, items, tree) =>
			{
				int expected = items.Max();

				var node = tree.Find(expected);

				while (node != null)
				{
					Console.WriteLine(node.ToString());
					Assert.AreEqual(expected, node.Value);

					node = node.Previous();
					expected--;
				}

				// Check if we went all the way through the array.
				Assert.AreEqual(-1, expected);
			});
		}

		private void BinaryTreeTester(int tests, Action<int, IList<int>, BinaryTree<int>> test)
		{
			const int seed = 2; // seed chosen to make the first test have a large number of items.

			// Always generate the same sequences so the unit test is reliable.
			var random = new Random(2);
			bool randomize = false;
			bool balance = false;

			for (int i = 0; i < tests; i++)
			{
				int size = 5 + random.Next(32);

				var items = CreateItems(size, randomize, random);
				var tree = CreateTree(items, balance);

				test(i, items, tree);

				// First two iteration are done without randomization.
				randomize |= balance;
				balance = !balance;
			}
		}

		private static IList<int> CreateItems(int count, bool randomize = false, Random random = null)
		{
			random = random ?? new System.Random(2);

			var list = Enumerable.Range(0, count).ToList();

			if (randomize)
			{
				list.Randomize(random);
			}

			return list;
		}

		private static BinaryTree<int> CreateTree(IEnumerable<int> array, bool balance)
		{
			BinaryTree<int> tree = new BinaryTree<int>();

			tree.AutoBalance = balance;
			tree.Add(array);

			Assert.AreEqual(tree.Count, tree.Root.Count);

			if (balance)
			{
				Assert.IsTrue(tree.IsBalanced, "Autobalance enabled but the tree is not balanced.");
			}

			return tree;
		}
	}
}
