using AgateLib.ApplicationModels;
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
			:base (parameters)
		{

		}

		protected override void Initialize()
		{
			Configuration.Initialize();
		}
		protected override void Dispose()
		{
			
		}
	}
}
