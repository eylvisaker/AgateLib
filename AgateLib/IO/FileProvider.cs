//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ApplicationModels;
using AgateLib.Drivers;
using AgateLib.Quality;

namespace AgateLib.IO
{
	public static class FileProvider
	{
		public static void Initialize(IReadFileProvider assetProvider, AssetLocations assetLocations)
		{
            Condition.Requires<ArgumentNullException>(assetProvider != null, "assetProvider");

			AgateLib.IO.Assets.AddAssetLocations(assetProvider, assetLocations);
		}

		[Obsolete("Use AgateLib.IO.Assets instead.")]
		public static IReadFileProvider Assets { get { return AgateLib.IO.Assets.AssetProvider; } }

		[Obsolete("Use AgateLib.IO.Assets.Surfaces instead.")]
		public static IReadFileProvider SurfaceAssets { get { return AgateLib.IO.Assets.Images; } }
		[Obsolete("Use AgateLib.IO.Assets.Resources instead.")]
		public static IReadFileProvider ResourceAssets { get { return AgateLib.IO.Assets.Resources; } }
		[Obsolete("Use AgateLib.IO.Assets.Music instead.")]
		public static IReadFileProvider MusicAssets { get { return AgateLib.IO.Assets.Music; } }
		[Obsolete("Use AgateLib.IO.Assets.Sounds instead.")]
		public static IReadFileProvider SoundAssets { get { return AgateLib.IO.Assets.Sounds; } }
		[Obsolete("Use AgateLib.IO.Assets.UserInterfaceAssets instead.")]
		public static IReadFileProvider UserInterfaceAssets { get { return AgateLib.IO.Assets.UserInterfaceAssets; } }

		public static IReadWriteFileProvider UserFiles { get; set; }


	}
}
