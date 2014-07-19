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
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;


namespace AgateLib.Meshes.Loaders.ObjFile
{
	/// <summary>
	/// typed representation of the .obj file
	/// </summary>
	public class OBJFileRepresentation
	{
		private List<List<object>> mFileRepresentation;

		/// <summary>
		/// Construct and parse the .obj representation from a Stream
		/// </summary>
		public OBJFileRepresentation(Stream stream)
		{
			using (StreamReader sr = new StreamReader(stream))
			{
				mFileRepresentation = Parser.Parse(sr);
			}
		}
	}
}
