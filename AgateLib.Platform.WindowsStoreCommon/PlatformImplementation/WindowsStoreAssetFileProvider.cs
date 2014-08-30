using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace AgateLib.Platform.WindowsStore.PlatformImplementation
{
	public class WindowsStoreAssetFileProvider : IReadFileProvider
	{
		string path;

		public WindowsStoreAssetFileProvider(string path)
		{
			if (path.EndsWith("/") == false)
				path += "/";

			this.path = path;
		}
		public System.IO.Stream OpenRead(string filename)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public bool IsRealFile(string filename)
		{
			throw new NotImplementedException();
		}

		public string ResolveFile(string filename)
		{
			return path + filename;
		}
	}
}
