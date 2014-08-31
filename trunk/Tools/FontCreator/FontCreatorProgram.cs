using AgateLib.ApplicationModels;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.ApplicationModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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
		public static void RegisterTempFile(string file)
		{
			tempFiles.Add(file);
		}
	}
}
