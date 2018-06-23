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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using AgateLib.Quality;

namespace AgateLib.Collections.Generic
{
    public interface IPoolResource : IDisposable
    {
        event EventHandler Disposed;

        void Initialize();
    }

    public class Pool<T> where T : IPoolResource
    {
        private ConcurrentBag<T> objects = new ConcurrentBag<T>();

        private Func<T> generator;

        private int maxCreation;
        
        public Pool(Func<T> generator, int maxSize = int.MaxValue)
        {
            Require.ArgumentNotNull(generator, nameof(generator));
            Require.That(maxSize > 0, "MaxSize must be positive");

            this.generator = generator;
            this.maxCreation = maxSize;
        }

        /// <summary>
        /// Gets an object from the pool.
        /// </summary>
        /// <returns></returns>
        public bool TryGet(out T result)
        {
            if (objects.TryTake(out result))
            {
                result.Initialize();
                return true;
            }

            if (maxCreation <= 0)
            {
                result = default(T);
                return false;
            }

            maxCreation--;

            var item = generator();

            item.Disposed += (sender, e) => Put(item);

            result = item;

            return true;
        }

        /// <summary>
        /// Gets an object from the pool.
        /// </summary>
        /// <returns></returns>
        public T GetOrDefault()
        {
            if (TryGet(out T result))
            {
                return result;
            }

            return default(T);
        }

        private void Put(T obj)
        {
            objects.Add(obj);
        }
    }
}
