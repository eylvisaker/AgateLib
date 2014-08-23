using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.Platform.WindowsForms.Factories;
using AgateLib.Utility;

namespace AgateLib.Platform.WindowsForms
{
	static class Initializer
	{
		public static void Initialize(FormsModelParameters Parameters)
		{
			Core.Initialize(new FormsFactory());

			var assetProvider = new FileSystemProvider(System.IO.Path.GetFullPath(Parameters.AssetPath));
			AgateLib.IO.FileProvider.Initialize(assetProvider, Parameters.AssetLocations);

			System.IO.Directory.SetCurrentDirectory(assetProvider.SearchPath);
		}
	}
}
