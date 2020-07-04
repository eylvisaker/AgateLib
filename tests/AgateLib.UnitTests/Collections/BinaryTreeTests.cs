using AgateLib.Collections.Generic;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AgateLib.UnitTests.Collections
{
    public class BinaryTreeTests
    {
        [Fact]
        public void BinaryTree_Empty()
        {
            var tree = new BinaryTree<int>();

            tree.Add(new[] { 1, 2, 3, 4 });
            tree.Clear();

            tree.Count.Should().Be(0);
            tree.Items.Count().Should().Be(0);
            tree.ItemsAscending.Count().Should().Be(0);
            tree.ItemsDescending.Count().Should().Be(0);
            tree.Find(1).Should().BeNull();
            tree.Find(-1).Should().BeNull();
            tree.Find(0).Should().BeNull();
        }

        [Fact]
        public void BinaryTree_Remove()
        {
            var tree = new BinaryTree<int>();

            tree.Add(new[] { 1, 2, 3, 4 });

            tree.Count.Should().Be(4);

            tree.Remove(3);

            tree.Count.Should().Be(3);

            tree.Items.Select(x => x.Value).ToList().Should().BeEquivalentTo(new[] { 1, 2, 4 });
            tree.ItemsAscending.Select(x => x.Value).ToList().Should().BeEquivalentTo(new[] { 1, 2, 4 });
            tree.ItemsDescending.Select(x => x.Value).ToList().Should().BeEquivalentTo(new[] { 1, 2, 4 });

            tree.Find(1).Should().NotBeNull();
            tree.Find(-1).Should().BeNull();
            tree.Find(0).Should().BeNull();
            tree.Find(3).Should().BeNull();
        }

        [Fact]
        public void BinaryTree_RemoveRoot()
        {
            var tree = new BinaryTree<int>();

            tree.Add(new[] { 1, 2, 3, 4 });

            tree.Count.Should().Be(4);

            tree.Remove(1);

            tree.Count.Should().Be(3);

            tree.Items.Select(x => x.Value).ToList().Should().BeEquivalentTo(new[] { 2, 3, 4 });
            tree.ItemsAscending.Select(x => x.Value).ToList().Should().BeEquivalentTo(new[] { 2, 3, 4 });
            tree.ItemsDescending.Select(x => x.Value).ToList().Should().BeEquivalentTo(new[] { 2, 3, 4 });

            tree.Find(1).Should().BeNull();
            tree.Find(-1).Should().BeNull();
            tree.Find(0).Should().BeNull();
            tree.Find(3).Should().NotBeNull();
        }

        [Fact]
        public void BinaryTree_ThrowIfExists()
        {
            var items = CreateItems(25);
            var tree = CreateTree(items, true);

            new Action(() => tree.Add(14)).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void BinaryTree_ItemsCollection()
        {
            const int tests = 25;

            BinaryTreeTester(tests, (testIndex, items, tree) =>
            {
                var result = tree.Items.Select(node => node.Value).ToList();

                tree.Count.Should().Be(items.Count);
                result.Count.Should().Be(items.Count);

                result.Should().BeEquivalentTo(items.ToList(),
                    $"Equivalence failed for sequence #{testIndex}: {string.Join(",", items.Select(x => x.ToString()))}");
            });
        }

        [Fact]
        public void BinaryTree_AscendingOrder()
        {
            const int tests = 25;

            BinaryTreeTester(tests, (testIndex, items, tree) =>
            {
                int expected = 0;

                var result = tree.ItemsAscending.Select(node => node.Value).ToList();

                tree.Count.Should().Be(items.Count);
                result.Count.Should().Be(items.Count);

                foreach (int item in result)
                {
                    item.Should().Be(expected,
                        $"Order failed for sequence #{testIndex}: {string.Join(",", items.Select(x => x.ToString()))}");

                    expected++;
                }
            });
        }

        [Fact]
        public void BinaryTree_DescendingOrder()
        {
            const int tests = 25;

            BinaryTreeTester(tests, (testIndex, items, tree) =>
            {
                int expected = items.Max();

                var result = tree.ItemsDescending.Select(node => node.Value).ToList();

                tree.Count.Should().Be(items.Count);
                result.Count.Should().Be(items.Count);

                foreach (var item in result)
                {
                    item.Should().Be(expected,
                        $"Order failed for sequence #{testIndex}: {string.Join(",", items.Select(x => x.ToString()))}");

                    expected--;
                }
            });
        }

        [Fact]
        public void BinaryTree_Next()
        {
            const int tests = 25;

            BinaryTreeTester(tests, (testIndex, items, tree) =>
            {
                int expected = 0;

                var node = tree.Find(expected);

                while (node != null)
                {
                    //Console.WriteLine(node.ToString());

                    node.Value.Should().Be(expected);

                    node = node.Next();
                    expected++;
                }

                // Check if we went all the way through the array.
                expected.Should().Be(items.Count);
            });
        }

        [Fact]
        public void BinaryTree_Previous()
        {
            const int tests = 25;

            BinaryTreeTester(tests, (testIndex, items, tree) =>
            {
                int expected = items.Max();

                var node = tree.Find(expected);

                while (node != null)
                {
                    //Console.WriteLine(node.ToString());

                    node.Value.Should().Be(expected);

                    node = node.Previous();
                    expected--;
                }

                // Check if we went all the way through the array.
                expected.Should().Be(-1);
            });
        }

        private void BinaryTreeTester(int tests, Action<int, IList<int>, BinaryTree<int>> test)
        {
            const int seed = 2; // seed chosen to make the first test have a large number of items.

            // Always generate the same sequences so the unit test is reliable.
            var random = new Random(seed);
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
            BinaryTree<int> tree = new BinaryTree<int>
            {
                AutoBalance = balance
            };
            tree.Add(array);

            tree.Root.Count.Should().Be(tree.Count);

            if (balance)
            {
                tree.IsBalanced.Should().BeTrue("Autobalance enabled but the tree is not balanced.");
            }

            return tree;
        }
    }
}
