using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
			view.LaunchExample += (sender, args) => LaunchExample(args.Example);
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

		private void LaunchExample(ExampleItem example)
		{
			MethodInfo exampleMain = FindExampleMain(example);
			var args = new string[] { "-window" };

			try
			{
				view.Hide();

				exampleMain.Invoke(null, new object[] { args });
			}
			finally
			{
				view.Show();
			}
		}

		private MethodInfo FindExampleMain(ExampleItem example)
		{
			var ns = "Examples." + example.Path.Replace("/", ".");

			var types = Assembly.GetAssembly(GetType()).DefinedTypes.Where(x => x.Namespace == ns);

			return types
				.SelectMany(x => x.DeclaredMethods)
				.Single(x => x.Name == "Main" && x.IsStatic);
		}
	}
}
