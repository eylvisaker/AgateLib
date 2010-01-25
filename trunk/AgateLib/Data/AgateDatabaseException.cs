using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	[global::System.Serializable]
	public class AgateDatabaseException : AgateException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public AgateDatabaseException() { ErrorCount = 1; }
		public AgateDatabaseException(string message) : base(message) { ErrorCount = 1; }
		public AgateDatabaseException(string message, Exception inner) : base(message, inner) { ErrorCount = 1; }
		public AgateDatabaseException(string format, params object[] args)
			: base(format, args)
		{ ErrorCount = 1; }
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

		protected AgateDatabaseException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		internal int ErrorCount { get; set; }
	}
}
