using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.InputLib;
using AgateLib.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AgateLib.UnitTests.AgateAppTests
{
	[TestClass]
	public class SceneStackTests : AgateUnitTest
	{
		class SafeSceneStack : SceneStack
		{
			public SafeSceneStack()
			{
				const int maxFrames = 10000;

				int counter = 0;
				base.FrameCompleted += (sender, args) =>
				{
					counter++;

					if (counter > maxFrames)
					{
						Abort();

						throw new InvalidOperationException($"Unit test failed to terminate after {maxFrames} frames.");
					}
				};
			}
		}

		[TestMethod]
		public void SceneStack_UpdateDrawCalled()
		{
			bool updateCalled = false;
			bool drawCalled = false;

			var scene = new Scene();
			var sceneStack = new SafeSceneStack();

			scene.Update += (sender, e) => { updateCalled = true; };
			scene.Redraw += (sender, e) =>
			{
				Assert.IsTrue(updateCalled, "Update should be called before the first call to draw.");
				drawCalled = true;

				scene.IsFinished = true;
			};

			sceneStack.Start(scene);

			Assert.IsTrue(updateCalled, "Update was not called.");
			Assert.IsTrue(drawCalled, "Draw was not called.");
		}

		[TestMethod]
		public void SceneStack_UpdateDrawBelow()
		{
			var scene1 = new Scene();
			var scene2 = new Scene { DrawBelow = false, UpdateBelow = false };
			var scene3 = new Scene { DrawBelow = true, UpdateBelow = true };

			scene1.Update += (sender, e) => Assert.Fail("Scene1 should not be updated.");
			scene1.Redraw += (sender, e) => Assert.Fail("Scene1 should not be drawn.");

			bool updateSuccess = false;
			bool redrawSuccess = false;

			scene2.Update += (sender, e) => updateSuccess = true;
			scene2.Redraw += (sender, e) => redrawSuccess = true;
			scene2.SceneActivated += (sender, e) => AgateApp.IsAlive = false;

			int drawCount = 0;

			scene3.Redraw += (sender, e) =>
			{
				if (++drawCount == 3)
					scene3.IsFinished = true;
			};

			var stack = new SafeSceneStack();

			stack.Add(scene1);
			stack.Add(scene2);
			stack.Start(scene3);

			Assert.IsTrue(updateSuccess);
			Assert.IsTrue(redrawSuccess);
			Assert.AreEqual(3, drawCount);
		}

		[TestMethod]
		public void SceneStack_InputHandler()
		{
			bool caughtKeyDown = false;

			var inputHandler = new SimpleInputHandler();
			var scene = new Scene { InputHandler = inputHandler };

			inputHandler.KeyDown += (sender, e) => caughtKeyDown = true;

			scene.Update += (sender, e) =>
			{
				if (caughtKeyDown)
					scene.IsFinished = true;

				Input.QueueInputEvent(AgateInputEventArgs.KeyDown(KeyCode.A, new KeyModifiers()));
			};

			var stack = new SafeSceneStack();

			stack.Start(scene);
		}

		[TestMethod]
		public void SceneStack_TopScene()
		{
			var scene = new Scene();

			Assert.IsFalse(scene.IsTopScene);

			scene.Update += (sender, e) =>
			{
				Assert.IsTrue(scene.IsTopScene);

				scene.IsFinished = true;
			};

			var stack = new SafeSceneStack();

			stack.Start(scene);
		}

		[TestMethod]
		public void SceneStack_DisposableScene()
		{
			int count = 0;
			var scene = new Mock<IScene>();

			scene.Setup(x => x.IsFinished)
				.Returns(() => count > 5);

			scene.Setup(x => x.Update(It.IsAny<ClockTimeSpan>()))
				.Callback<ClockTimeSpan>(clock => count++);

			var disposable = scene.As<IDisposable>();

			bool disposed = false;

			disposable.Setup(x => x.Dispose()).Callback(() => disposed = true);

			var stack = new SafeSceneStack();

			stack.Start(scene.Object);

			Assert.IsTrue(disposed, "Disposable scene did not have Dispose called.");
		}
	}
}
