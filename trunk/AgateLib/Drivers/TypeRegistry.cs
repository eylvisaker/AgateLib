using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Drivers
{
	public static class TypeRegistry
	{
		public static Type DisplayDriver { get; set; }
		public static Type AudioDriver { get; set; }
		public static Type InputDriver { get; set; }
	}
}
