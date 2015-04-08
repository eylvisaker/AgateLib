using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Quality
{
    public static class Condition
    {
        [DebuggerStepThrough]
        public static void Requires<TE>(bool state) where TE : Exception, new()
        {
            if (state == true)
                return;

            throw new TE();
        }

        [DebuggerStepThrough]
        public static void Requires(bool state)
        {
            if (state == true)
                return;

            throw new InvalidConditionException();
        }

        [DebuggerStepThrough]
        public static void Requires<TE>(bool state, string message) where TE : Exception, new()
        {
            if (state == true)
                return;

            var exception = (TE)Activator.CreateInstance(typeof(TE), message);
            throw exception;
        }

        [DebuggerStepThrough]
        public static void Requires<TE>(bool state, string paramName, string message) where TE: ArgumentException, new ()
        {
            if (state == true)
                return;

            if (typeof(TE) == typeof(ArgumentException))
                throw new ArgumentException(message ?? "The parameter was invalid.", paramName);
            if (typeof(TE) == typeof(ArgumentNullException))
                throw new ArgumentNullException(paramName, message ?? "The parameter must not be null.");
            if (typeof(TE) == typeof(ArgumentOutOfRangeException))
                throw new ArgumentOutOfRangeException(paramName, message ?? "The parameter was out of range.");

            Exception ex = null;

            if (message == null)
                ex = (TE)Activator.CreateInstance(typeof(TE), paramName);
            else
                ex = (TE)Activator.CreateInstance(typeof(TE), paramName, message);

            throw ex;
        }

        [DebuggerStepThrough]
        public static bool ForAll<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            bool state = true;

            foreach (var v in collection)
            {
                state = predicate(v);
                if (state == false)
                    break;
            }

            return state;
        }
    }
}
