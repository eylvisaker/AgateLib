using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AgateLib.UserInterface.Css
{
	/// <summary>
	/// Provides extension methods for methods which are missing in the PCL reflection namespace.
	/// </summary>
	public static class ReflectionExtensions
	{
		/// <summary>
		/// Finds and returns the first custom attribute of the specified type.
		/// Returns null if the attribute is not found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="member"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit)
		{
			return (T)member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
		}
	}
}
