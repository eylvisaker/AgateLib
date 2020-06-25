using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;

namespace AgateLib.UserInterface
{
    public class InputCaptureElement<TProps> : RenderElement<TProps> where TProps : InputCaptureElementProps
    {
        public InputCaptureElement(TProps props) : base(props)
        {
        }

        public event EventHandler<UserInterfaceActionEventArgs> UserAction;

        public override bool ParticipateInLayout => false;
        public override bool CanHaveFocus => true;

        public override void OnUserInterfaceAction(UserInterfaceActionEventArgs args)
        {
            UserAction?.Invoke(this, args);

            if (args.Action == UserInterfaceAction.Accept)
            {
                OnAccept(args);
            }
            else if (args.Action == UserInterfaceAction.Cancel)
            {
                OnCancel(args);
            }

            if (!args.Handled && !Props.CaptureInput)
            {
                Parent?.OnChildAction(this, args);
            }
        }

        public override Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize) => Size.Empty;
        public override void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size) { }
        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea) { }
    }

    public class InputCaptureElementProps : RenderElementProps
    {
        /// <summary>
        /// If true, the input capture element will not allow movement events to set focus to other controls.
        /// Defaults to true.
        /// </summary>
        public bool CaptureInput { get; set; } = true;

    }
}
