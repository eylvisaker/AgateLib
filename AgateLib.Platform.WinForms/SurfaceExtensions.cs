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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms
{
	public static class SurfaceExtensions
	{
		/// <summary>
		/// Returns a value in the ImageFileFormat enum based on the file
		/// extension of the given filename.  No checks are done to see
		/// if that file exists.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static ImageFileFormat FormatFromExtension(string filename)
		{
			string ext = Path.GetExtension(filename);

			switch (ext)
			{
				case ".bmp":
					return ImageFileFormat.Bmp;

				case ".jpg":
				case ".jpe":
				case ".jpeg":
					return ImageFileFormat.Jpg;

				case ".tga":
					return ImageFileFormat.Tga;

				case ".png":
				default:
					return ImageFileFormat.Png;
			}

		}


		/// <summary>
		/// Saves the surface to the specified file.
		/// 
		/// Infers the file format from the extension.  If there
		/// is no extension present or it is unrecognized, PNG is
		/// assumed.
		/// </summary>
		/// <param name="filename">File name to save to.</param>
		public static void SaveTo(this Surface surf, string filename)
		{
			surf.SaveTo(filename, FormatFromExtension(filename));
		}
	}
}
