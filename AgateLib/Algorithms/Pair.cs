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

namespace AgateLib.Algorithms
{
	/// <summary>
	/// This structure is used instead of the generic .NET Tuple&lt;&gt; classes because
	/// the Tuple classes are immutable, and this is needed internally by the iterating algorithms.
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	[Obsolete("Is this still used? Or should it be replaced?", false)]
	public struct Pair<T1, T2>
	{
		public T1 First;
		public T2 Second;

		public Pair(T1 f, T2 s)
		{
			First = f;
			Second = s;
		}
	}
}