using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.AgateAppTests
{
	[TestClass]
	public class SceneStackTests : AgateUnitTest
	{
		class MyScene : Scene
		{
			public event EventHandler TheUpdate;
			public event EventHandler TheRedraw;

			public override void Update(ClockTimeSpan elapsed)
			{
				TheUpdate?.Invoke(this, EventArgs.Empty);
			}

			public override void Draw()
			{
				TheRedraw?.Invoke(this, EventArgs.Empty);
			}
		}

		[TestMethod]
		public void SceneStack_UpdateDrawCalled()
		{
			bool updateCalled = false;
			bool drawCalled = false;

			var scene = new MyScene();
			var sceneStack = new SceneStack();

			scene.TheUpdate += (sender, e) => { updateCalled = true; };
			scene.TheRedraw += (sender, e) =>
			{
				Assert.IsTrue(updateCalled, "Update should be called before the first call to draw.");
				drawCalled = true;

				sceneStack.Remove(scene);
			};

			sceneStack.Start(scene);

			Assert.IsTrue(updateCalled, "Update was not called.");
			Assert.IsTrue(drawCalled, "Draw was not called.");
		}
	}
}
