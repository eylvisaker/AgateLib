using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AgateLib.Collections.Generic
{
    public class PoolTest
    {
        class TestResource : IPoolResource
        {
            public event EventHandler Disposed;

            public int InitializeCount { get; private set; }
            public int DisposeCount { get; private set; }

            public void Dispose()
            {
                DisposeCount++;

                Disposed?.Invoke(this, EventArgs.Empty);
            }

            public void Initialize()
            {
                InitializeCount++;
            }
        }

        Pool<TestResource> pool = new Pool<TestResource>(() => new TestResource());

        [Fact]
        public void PoolUseOneObject()
        {
            for(int i = 0; i < 100; i++)
            {
                pool.TryGet(out var item).Should().BeTrue();

                item.InitializeCount.Should().Be(i, $"Initialize test failed on iteration {i}");
                item.DisposeCount.Should().Be(i, $"Dispose test failed on iteration {i}");

                item.Dispose();
            }
        }

        [Fact]
        public void PoolReachMaximum()
        {
            pool = new Pool<TestResource>(() => new TestResource(), 2);

            var a = pool.GetOrDefault();
            var b = pool.GetOrDefault();
            var c = pool.GetOrDefault();

            b.Should().NotBeNull();
            c.Should().BeNull();
        }
    }
}
