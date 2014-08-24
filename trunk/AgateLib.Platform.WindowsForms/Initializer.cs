using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.Platform.WindowsForms.Factories;
using AgateLib.Utility;

namespace AgateLib.Platform.WindowsForms
{
	static class Initializer
	{
		public static void Initialize(FormsModelParameters parameters)
		{
			Core.Initialize(new FormsFactory(), parameters.AssetLocations);

			var assetProvider = new FileSystemProvider(System.IO.Path.GetFullPath(parameters.AssetPath));
			AgateLib.IO.FileProvider.Initialize(assetProvider, parameters.AssetLocations);

			System.IO.Directory.SetCurrentDirectory(assetProvider.SearchPath);
		}
	}
}
