using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AgateLib.CompatibilityExtensions
{
	class Enums
	{
		public static Array GetValues(Type enumType)
		{
#if XBOX360
			if (enumType.IsEnum == false)
				throw new AgateException("You must pass an enumeration type.");


			FieldInfo[] fieldInfo = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
			Enum[] vals = new Enum[fieldInfo.Length];

			for (int i = 0; i < vals.Length; ++i)
			{
				vals[i] = (Enum)fieldInfo[i].GetValue(null);
			}

			return vals;
#else
			return Enum.GetValues(enumType);
#endif
		}

	}
}