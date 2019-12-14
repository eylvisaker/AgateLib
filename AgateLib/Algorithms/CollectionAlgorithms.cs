using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Algorithms
{
    public static class CollectionAlgorithms
    {
        /// <summary>
        /// Returns the object in the list which maximizes the specified function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The list of items to pick from.</param>
        /// <param name="func">The function to maximize.</param>
        /// <returns></returns>
        public static T Maximize<T>(this IEnumerable<T> items, Func<T, double> func)
        {
            T result = default(T);
            double max = double.MinValue;
            bool any = false;

            foreach (T item in items)
            {
                any = true;
                double val = func(item);

                if (val > max)
                {
                    max = val;
                    result = item;
                }
            }

            if (!any)
                throw new ArgumentException($"No items in collection", nameof(items));

            return result;
        }

        /// <summary>
        /// Returns the object in the list which minimizes the specified function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The list of items to pick from.</param>
        /// <param name="func">The function to minimize.</param>
        /// <returns></returns>
        public static T Minimize<T>(this IEnumerable<T> items, Func<T, double> func)
            => Maximize(items, x => -func(x));
    }
}
