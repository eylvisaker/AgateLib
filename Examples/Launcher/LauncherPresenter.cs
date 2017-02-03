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

			view.SelectedExampleChanged += View_SelectedExampleChanged;
			view.LaunchExample += (sender, args) => LaunchExample(args.Example);

			this.model = JsonConvert.DeserializeObject<ExampleCategories>(
				File.ReadAllText("example-list.json"));

			view.Categories = model;
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

			try
			{
				view.Image = Image.FromFile(image);
			}
			catch (FileNotFoundException)
			{
				view.Image = null;
			}
		}

		private void LaunchExample(ExampleItem example)
		{
			MethodInfo exampleMain = FindExampleMain(example);
			var args = view.Arguments.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			CopyExampleAssets(example.Path);

			try
			{
				view.Hide();

				exampleMain.Invoke(null, new object[] { args });
			}
			finally
			{
				view.Show();
			}

			GC.Collect();

			DeleteAssets();
		}

		private void CopyExampleAssets(string path)
		{
			var fullPath = Path.Combine(path, "Assets");

			DeleteAssets();

			if (Directory.Exists(fullPath))
			{
				DirectoryCopy(fullPath, "Assets", true);
			}
		}

		private void DeleteAssets()
		{
			if (Directory.Exists("Assets"))
			{
				Directory.Delete("Assets", true);
			}
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
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
