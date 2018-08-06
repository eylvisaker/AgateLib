using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class TextBox : Widget<TextBoxProps, TextBoxState>
    {
        public TextBox(TextBoxProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            throw new NotImplementedException();
        }
    }

    public class TextBoxProps : WidgetProps
    {
        public string Value { get; internal set; }
        public UserInterfaceEventHandler<string> OnChange { get; internal set; }
    }

    public class TextBoxState
    {
    }
}
