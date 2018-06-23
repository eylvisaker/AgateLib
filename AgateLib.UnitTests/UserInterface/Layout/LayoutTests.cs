using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Xunit;

namespace AgateLib.UserInterface.Layout
{
    public abstract class LayoutTests
    {
        [Fact]
        public void Layout_FirstItemHasFocus()
        {
            WidgetLayout.Clear();

            WidgetLayout.Focus.Should().BeNull();

            var widget = CommonMocks.Widget("widget").Object;

            WidgetLayout.Add(widget);
            WidgetLayout.Focus.Should().Be(widget);
        }

        [Fact]
        public void Layout_WidgetAddedEvent()
        {
            var widgetsAdded = new List<IWidget>();
            var widgetsCreated = new List<IWidget>();

            WidgetLayout.WidgetAdded += w => widgetsAdded.Add(w);

            for (int i = 0; i < 10; i++)
            {
                var widget = CommonMocks.Widget("test" + i).Object;

                widgetsCreated.Add(widget);
                WidgetLayout.Add(widget);
            }

            widgetsAdded.Should().BeEquivalentTo(widgetsCreated);
        }

        [Fact]
        public void Layout_SetsDisplayHasFocus()
        {
            var widgetsCreated = new List<IWidget>();

            for (int i = 0; i < 10; i++)
            {
                var widget = CommonMocks.Widget("test" + i).Object;

                widgetsCreated.Add(widget);
                WidgetLayout.Add(widget);
            }

            for (int j = 0; j < 10; j++)
            {
                WidgetLayout.Focus = widgetsCreated[j];

                FocusShouldBe(widgetsCreated[j]);

                for (int i = 0; i < 10; i++)
                {
                    if (i == j)
                        widgetsCreated[i].Display.HasFocus.Should().BeTrue($"item {i} should be told that it has focus");
                    else
                        widgetsCreated[i].Display.HasFocus.Should().BeFalse(
                            $"after setting item {j} to have focus, item {i} should be marked as not having focus");
                }
            }
        }

        protected void FocusShouldBe(IWidget widget)
        {
            bool found = false;

            WidgetLayout.Focus.Should().Be(widget);

            foreach (var item in WidgetLayout)
            {
                found |= item == widget;

                item.Display.HasFocus.Should().Be(item == widget);
            }

            if (widget != null)
            {
                found.Should().BeTrue("the widget should be part of the layout");
            }
            else
            {
                found.Should().BeFalse("null should not be part of the layout");
            }
        }

        protected abstract IWidgetLayout WidgetLayout { get; }
    }
}
