using AgateLib.ApplicationModels;
using AgateLib.IO;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform.WinForms.Factories;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Utility;
using System;
using System.IO;

namespace AgateLib.Platform.WinForms
{
	static class WinFormsInitializer
	{
		static FormsFactory factory;

		public static void Initialize(ModelParameters parameters)
		{
			if (factory == null)
			{
				factory = new FormsFactory();
				Core.Initialize(factory);
			}

			factory.SetAssetLocations(parameters.AssetLocations);
			Core.InitAssetLocations(parameters.AssetLocations);

			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			FileProvider.UserFiles = new FileSystemProvider(Path.Combine(appData, parameters.ApplicationName));

			////var assetProvider = new FileSystemProvider(System.IO.Path.GetFullPath(parameters.AssetLocations.Path));
			////AgateLib.IO.FileProvider.Initialize(assetProvider, parameters.AssetLocations);

			//System.IO.Directory.SetCurrentDirectory(assetProvider.SearchPath);
		}
	}
}
