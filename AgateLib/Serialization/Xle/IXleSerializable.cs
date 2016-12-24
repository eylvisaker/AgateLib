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
using System.Text;

namespace AgateLib.Serialization.Xle
{
	/// <summary>
	/// Interface that must be implemented in order to make an object serializable with
	/// the XleSerializer.
	/// </summary>
	[Obsolete("Don't use Xle serializer.")]
	public interface IXleSerializable
	{
		/// <summary>
		/// This method is called when an object is to be serialized.  The object should
		/// call the Write() methods on the info object in order to write enough of its data
		/// so that it can be deserialized.
		/// </summary>
		/// <param name="info"></param>
		void WriteData(XleSerializationInfo info);
		/// <summary>
		/// This method is called when an object is to be deserialized.  The object should
		/// call the Read*() methods on the info object to reconstruct its internal structure.
		/// </summary>
		/// <param name="info"></param>
		void ReadData(XleSerializationInfo info);
	}
}
