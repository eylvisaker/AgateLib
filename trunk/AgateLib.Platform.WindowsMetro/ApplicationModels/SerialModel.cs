using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ApplicationModels;
using Windows.Foundation;
using SharpDX.SimpleInitializer;
using AgateLib.Platform.WindowsStore.ApplicationModels;
using AgateLib.Platform.WindowsStore.DisplayImplementation;

namespace AgateLib.Platform.WindowsMetro.ApplicationModels
{
	public class SerialModel : WindowsStoreSerialModel
	{
		public SerialModel(WindowsStoreSerialModelParameters parameters)
			: base(parameters)
		{
		}

		protected override void InitializeImpl()
		{
			WindowsInitializer.Initialize(Parameters.RenderTarget, Parameters.AssetLocations);

			InitWindowsStoreCommon();
		}

	}
}
