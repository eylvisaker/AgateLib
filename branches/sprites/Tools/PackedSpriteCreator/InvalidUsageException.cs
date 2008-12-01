using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.PackedSpriteCreator
{
	[global::System.Serializable]
	public class InvalidUsageException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public InvalidUsageException() { }
		public InvalidUsageException(string message) : base(message) { }
		public InvalidUsageException(string message, Exception inner) : base(message, inner) { }
		protected InvalidUsageException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
