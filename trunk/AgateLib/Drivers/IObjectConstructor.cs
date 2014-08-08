using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Drivers
{
	public interface IObjectConstructor
	{
		object CreateInstance(Type t);
	}
}
