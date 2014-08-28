using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	/// <summary>
	/// The passive model does very little - it simply initializes AgateLib and cleans up when your
	/// program exits. The passive model is suitable for applications which provide their own message
	/// pump and render loop logic. 
	/// </summary>
	public class PassiveModel : EntryPointAppModelBase
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


		public override void KeepAlive()
		{
			System.Windows.Forms.Application.DoEvents();

			base.KeepAlive();
		}
	}
}
