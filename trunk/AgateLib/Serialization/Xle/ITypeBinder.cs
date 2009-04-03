﻿//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Serialization.Xle
{
	/// <summary>
	/// This interface must be implemented by any class which wants to provide
	/// custom type binding (mapping from a string to a Type object) for the
	/// XleSerializer.
	/// </summary>
	public interface ITypeBinder
	{
		/// <summary>
		/// Returns a Type object representing the type described by the typename parameter.
		/// </summary>
		/// <param name="typename"></param>
		/// <returns></returns>
		Type GetType(string typename);
	}
}
