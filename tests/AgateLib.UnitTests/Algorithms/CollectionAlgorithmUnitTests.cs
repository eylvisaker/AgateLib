using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AgateLib.Algorithms
{
    public class CollectionAlgorithmUnitTests
    {
        private List<int> items = new List<int> { 4, 8, 12, 11, -9, 6, -4, 3 };

        [Fact]
        public void MaximizeThrowsIfCollectionIsEmpty() => new Action(() => new List<int>().Maximize(x => x)).Should().Throw<ArgumentException>();

        [Fact]
        public void Maximize() => items.Maximize(x => 2 * x).Should().Be(12);

        [Fact]
        public void Minimize() => items.Minimize(x => 2 * x).Should().Be(-9);
    }
}
