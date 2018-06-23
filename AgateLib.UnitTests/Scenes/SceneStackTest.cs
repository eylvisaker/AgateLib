using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.Scenes
{
    public class SceneStackTest
    {
        SceneStack stack = new SceneStack();
        double totalTimeInSeconds;
        List<Mock<IScene>> activatedScenes = new List<Mock<IScene>>();

        [Fact]
        public void SceneStackRemovesTopSceneWhenFinished()
        {
            var a = CreateScene();
            var b = CreateScene();
            var c = CreateScene();

            stack.Add(a.Object);
            stack.Add(b.Object);
            stack.Add(c.Object);
            
            RunUpdate(5);

            activatedScenes.Count.Should().Be(0);

            c.Setup(x => x.IsFinished).Returns(true);

            RunUpdate(2);

            activatedScenes.Should().ContainSingle(x => x == b);

            stack.SceneList.Should().NotContain(c.Object);
        }

        [Fact]
        public void SceneStackRemovesFinishedScenesInMiddleOfStack()
        {
            var a = CreateScene();
            var b = CreateScene();
            var c = CreateScene();

            stack.Add(a.Object);
            stack.Add(b.Object);
            stack.Add(c.Object);

            RunUpdate(5);

            activatedScenes.Count.Should().Be(0);

            b.Setup(x => x.IsFinished).Returns(true);

            RunUpdate(2);

            stack.SceneList.Should().NotContain(b.Object);
            activatedScenes.Count.Should().Be(0);
        }

        private void RunUpdate(int times, double intervalInSeconds = 0.1)
        {
            for (int i = 0; i < times; i++)
            {
                totalTimeInSeconds += intervalInSeconds;
                stack.Update(new GameTime(
                    TimeSpan.FromSeconds(intervalInSeconds),
                    TimeSpan.FromSeconds(totalTimeInSeconds)));
            }
        }

        private Mock<IScene> CreateScene(bool updateBelow = true)
        {
            var result = new Mock<IScene>();

            result.SetupAllProperties();
            result.Setup(x => x.UpdateBelow).Returns(updateBelow);
            result.Setup(x => x.SceneActivated()).Callback(() => activatedScenes.Add(result));

            return result;
        }
    }
}
