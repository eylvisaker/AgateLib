using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests
{
    public class SizeFTests
    {
        [Fact]
        public void SizeF_ToVector2()
        {
            var result = Vector2.UnitX + (Vector2)new SizeF(10, 12);

            result.Should().Be(new Vector2(11, 12));
        }
    }
}
