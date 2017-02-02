using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.DisplayImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.Tests.IntegrationTests.WinForms
{
	[TestClass]
	public class DisplayWindowInitializeTests
	{
		[TestMethod]
		public void CreatedWindowOnEachMonitorTarget()
		{
			bool failure = false;
			StringBuilder builder = new StringBuilder();

			using (new AgateWinForms(null)
				.Initialize())
			{
				foreach (var screen in Display.Screens.AllScreens)
				{
					using (var window = new DisplayWindow(new CreateWindowParams
					{
						TargetScreen = screen,
						Resolution = new Resolution(100, 100)
					}))
					{
						var impl = window.Impl as GL_DisplayControl;

						if (!screen.Bounds.Contains(impl.DesktopBounds))
						{
							failure = true;
							builder.AppendLine($"FAILURE: Window was not created on window at {screen.Bounds}");
						}
						else
						{
							builder.AppendLine($"SUCCESS: Window was created in screen at {screen.Bounds}.");
						}
					}
				}
			}

			Assert.IsFalse(failure, builder.ToString());
		}
	}
}
