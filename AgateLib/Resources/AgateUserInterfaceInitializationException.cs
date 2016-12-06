using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Resources
{
	public class AgateUserInterfaceInitializationException : AgateResourceException
	{
		public AgateUserInterfaceInitializationException()
		{
		}

		public AgateUserInterfaceInitializationException(string message) : base(message)
		{
		}

		public AgateUserInterfaceInitializationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
