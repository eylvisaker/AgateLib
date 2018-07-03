using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using FluentAssertions;
using Xunit;

namespace AgateLib.Tests.MathematicsTests
{
    public class Vector2ListTests
    {

        [Fact]
        public void V2List_InsertRange()
        {
            Vector2List list = new Vector2List
            {
                { 0, 0 },
                { 1, 1 },
                { 2, 2 },
                { 8, 8 },
                { 9, 9 }
            };

            Vector2List secondList = new Vector2List
            {
                { 3, 3 },
                { 4, 4 },
                { 5, 5 },
                { 6, 6 },
                { 7, 7 }
            };

            list.InsertRange(3, secondList);

            list.Count.Should().Be(10);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].X.Should().Be(i);
                list[i].Y.Should().Be(i);
            }
        }
    }
}
