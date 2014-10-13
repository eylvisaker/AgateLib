using AgateLib.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStore.PlatformImplementation
{
	class IsolatedStorageFileProvider : IReadWriteFileProvider
	{
		public async Task<Stream> OpenWriteAsync(string file)
		{
			return await Task.Run(() => new MemoryStream());
		}

		public void CreateDirectory(string folder)
		{
		}

		public async Task<Stream> OpenReadAsync(string filename)
		{
			return await Task.Run(() => new MemoryStream());
		}

		public bool FileExists(string filename)
		{
			return false;
		}

		public IEnumerable<string> GetAllFiles()
		{
			yield break;
		}

		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			yield break;
		}

		public string ReadAllText(string filename)
		{
			return "";
		}

		public bool IsRealFile(string filename)
		{
			return true;
		}

		public string ResolveFile(string filename)
		{
			return filename;
		}

		public bool IsLogicalFilesystem
		{
			get { return false; }
		}
	}
}
