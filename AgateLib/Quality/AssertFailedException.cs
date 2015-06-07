using System;

namespace AgateLib.Quality
{
	/// <summary>
	/// Exception thrown when an assert from AgateLib.Quality fails.
	/// </summary>
	public class AssertFailedException : Exception
	{
		/// <summary>
		/// Constructs the exception.
		/// </summary>
		public AssertFailedException()
		{
		}

		/// <summary>
		/// Constructs the exception.
		/// </summary>
		/// <param name="message"></param>
		public AssertFailedException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructs the exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public AssertFailedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}