using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Microsoft.Xna.Framework;
using Moq;

namespace AgateLib.Collections.Generic
{
    public class ListSortingTests
    {
        public class SortObj
        {
            public int Rank { get; set; }
            public string Identity { get; set; }

            public override string ToString() => Identity;
        }

        class ComparableObj : IComparable<ComparableObj>
        {
            public ComparableObj(int rank)
            {
                Rank = rank;
            }

            public int Rank { get; }

            public int CompareTo(ComparableObj other)
            {
                return Rank.CompareTo(other.Rank);
            }
        }

        class LooseComparableObj : IComparable
        {
            public LooseComparableObj(int rank)
            {
                Rank = rank;
            }

            public int Rank { get; }

            public int CompareTo(object obj)
            {
                var other = obj as LooseComparableObj;
                return Rank.CompareTo(other.Rank);
            }
        }

        [Fact]
        public void InsertionSortComparableObjects()
        {
            VerifySortStability(list => list.InsertionSort(),
                i => new ComparableObj(i),
                obj => obj.Rank);

            VerifySortStability(list => list.InsertionSort(),
                i => new LooseComparableObj(i),
                obj => obj.Rank);

            var comparer = new Mock<IComparer<SortObj>>();
            comparer.Setup(x => x.Compare(It.IsAny<SortObj>(), It.IsAny<SortObj>()))
                .Returns<SortObj, SortObj>((x, y) => x.Rank.CompareTo(y.Rank));

            VerifySortStability(list => list.InsertionSort(comparer.Object),
                i => new SortObj { Rank = i, Identity = i.ToString() },
                obj => obj.Rank);
        }

        [Fact]
        public void InsertionSortRequiresComparison()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(3, 4));

            list.Invoking(x => x.InsertionSort()).Should().ThrowExactly<InvalidOperationException>(
                "Sorting should require a comparison method.");
        }

        [Fact]
        public void InsertionSortIsStable()
        {
            Comparison<SortObj> comparison = (x, y) => x.Rank.CompareTo(y.Rank);

            VerifySortStability(list => list.InsertionSort(comparison));
        }

        [Fact]
        public void RandomizeList()
        {
            List<SortObj> list1 = new List<SortObj>();

            for(int i = 0; i < 20; i++)
            {
                list1.Add(new SortObj { Rank = i });
            }

            List<SortObj> list2 = new List<SortObj>(list1);

            Random rnd1 = new Random(20);
            list1.Randomize(rnd1);

            Random rnd2 = new Random(30);
            list2.Randomize(rnd2);

            list2.Should().BeEquivalentTo(list1, "both lists should have same itesm");

            int differentObjects = 0;

            for(int i = 0; i < list1.Count; i++)
            {
                if (!ReferenceEquals(list1[i], list2[i]))
                    differentObjects++;
            }

            differentObjects.Should().BeGreaterThan(1, "randomized lists should not be same order");
        }

        private void VerifySortStability(Action<List<SortObj>> sorter)
        {
            VerifySortStability(
                sorter,
                i => new SortObj { Rank = i, Identity = i.ToString() },
                obj => obj.Rank);
        }

        private void VerifySortStability<T>(Action<List<T>> sorter, Func<int, T> initializer, Expression<Func<T, int>> rank)
        {
            List<T> list = new List<T>();

            Random random = new Random(20);

            for (int i = 0; i < 20; i++)
            {
                list.Add(initializer(random.Next(5)));
            }

            sorter(list);
            list.Should().BeInAscendingOrder(rank);

            List<T> preservedOrder = new List<T>();
            preservedOrder.AddRange(list);

            sorter(list);
            list.Should().BeEquivalentTo(preservedOrder, opt => opt.WithStrictOrdering());
        }
    }
}
