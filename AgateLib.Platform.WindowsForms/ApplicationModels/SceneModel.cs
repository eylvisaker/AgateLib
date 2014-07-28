using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public class SceneModel : FormsModelBase
	{
		private SceneModel(ModelParameters parameters) : base(parameters)
		{ }

		public static ModelParameters DefaultParameters { get; set; }

		protected override int BeginModel(Func<int> entryPoint)
		{
			throw new NotImplementedException();
		}

		public override void KeepAlive()
		{
			throw new NotImplementedException();
		}
	}
}
