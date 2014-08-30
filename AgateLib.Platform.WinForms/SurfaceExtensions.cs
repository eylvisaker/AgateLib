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
