using AgateLib.ApplicationModels;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.ApplicationModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FontCreator
{
	class FontCreatorProgram
	{
		static List<string> tempFiles = new List<string>();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var parameters = new PassiveModelParameters(args);

			parameters.AssetLocations.Surfaces = "images";

			new PassiveModel(parameters).Run(() =>
			{
				if (args.First() == "-build" && args.Length >= 2)
				{
					ScriptBuild(args.Skip(1));
					return;
				}

				Directory.CreateDirectory("./images");

				frmFontCreator frm = new frmFontCreator();
				frm.Show();

				Properties.Settings.Default.Reload();

				// workaround for bug in mono 
				bool skipWarning = false;

				try
				{
					skipWarning = Properties.Settings.Default.SkipWarning;
				}
				catch
				{ }

				if (skipWarning == false)
				{
					new frmWarningSplash().ShowDialog(frm);
				}

				try
				{
					Properties.Settings.Default.Save();
				}
				catch
				{ }

				Application.Run(frm);


				foreach (string file in tempFiles)
				{
					File.Delete(file);
				}
			});
		}

		private static void ScriptBuild(IEnumerable<string> files)
		{
			foreach (var file in files)
			{
				var parameters = ReadParameters(file);

				FontBuilder builder = new FontBuilder();
				builder.Parameters = parameters;

				builder.CreateFont();

				string saveName = parameters.SaveName;
				if (string.IsNullOrWhiteSpace(saveName))
					saveName = parameters.Family;

				builder.SaveFont($"output/{saveName}.yaml",
					saveName, $"Fonts/{saveName}");
			}
		}

		private static FontBuilderParameters ReadParameters(string file)
		{
			Deserializer deserializer = new DeserializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention())
				.WithTypeConverter(new AgateLib.Geometry.TypeConverters.ColorConverterYaml())
				.Build();

			using (var stream = new StreamReader(file))
			{
				return deserializer.Deserialize<FontBuilderParameters>(stream);
			}
		}

		public static void RegisterTempFile(string file)
		{
			tempFiles.Add(file);
		}
	}
}
