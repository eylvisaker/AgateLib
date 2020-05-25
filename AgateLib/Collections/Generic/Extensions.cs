using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Collections.Generic
{
    public static class Extensions
    {
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            int i = 0;

            foreach (T element in self)
            {
                if (EqualityComparer<T>.Default.Equals(element, elementToFind))
                    return i;
                i++;
            }

            return -1;
        }
    }
}
