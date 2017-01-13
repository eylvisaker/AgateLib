using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Examples.Launcher
{
	class LauncherPresenter
	{
		private ExampleCategories model;
		private ILauncherView view;

		public LauncherPresenter(ILauncherView view)
		{
			this.view = view;

			this.model = JsonConvert.DeserializeObject<ExampleCategories>(
				File.ReadAllText("example-list.json"));

			view.Categories = model;
		}
	}
}
