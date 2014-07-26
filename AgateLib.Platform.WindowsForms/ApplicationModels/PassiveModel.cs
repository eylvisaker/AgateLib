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
			DefaultParameters = new ModelParameters
			{
				ApplicationName = "AgateLib Application",
				AutoCreateDisplayWindow = false,
			};
		}

		static Func<int> ActionToFunc(Action entry)
		{
			return () => { entry(); return 0; };
		}
		public static int Run(Action entry)
		{
			return RunImpl(entry);
		}
		public static int Run(Func<int> entry)
		{
			return RunImpl(entry);
		}
		public static int Run(string[] args, Action entry)
		{
			DefaultParameters.Arguments = args;

			return RunImpl(entry);
		}
		public static int Run(string[] args, Func<int> entry) 
		{
			DefaultParameters.Arguments = args;

			return RunImpl(entry);
		}
		public static int Run(ModelParameters parameters, Action entry)
		{
			DefaultParameters = parameters;

			return RunImpl(entry);
		}
		public static int Run(ModelParameters parameters, Func<int> entry)
		{
			DefaultParameters = parameters;

			return RunImpl(entry);
		}
		private static int RunImpl(Action entry)
		{
			return RunImpl(ActionToFunc(entry));
		}
		private static int RunImpl(Func<int> entry)
		{
			return new PassiveModel(DefaultParameters).RunModel(entry);
		}

		public static ModelParameters DefaultParameters { get; set; }

		#endregion

		PassiveModel(ModelParameters parameters)
			: base(parameters)
		{

		}

		protected override int BeginModel(Func<int> entryPoint)
		{
			return entryPoint();
		}

	}
}
