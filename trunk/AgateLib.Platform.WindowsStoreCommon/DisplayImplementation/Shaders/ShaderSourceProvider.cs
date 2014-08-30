using SharpDX.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;

namespace AgateLib.Platform.WindowsStore.DisplayImplementation.Shaders
{
	static class ShaderSourceProvider
	{
		private static byte[] ReadAllBytes(string path)
		{
			var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
			var file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///AgateLib.Platform.WindowsStoreCommon/" + path)).AsTask().Result;// folder.GetFileAsync(path).AsTask().Result;

			var buffer = Windows.Storage.FileIO.ReadBufferAsync(file).AsTask().Result;

			return buffer.ToArray();
		}

		public static byte[] Basic2Dpixel
		{
			get { return ReadAllBytes(@"Resources/Basic2Dpixel.fxo"); }
		}
		public static byte[] Basic2Dvert
		{
			get { return ReadAllBytes(@"Resources/Basic2Dvert.fxo"); }
		}
	}
}
