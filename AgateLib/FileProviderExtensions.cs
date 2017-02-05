using System;
using System.IO;
using AgateLib.IO;
using AgateLib.Quality;

namespace AgateLib
{
	public static class FileProviderExtensions
	{
		public static Stream OpenRead(this IReadFileProvider provider, string filename)
		{
			Condition.Requires<ArgumentNullException>(provider != null, "provider");
			Condition.Requires<ArgumentNullException>(filename != null, "filename");

			var task = provider.OpenReadAsync(filename);

			return task.GetAwaiter().GetResult();
		}

		public static Stream OpenWrite(this IReadWriteFileProvider provider, string filename, FileOpenMode mode = FileOpenMode.Create)
		{
			Condition.Requires<ArgumentNullException>(provider != null, "provider");
			Condition.Requires<ArgumentNullException>(filename != null, "filename");

			var task = provider.OpenWriteAsync(filename, mode);

			return task.GetAwaiter().GetResult();
		}
	}
}