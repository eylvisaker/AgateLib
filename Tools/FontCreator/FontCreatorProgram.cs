using AgateLib.Platform.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using AgateLib;
using AgateLib.Mathematics.TypeConverters;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FontCreatorApp
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

			using (new AgateWinForms(args)
				.AssetPath("images")
				.Initialize())
			{
				if (args.FirstOrDefault() == "-build" && args.Length >= 2)
				{
					ScriptBuild(args.Skip(1));
					return;
				}

				Directory.CreateDirectory("./images");

				frmFontCreator frm = new frmFontCreator();
				frm.Show();

				var warning = AgateApp.Settings.GetOrCreate<FontCreatorWarningSettings>(
					"warnings", () => new FontCreatorWarningSettings());

				if (warning.SkipWarning == false)
				{
					new frmWarningSplash().ShowDialog(frm);
				}

				Application.Run(frm);

				foreach (string file in tempFiles)
				{
					File.Delete(file);
				}
			}
		}

		private static void ScriptBuild(IEnumerable<string> files)
		{
			foreach (var file in files)
			{
				var parameters = ReadParameters(file);

				FontCreator creator = new FontCreator();
				creator.Parameters = parameters;

				creator.CreateFont();

				string saveName = parameters.SaveName;
				if (string.IsNullOrWhiteSpace(saveName))
					saveName = parameters.Family;

				string filename = $"output/{saveName}.yaml";

				creator.SaveFont(filename, saveName, $"Fonts/{saveName}");
			}
		}

		private static FontBuilderParameters ReadParameters(string file)
		{
			Deserializer deserializer = new DeserializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention())
				.WithTypeConverter(new ColorConverterYaml())
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
