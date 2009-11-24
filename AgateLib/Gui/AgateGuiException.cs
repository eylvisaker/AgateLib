using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;

namespace AgateLib.Gui
{
	[global::System.Serializable]
	public class AgateGuiException : AgateException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public AgateGuiException() { }
		public AgateGuiException(string message) : base(message) { }
		public AgateGuiException(string message, Exception inner) : base(message, inner) { }
		protected AgateGuiException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}
