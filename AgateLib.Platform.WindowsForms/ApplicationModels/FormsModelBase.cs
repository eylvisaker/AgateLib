using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsForms.Factories;
using AgateLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public abstract class FormsModelBase : AgateAppModel
	{
		public FormsModelBase(ModelParameters parameters)
			: base(parameters)
		{

		}

		public new FormsModelParameters Parameters
		{
			get { return (FormsModelParameters)base.Parameters; }
		}

		protected override void InitializeImpl()
		{
			Core.Initialize(new FormsFactory());

			var assetProvider = new FileSystemProvider(System.IO.Path.GetFullPath(Parameters.AssetPath));
			AgateLib.IO.FileProvider.Initialize(assetProvider, Parameters.AssetLocations);

			System.IO.Directory.SetCurrentDirectory(assetProvider.SearchPath);
		}

		protected override void Dispose(bool disposing)
		{
		}
	}
}
