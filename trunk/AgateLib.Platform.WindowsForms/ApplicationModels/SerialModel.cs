using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public class SerialModel : FormsModelBase
	{
		static SerialModel()
		{
			DefaultParameters = new ModelParameters
			{
				ApplicationName = "AgateLib Application",
				AutoCreateDisplayWindow = true,
			};
		}
		private SerialModel(ModelParameters parameters) : base(parameters)
		{ }

		public static void Run(string[] parameters, Action entryPoint)
		{
			Configuration.Initialize();
		}

		public static ModelParameters DefaultParameters { get; set; }

		protected override int BeginModel(Func<int> entryPoint)
		{
			throw new NotImplementedException();
		}
	}
}
