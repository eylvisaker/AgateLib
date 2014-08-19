//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
