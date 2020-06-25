using AgateLib.Quality;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgateLib.Randomizer
{
    public static class RandomSets
    {
        /// <summary>
        /// Picks a single item from the list at random.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T PickOne<T>(this IReadOnlyCollection<T> list, IRandom random)
        {
            Require.ArgumentNotNull(list, nameof(list));
            Require.ArgumentNotNull(random, nameof(random));

            return random.PickOne(list);
        }

        /// <summary>
        /// Picks a single item from the list at random. If the list is null or empty,
        /// returns the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="random"></param>
        /// <param name="defaultValue">Default value to be returned if the list is null or empty.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T PickOne<T>(this IReadOnlyCollection<T> list, IRandom random, T defaultValue)
        {
            if (list == null || list.Count == 0)
            {
                return defaultValue;
            }

            Require.ArgumentNotNull(list, nameof(list));
            Require.ArgumentNotNull(random, nameof(random));

            return random.PickOne(list);
        }

        [DebuggerStepThrough]
        public static T PickOneWeighted<T>(this IReadOnlyCollection<T> list, IRandom random, Func<T, int> weightFunc)
        {
            Require.ArgumentNotNull(list, nameof(list));
            Require.ArgumentNotNull(random, nameof(random));
            Require.ArgumentNotNull(weightFunc, nameof(weightFunc));

            return random.PickOneWeighted(list, weightFunc);
        }
    }
}
