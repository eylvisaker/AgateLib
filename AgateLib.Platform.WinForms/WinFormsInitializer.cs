using AgateLib.ApplicationModels;
using AgateLib.IO;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform.WinForms.Factories;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Quality;
using AgateLib.Utility;
using System;
using System.IO;
using System.Reflection;

namespace AgateLib.Platform.WinForms
{
	/// <summary>
	/// Initializes AgateLib to use the Windows Forms platform.
	/// </summary>
	public class WinFormsInitializer
	{
		FormsFactory factory;

		public void Initialize(ModelParameters parameters)
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

			Initialize(parameters, Path.GetDirectoryName(Path.GetFullPath(assembly.Location)));
		}
		/// <summary>
		/// Initializes AgateLib.
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="assemblyPath">Path to the folder where the application files reside in.</param>
		public void Initialize(ModelParameters parameters, string appRootPath)
		{
			Condition.Requires<ArgumentNullException>(parameters != null, "parameters");
			Condition.Requires<ArgumentNullException>(appRootPath != null, "appRootPath");

			if (factory == null)
			{
				factory = new FormsFactory(appRootPath);
				Core.Initialize(factory);
			}

			Core.InitAssetLocations(parameters.AssetLocations);

			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			FileProvider.UserFiles = new FileSystemProvider(Path.Combine(appData, parameters.ApplicationName));
		}
	}
}
