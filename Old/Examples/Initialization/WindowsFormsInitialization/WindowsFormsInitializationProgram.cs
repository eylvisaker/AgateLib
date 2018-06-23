using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;

namespace Examples.Initialization.WindowsFormsInitialization
{
	static class WindowsFormsInitializationProgram
	{
		static AgateFormMixtureForm form;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args).Initialize())
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				form = new AgateFormMixtureForm();
				form.Draw += (sender, e) => DrawFormContents();

				Application.Run(form);
			}
		}

		private static void DrawFormContents()
		{
			Display.RenderTarget = form.DisplayWindow.FrameBuffer;
			Display.BeginFrame();
			Display.Clear(Color.Maroon);

			var layout = Font.AgateSans.LayoutText(form.EntryText, form.DisplayWindow.Width);

			layout.Draw(new Point(0, 0));

			Display.EndFrame();
			AgateApp.KeepAlive();
		}
	}
}
