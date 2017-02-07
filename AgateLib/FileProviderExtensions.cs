using System;
using System.IO;
using AgateLib.IO;
using AgateLib.Quality;

namespace AgateLib
{
	/// <summary>
	/// Provides extensions for file provider objects.
	/// </summary>
	public static class FileProviderExtensions
	{
		/// <summary>
		/// Synchronous opening of files for reading.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static Stream OpenRead(this IReadFileProvider provider, string filename)
		{
			Condition.Requires<ArgumentNullException>(provider != null, "provider");
			Condition.Requires<ArgumentNullException>(filename != null, "filename");

			var task = provider.OpenReadAsync(filename);

			return task.GetAwaiter().GetResult();
		}

		/// <summary>
		/// Synchronous opening of files for writing.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="filename"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static Stream OpenWrite(this IReadWriteFileProvider provider, string filename, FileOpenMode mode = FileOpenMode.Create)
		{
			Condition.Requires<ArgumentNullException>(provider != null, "provider");
			Condition.Requires<ArgumentNullException>(filename != null, "filename");

			var task = provider.OpenWriteAsync(filename, mode);

			return task.GetAwaiter().GetResult();
		}
	}
}