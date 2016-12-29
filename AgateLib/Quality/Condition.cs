//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
		public static void RequireArgumentNotNull<T>(T param, string paramName) where T : class
		{
			RequireArgumentNotNull(param, paramName, paramName + " must not be null");
		}

		[DebuggerStepThrough]
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
