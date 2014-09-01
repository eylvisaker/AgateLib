using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Resources.Legacy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.Resources
{
	public static class BuiltinResources
	{
		static ZipFileProvider mProvider;
		static AgateResourceCollection mResources;

		static BuiltinResources()
		{
		}

		public static async Task<FontSurface> GetFontAsync(string name)
		{
			if (mProvider == null)
			{
				mProvider = new ZipFileProvider("Fonts.zip", new MemoryStream(Builtin.Fonts));
				mResources = new AgateResourceCollection();
				mResources.FileProvider = mProvider;
				AgateResourceLoader.LoadResources(mResources, await mProvider.OpenReadAsync("Resources.xml"));
			}

			return new FontSurface(mResources, name);
		}
	}
}
