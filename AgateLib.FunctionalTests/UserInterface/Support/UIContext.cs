using System;
using System.Collections.Generic;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering;
using FluentAssertions;
using Moq;
using ManualTests.AgateLib.UserInterface.FF6;
using AgateLib.Fakes;

namespace AgateLib.FunctionalTests.UserInterface.Support
{
    public class UIContext
    {
        private List<string> events = new List<string>();

        public UIContext()
        {
            Desktop = new Desktop();
        }

        public Desktop Desktop { get; set; }

        public IWidget ActiveWindow => Desktop.ActiveWorkspace.ActiveWindow;

        public Workspace ActiveWorkspace => Desktop.ActiveWorkspace;

        public bool ExitTriggered { get; private set; }

        public FF6Model Model { get; set; }

        public void RecordEvent(string name)
        {
            events.Add(name);
        }

        public void VerifyEvent(string name)
        {
            events.Should().Contain(name);
        }

        public void WaitForAnimations()
        {
            Desktop.WaitForAnimations();
        }
    }
}