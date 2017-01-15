﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Quality
{
	public static class Require
	{
		/// <summary>
		/// Throws an ArgumentNull Exception if the specified
		/// argument is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="param"></param>
		/// <param name="paramName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T param, string paramName) where T : class
		{
			ArgumentNotNull(param, paramName, paramName + " must not be null");
		}

		/// <summary>
		/// Throws an ArgumentNull Exception if the specified
		/// argument is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="param"></param>
		/// <param name="paramName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T param, string paramName, string message) where T : class
		{
			if (param != null)
				return;

			throw new ArgumentNullException(paramName, message);
		}

		/// <summary>
		/// Throws an exception if the value of state is false.
		/// </summary>
		/// <typeparam name="TE"></typeparam>
		/// <param name="state">If this value is false, an exception is thrown.</param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void True<TE>(bool state, string message)
			where TE : Exception, new()
		{
			if (state)
				return;

			var exception = (TE)Activator.CreateInstance(typeof(TE), message);
			throw exception;
		}

		/// <summary>
		/// Throws an exception if the value of state is true.
		/// </summary>
		/// <typeparam name="TE"></typeparam>
		/// <param name="state">If this value is true, an exception is thrown.</param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void False<TE>(bool state, string message)
			where TE : Exception, new()
		{
			if (!state)
				return;

			var exception = (TE)Activator.CreateInstance(typeof(TE), message);
			throw exception;
		}
	}
}
