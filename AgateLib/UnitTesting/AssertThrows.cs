using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTesting
{
	public static class AssertThrows
	{
		public static void Throws<T>(Action expression) where T:Exception
		{
			try
			{
				expression();
			}
			catch (T)
			{
				return;
			}

			throw new Exception("Expression did not throw " + typeof(T).Name);
		}
	}
}
