//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Quality
{
	[Obsolete("Use methods in Require instead.")]
	public static class Condition
	{
		[Obsolete("Use Requires.True instead.")]
		[DebuggerStepThrough]
		public static void Requires<TE>(bool state) where TE : Exception, new()
		{
			if (state == true)
				return;

			throw new TE();
		}

		/// <summary>
		/// Use Requires.True instead.
		/// </summary>
		/// <typeparam name="TE"></typeparam>
		/// <param name="state"></param>
		[Obsolete("Use Requires.True instead.")]
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

		[Obsolete("Use Require.ArgumentNotNull instead.")]
		[DebuggerStepThrough]
		public static void RequireArgumentNotNull<T>(T param, string paramName) where T : class
		{
			RequireArgumentNotNull(param, paramName, paramName + " must not be null");
		}

		[DebuggerStepThrough]
		[Obsolete("Use Require.ArgumentNotNull instead.")]
		public static void RequireArgumentNotNull<T>(T param, string paramName, string message) where T : class
		{
			if (param != null)
				return;

			throw new ArgumentNullException(paramName, paramName + " must not be null. " + message);
		}

		[DebuggerStepThrough]
		public static void Requires<TE>(bool state, string paramName, string message) where TE : ArgumentException, new()
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

		[Obsolete("Use Require.ForAll instead.")]
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
