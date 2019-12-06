using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface
{
    public class LabelReader : InputCaptureElement<LabelReaderProps>
    {
        private int readRate = 1;
        private bool readingComplete = false;

        public LabelReader(LabelReaderProps props) : base(props)
        {
            readRate = props.NormalReadRate;
        }

        public override void Update(IUserInterfaceRenderContext renderContext)
        {
            if (!HasFocus)
            {
                readRate = Props.NormalReadRate;
                return;
            }

            var labels = Parent.Children.OfType<LabelElement>();

            if (labels.All(x => x.AnimationState.IsComplete) && !readingComplete)
            {
                renderContext.Invoke(() => Props.OnAnimationCompleted?.Invoke(new UserInterfaceEvent(this)));

                readRate = Props.NormalReadRate;
            }
            else
            {
                readingComplete = false;
            }

            foreach (var label in labels)
            {
                label.AnimationState.SlowReadRate = ContentRenderOptions.DefaultSlowReadRate * readRate;
            }
        }

        public override void OnButtonDown(ButtonStateEventArgs args)
        {
            if (args.ActionButton == UserInterfaceAction.Accept)
            {
                readRate = Props.AcceleratedReadRate;
            }
        }

        public override void OnButtonUp(ButtonStateEventArgs args)
        {
            if (args.ActionButton == UserInterfaceAction.Accept)
            {
                readRate = Props.NormalReadRate;
            }
        }
    }

    public class LabelReaderProps : InputCaptureElementProps
    {
        public int NormalReadRate { get; set; } = 1;

        public int AcceleratedReadRate { get; set; } = 5;

        public UserInterfaceEventHandler OnAnimationCompleted { get; set; }
    }
}
