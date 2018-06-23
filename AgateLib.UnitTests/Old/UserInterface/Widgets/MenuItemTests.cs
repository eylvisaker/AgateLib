using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Widgets
{
	[TestClass]
	public class MenuItemTests
	{
		[TestMethod]
		public void PushAccept()
		{
			bool pressed = false;
			MenuItem item = new MenuItem();

			item.PressAccept += (sender, args) => pressed = true;
			item.OnPressAccept();

			Assert.IsTrue(pressed, "PressAccept was not raised.");
		}
		
		[TestMethod]
		public void PushMenu()
		{
			bool pressed = false;
			MenuItem item = new MenuItem();

			item.PressMenu += (sender, args) => pressed = true;
			item.OnPressMenu();

			Assert.IsTrue(pressed, "PressMenu was not raised.");
		}

		[TestMethod]
		public void PushToggle()
		{
			bool pressed = false;
			MenuItem item = new MenuItem();

			item.PressToggle += (sender, args) => pressed = true;
			item.OnPressToggle();

			Assert.IsTrue(pressed, "PressToggle was not raised.");
		}
	}
}
