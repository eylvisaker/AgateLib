using AgateLib.Platform.WindowsStore.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsPhone.ApplicationModels
{
	public class SerialModel : WindowsStoreSerialModel
	{
		public SerialModel(WindowsStoreSerialModelParameters parameters)
			: base(parameters)
		{
		}

		protected override void InitializeImpl()
		{
			WindowsPhoneInitializer.Initialize(Parameters.RenderTarget, Parameters.AssetLocations);

			InitWindowsStoreCommon();
		}
	}
}
