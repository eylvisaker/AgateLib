using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public class PassiveModel : FormsModelBase
	{
		#region --- Static Members ---

		static PassiveModel()
		{
			DefaultParameters = new PassiveModelParameters();
		}

		
		public static PassiveModelParameters DefaultParameters { get; set; }

		#endregion

		public PassiveModel() : this(DefaultParameters)
		{ }

		public PassiveModel(PassiveModelParameters parameters)
			: base(parameters)
		{
		}
		public PassiveModel(string[] args) : this(DefaultParameters)
		{
			Parameters.Arguments = args;

			ProcessArguments();
		}

		public new PassiveModelParameters Parameters
		{
			get { return (PassiveModelParameters)base.Parameters; }
		}

		protected override int BeginModel(Func<int> entryPoint)
		{
			return entryPoint();
		}

	}
}
