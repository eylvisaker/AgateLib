using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;

namespace AgateLib.Configuration.State
{
	class IOState
	{
		internal IReadFileProvider mAssetProvider;
		internal IReadFileProvider mImages;
		internal IReadFileProvider mResources;
		internal IReadFileProvider mMusic;
		internal IReadFileProvider mSounds;
		internal IReadFileProvider mUserInterfaceAssets;
		internal IReadWriteFileProvider UserFiles;
	}
}
