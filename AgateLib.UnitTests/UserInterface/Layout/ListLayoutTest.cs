using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Xunit;
using Moq;

namespace ManualTests.AgateLib.UserInterface.Layout
{
    public abstract class ListLayoutTests : LayoutTests
    {
        public ListLayoutTests()
        {
            for (int i = 0; i < 10; i++)
            {
                Add(CreateWidget().Object);
            }

            ListLayout.FocusIndex.Should().Be(0);
        }

        [Fact]
        public void LL_NextItem()
        {
            NextItem();
            NextItem();

            ListLayout.FocusIndex.Should().Be(2);
        }

        [Fact]
        public void LL_PreviousItem()
        {
            ListLayout.FocusIndex = 6;

            PreviousItem();
            PreviousItem();

            ListLayout.FocusIndex.Should().Be(4);
        }

        [Fact]
        public void LL_Clear()
        {
            ListLayout.Clear();

            ListLayout.Count.Should().Be(0);
        }

        [Fact]
        public void LL_FocusChangedEvent()
        {
            int focusChangedCalled = 0;

            ListLayout.FocusChanged += (_, __) => focusChangedCalled++;

            ListLayout.FocusIndex = 2;
            focusChangedCalled.Should().Be(1);
            ListLayout.Focus.Should().Be(ListLayout[2]);

            ListLayout.FocusIndex = 4;
            focusChangedCalled.Should().Be(2);
            ListLayout.Focus.Should().Be(ListLayout[4]);

            ListLayout.FocusIndex = 4;
            focusChangedCalled.Should().Be(2);

            PreviousItem();
            focusChangedCalled.Should().Be(3);

            NextItem();
            focusChangedCalled.Should().Be(4);
        }

        [Fact]
        public void LL_NoWrap()
        {
            EnableWrapping = false;
            EnableWrapping.Should().BeFalse();

            ListLayout.FocusIndex = 0;
            PreviousItem();
            ListLayout.FocusIndex.Should().Be(0);

            ListLayout.FocusIndex = ListLayout.Count - 1;
            NextItem();
            ListLayout.FocusIndex.Should().Be(ListLayout.Count - 1);
        }

        [Fact]
        public void LL_Wrap()
        {
            EnableWrapping = true;
            EnableWrapping.Should().BeTrue();

            ListLayout.FocusIndex = 0;
            PreviousItem();
            ListLayout.FocusIndex.Should().Be(ListLayout.Count - 1);

            ListLayout.FocusIndex = ListLayout.Count - 1;
            NextItem();
            ListLayout.FocusIndex.Should().Be(0);
        }

        [Fact]
        public void LL_WidgetCapturesInput()
        {
            var inputWidget = CommonMocks.Widget("test");

            int ignoreNext = 3;

            inputWidget
                .Setup(x => x.ProcessEvent(It.IsAny<WidgetEventArgs>()))
                .Callback<WidgetEventArgs>(e =>
                {
                    if (e.EventType != WidgetEventType.ButtonDown)
                        return;

                    if (ignoreNext > 0)
                    {
                        ignoreNext--;
                        e.Handled = true;
                        return;
                    }
                });

            ListLayout.Insert(3, inputWidget.Object);
            ListLayout.FocusIndex = 2;
            ListLayout.FocusIndex.Should().Be(2);
            ignoreNext.Should().Be(3);

            NextItem();
            ListLayout.FocusIndex.Should().Be(3);
            ignoreNext.Should().Be(3);

            NextItem();
            ListLayout.FocusIndex.Should().Be(3);
            ignoreNext.Should().Be(2);

            NextItem();
            ListLayout.FocusIndex.Should().Be(3);
            ignoreNext.Should().Be(1);

            NextItem();
            ListLayout.FocusIndex.Should().Be(3);
            ignoreNext.Should().Be(0);

            NextItem();
            ListLayout.FocusIndex.Should().Be(4);
        }

        [Fact]
        public void LL_NoNulls()
        {
            Action add = () => ListLayout.Add(null);
            Action insert = () => ListLayout.Insert(3, null);

            add.Should().Throw<ArgumentNullException>();
            insert.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LL_ListBehavior()
        {
            var item = new Mock<IWidget>().Object;

            var item3 = ListLayout[3];
            ListLayout.IndexOf(item3).Should().Be(3);
            ListLayout.Contains(item).Should().BeFalse();

            ListLayout.Insert(3, item);
            ListLayout[3].Should().Be(item);
            ListLayout[4].Should().Be(item3);
            ListLayout.IndexOf(item).Should().Be(3);
            ListLayout.IndexOf(item3).Should().Be(4);

            ListLayout.Count.Should().Be(11);
            ListLayout.Contains(item).Should().BeTrue();

            ListLayout.Remove(item).Should().BeTrue();

            ListLayout[3].Should().Be(item3);
            ListLayout.Count.Should().Be(10);
            ListLayout.Contains(item).Should().BeFalse();

            ListLayout.RemoveAt(3);
            ListLayout.IndexOf(item3).Should().Be(-1);
            ListLayout.Contains(item3).Should().BeFalse();
            ListLayout.Count.Should().Be(9);

            IWidget[] array = new IWidget[9];
            ListLayout.CopyTo(array, 0);

            var index = 0;
            foreach(var listItem in ListLayout)
            {
                listItem.Should().Be(array[index]);
                index++;
            }
        }

        [Fact]
        public void LL_NavigationUpdatesFocus()
        {
            ListLayout.FocusIndex = 2;
            FocusShouldBe(ListLayout[2]);

            NextItem();
            FocusShouldBe(ListLayout[3]);

            ListLayout.Focus = ListLayout[8];
            FocusShouldBe(ListLayout[8]);

            PreviousItem();
            FocusShouldBe(ListLayout[7]);
        }

        protected override IWidgetLayout WidgetLayout => ListLayout;
        protected abstract IListLayout ListLayout { get; }
        protected abstract bool EnableWrapping { get; set; }

        protected abstract void NextItem();
        protected abstract void PreviousItem();

        protected void Add(IWidget widget)
        {
            ListLayout.Add(widget);
        }

        protected void Send(MenuInputButton button)
        {
            ListLayout.InputEvent(WidgetEventArgs.ButtonDown(button));
            ListLayout.InputEvent(WidgetEventArgs.ButtonUp(button));
        }

        protected void TestSizeMetrics(Size expectedIdealSize)
        {
            var sizeMetrics = new SizeMetrics();
            var renderContext = new Mock<IWidgetRenderContext>();

            var idealSize = ListLayout.ComputeIdealSize(Size.Empty, renderContext.Object);

            idealSize.Should().Be(expectedIdealSize);
        }

        private Mock<IWidget> CreateWidget()
        {
            var result = new Mock<IWidget>();
            var display = new WidgetDisplay();

            result.Setup(x => x.Display).Returns(display);
            result.Setup(
                x => x.ComputeIdealSize(It.IsAny<IWidgetRenderContext>(), It.IsAny<Size>()))
                .Returns<IWidgetRenderContext, Size>((_, size) => new Size(100, 20));

            return result;
        }
    }
}
