using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Resources;

namespace AgateLib.Platform.WinForms.Resources
{
	public static class BuiltinResources
	{
		static ZipFileProvider mProvider;
		static AgateResourceManager mResources;

		static BuiltinResources()
		{
		}

		public static IFont GetFont(string name)
		{
			if (mProvider == null)
			{
				mProvider = new ZipFileProvider("Fonts.zip", new MemoryStream(Builtin.Fonts));
				mResources = new AgateResourceManager(
					new ResourceDataLoader(mProvider).Load("Fonts.yaml"), 
					mProvider, mProvider);
			}

			return mResources.DisplayResourceManager.FindFont(name);
		}
	}
}
