using AgateLib.Collections.Generic;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AgateLib.UnitTests.Collections
{
    public class ListExtensions
    {
        [Fact]
        public void SortPrimitives()
        {
            List<int> li = new List<int> { 1, 6, 2, 3, 8, 10, 9, 7, 4, 5 };

            li.Count.Should().Be(10, "the numbers 1-10 should in the test list.");

            li.InsertionSort();

            for (int i = 0; i < li.Count; i++)
            {
                li[i].Should().Be(i + 1);
            }
        }

        [Fact]
        public void InsertionSortTest()
        {
            List<int> list = new List<int> { 4, 2, 3, 1, 6, 7, 8, 9 };

            list.InsertionSort();

            list[0].Should().Be(1);
            list[list.Count - 1].Should().Be(9);
        }
    }
}
