//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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
