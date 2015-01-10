using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Quality
{
	public static class Condition
	{
		public static void Requires<TE>(bool state) where TE :Exception, new()
		{
			if (state == true)
				return;

			throw new TE();
		}
		public static void Requires(bool state)
		{
			if (state == true)
				return;

			throw new InvalidConditionException();
		}
		public static void Requires<TE>(bool state, string message) where TE :Exception, new()
		{
			if (state == true)
				return;

			var exception = (TE)Activator.CreateInstance(typeof(TE), message);
			throw exception;
		}

		public static bool ForAll<T>(IEnumerable<T> collection, Func<T, bool> predicate)
		{
			bool state = true;

			foreach(var v in collection)
			{
				state = predicate(v);
				if (state == false)
					break;
			}

			return state;
		}
	}
}
