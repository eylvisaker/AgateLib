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
		/// <summary>
		/// Throws an ArgumentNull Exception if the specified
		/// argument is null.
		/// </summary>
		/// <typeparam name="T">The type of the argument.</typeparam>
		/// <param name="param">The argument which should not be null.</param>
		/// <param name="paramName">The nameof(param).</param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T param, string paramName) where T : class
		{
			ArgumentNotNull(param, paramName, paramName + " must not be null");
		}

		/// <summary>
		/// Throws an ArgumentNull Exception if the specified
		/// argument is null.
		/// </summary>
		/// <typeparam name="T">The type of the argument.</typeparam>
		/// <param name="param">The argument which should not be null.</param>
		/// <param name="paramName">The nameof(param).</param>
		/// <param name="message">Message of the exception should param be null.</param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T param, string paramName, string message) where T : class
		{
			if (param != null)
				return;

			throw new ArgumentNullException(paramName, message);
		}

		/// <summary>
		/// Throws an ArgumentNull Exception if the specified
		/// argument is null.
		/// </summary>
		/// <typeparam name="T">The type of the argument.</typeparam>
		/// <param name="param">The argument which should not be null.</param>
		/// <param name="paramName">The nameof(param).</param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T? param, string paramName) where T : struct
		{
			ArgumentNotNull(param, paramName, paramName + " must not be null");
		}

		/// <summary>
		/// Throws an ArgumentOutOfRangeException if the condition is not met.
		/// </summary>
		/// <param name="condition">If false, this method throws an exception.</param>
		/// <param name="paramName">Name of the parameter.</param>
		/// <param name="message">Message for the exception.</param>
		[DebuggerStepThrough]
		public static void ArgumentInRange(bool condition, string paramName, string message)
		{
			if (condition)
				return;

			throw new ArgumentOutOfRangeException(paramName, message);
		}
		/// <summary>
		/// Throws an ArgumentNull Exception if the specified
		/// argument is null.
		/// </summary>
		/// <typeparam name="T">The type of the argument.</typeparam>
		/// <param name="param">The argument which should not be null.</param>
		/// <param name="paramName">The nameof(param).</param>
		/// <param name="message">Message of the exception should param be null.</param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(T? param, string paramName, string message) where T : struct
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
