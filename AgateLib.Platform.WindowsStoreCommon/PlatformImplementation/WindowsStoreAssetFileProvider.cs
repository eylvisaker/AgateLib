using AgateLib.Platform.WindowsStore.DisplayImplementation;
using SharpDX.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
			if (path.EndsWith("/") == false)
				path += "/";

			this.path = path;

			uriBase = "ms-appx:///" + path;
		}
		public async Task<System.IO.Stream> OpenReadAsync(string filename)
		{
			Uri uri = new Uri(uriBase + filename);

			StorageFile storageFile =
				await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri).AsTask().ConfigureAwait(false);

			var randomAccessStream = await storageFile.OpenReadAsync().AsTask().ConfigureAwait(false);
			
			return randomAccessStream.AsStreamForRead();
		}

		public bool FileExists(string filename)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetAllFiles()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			throw new NotImplementedException();
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
