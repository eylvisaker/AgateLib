using AgateLib.Platform.WindowsStore.DisplayImplementation;
using SharpDX.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace AgateLib.Platform.WindowsStore.PlatformImplementation
{
	public class WindowsStoreAssetFileProvider : IReadFileProvider
	{
		string path;
		string uriBase;

		public WindowsStoreAssetFileProvider(string path)
		{
			if (path == ".")
				uriBase = "ms-appx:///";
			else
			{
				if (path.EndsWith("/") == false)
					path += "/";

				this.path = path;

				uriBase = "ms-appx:///" + path;
			}
		}
		public async Task<System.IO.Stream> OpenReadAsync(string filename)
		{
			Uri uri = new Uri(uriBase + filename);

			StorageFile storageFile =
				await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri).AsTask().ConfigureAwait(false);

			var randomAccessStream = await storageFile.OpenReadAsync().AsTask().ConfigureAwait(false);

			return randomAccessStream.AsStreamForRead();
		}

		public override string ToString()
		{
			return uriBase;
		}
		public bool FileExists(string filename)
		{
			Uri uri = new Uri(uriBase + filename);

			try
			{
				StorageFile storageFile =
					Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri).AsTask().Result;

				return true;
			}
			catch (AggregateException e)
			{
				if (e.InnerException is FileNotFoundException)
					return false;

				throw;
			}

		}

		IEnumerable<StorageFile> FilesInDirectory(string searchPattern, StorageFolder folder)
		{
			return FilesInDirectory(CreateRegex(searchPattern), folder);
		}
		private Regex CreateRegex(string searchPattern)
		{
			var regex = new Regex(searchPattern
				.Replace(".", @"\.")
				.Replace("?", ".")
				.Replace("*", ".*"));

			return regex;
		}
		IEnumerable<StorageFile> FilesInDirectory(Regex searchPattern, StorageFolder folder)
		{
			var files = folder.GetFilesAsync().AsTask().Result;
			
			foreach (var file in files)
			{
				if (PatternMatches(searchPattern, file))
					yield return file;
			}

			var folders = folder.GetFoldersAsync().AsTask().Result;

			foreach (var dir in folders)
			{
				foreach (var file in FilesInDirectory(searchPattern, dir))
				{
					if (PatternMatches(searchPattern, file))
						yield return file;
				}
			}
		}

		private bool PatternMatches(Regex searchPattern, StorageFile file)
		{
			return searchPattern.IsMatch(file.Path);
		}

		public IEnumerable<string> GetAllFiles()
		{
			return GetAllFiles("*");
		}

		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			foreach(var file in FilesInDirectory(searchPattern, ApplicationData.Current.LocalFolder))
				yield return file.Path;
		}

		public string ReadAllText(string filename)
		{
			return NativeFile.ReadAllText(path + filename);
		}

		public bool IsRealFile(string filename)
		{
			throw new NotImplementedException();
		}

		public string ResolveFile(string filename)
		{
			return path + filename;
		}


		public bool IsLogicalFilesystem
		{
			get { return true; }
		}
	}
}
