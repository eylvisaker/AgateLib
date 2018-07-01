﻿//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.UserInterface.Rendering.Transitions;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{

    public class RenderElementAnimator
    {
        private readonly RenderElementDisplay display;
        private Rectangle animatedContentRect;

        public RenderElementAnimator(RenderElementDisplay display)
        {
            this.display = display;
        }

        /// <summary>
        /// Gets the state for double buffering. 
        /// </summary>
        public BufferState Buffer { get; } = new BufferState();

        public RenderTarget2D RenderTarget { get; set; }

        public IWidgetTransition Transition { get; set; }

        /// <summary>
        /// Gets or sets the border rect in the widget's parent's client space.
        /// </summary>
        public Rectangle AnimatedBorderRect
        {
            get => display.Region.BorderToContentOffset.Expand(animatedContentRect);
            set => animatedContentRect = display.Region.BorderToContentOffset.Contract(value);
        }

        /// <summary>
        /// Gets or sets the client rect in the widget's parent's client space.
        /// </summary>
        /// TODO: Unify data storage for content and border rects.
        public Rectangle AnimatedContentRect
        {
            get => animatedContentRect;
            set => animatedContentRect = value;
        }

        public Color Color => new Color(Alpha, Alpha, Alpha, Alpha);

        public AnimationState State { get; set; }

        public float Alpha { get; set; }

        public float LayerDepth { get; set; }

        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets whether the widget is double buffered for drawing.
        /// This is automatically set for top level windows in a workspace.
        /// </summary>
        public bool IsDoubleBuffered => Transition.IsDoubleBuffered;

        /// <summary>
        /// Gets whether or not the widget is currently in an animation state.
        /// </summary>
        public bool IsAnimating => State != AnimationState.Static;

        public void Initialize()
        {
            State = 0;
            Alpha = 1;
            Transition = null;
        }

        internal void ContentRectUpdated()
        {
            if (Transition != null)
            {
                Transition?.ContentRectUpdated(display);
            }
            else
            {
                AnimatedContentRect = display.ContentRect;
                AnimatedBorderRect = display.BorderRect;
            }
        }
    }

    public enum AnimationState
    {
        TransitionIn,
        Static,
        TransitionOut,
        Dead,
    }

}
