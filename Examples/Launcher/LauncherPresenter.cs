using System;
using System.Collections.Generic;
using System.Drawing;
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

			view.SelectedExampleChanged += View_SelectedExampleChanged;
		}

		private void View_SelectedExampleChanged(object sender, ExampleEventArgs e)
		{
			string image = e.Example?.Images?.FirstOrDefault();

			if (image == null)
			{
				view.Image?.Dispose();
				view.Image = null;
				return;
			}

			image = e.Example.Path + "/" + image;

			view.Image = Image.FromFile(image);
		}
	}
}
