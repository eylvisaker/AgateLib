using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.InputTests
{
	[TestClass]
	public class SimpleInputHandlerTests : AgateUnitTest
	{
		private SimpleInputHandler handler;
		private IInputHandler inputHandler;

		[TestInitialize]
		public void Initialize()
		{
			handler = new SimpleInputHandler();
			inputHandler = handler;
		}

		[TestMethod]
		public void SimpleInputHandler_MouseMove()
		{
			bool called = false;

			handler.MouseMove += (sender, e) => called = true;

			inputHandler.ProcessEvent(AgateInputEventArgs.MouseMove(new Point(25, 20)));

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void SimpleInputHandler_MouseDown()
		{
			bool called = false;

			handler.MouseDown += (sender, e) => called = true;

			inputHandler.ProcessEvent(AgateInputEventArgs.MouseDown(
				new Point(25, 20), MouseButton.Primary));

			Assert.IsTrue(called);
		}
	}
}
