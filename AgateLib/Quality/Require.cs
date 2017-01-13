using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Quality
{
	public static class Require
	{

		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T param, string paramName) where T : class
		{
			ArgumentNotNull(param, paramName, paramName + " must not be null");
		}

		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T param, string paramName, string message) where T : class
		{
			if (param != null)
				return;

			throw new ArgumentNullException(paramName, message);
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
