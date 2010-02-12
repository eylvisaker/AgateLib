using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	/// <summary>
	/// Exception which is thrown if there is an error when working with the database.
	/// </summary>
	[global::System.Serializable]
	public class AgateDatabaseException : AgateException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		public AgateDatabaseException()
		{
			ErrorCount = 1;
		}
		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		/// <param name="message"></param>
		public AgateDatabaseException(string message)
			: base(message)
		{
			ErrorCount = 1;
		}
		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public AgateDatabaseException(string message, Exception inner)
			: base(message, inner)
		{
			ErrorCount = 1;
		}
		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public AgateDatabaseException(string format, params object[] args)
			: base(format, args)
		{
			ErrorCount = 1;
		}
		internal AgateDatabaseException(int errorCount, string message)
			: base(message)
		{
			ErrorCount = errorCount;
		}
		internal AgateDatabaseException(int errorCount, string format, params object[] args)
			: base(format, args)
		{
			ErrorCount = errorCount;
		}

#if !XNA
		/// <summary>
		/// Constructs a database exception.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected AgateDatabaseException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
#endif

		internal int ErrorCount { get; set; }
	}
}
