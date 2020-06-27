using AgateLib.Display;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// The on-screen pointer indicating to the user which control has focus.
    /// </summary>
    public interface ICursor
    {
        /// <summary>
        /// Set before the draw call to provide rendering services.
        /// </summary>
        IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        /// <summary>
        /// If pointer controls are enabled, this will be set any time the user pushes the stick
        /// controlling the pointer, or users the mouse to move the pointer.
        /// </summary>
        Vector2 Velocity { get; set; }

        /// <summary>
        /// On a desktop system, if the mouse cursor is not in captured mode, then the position
        /// of the cursor will be set anytime the user moves the mouse.
        /// </summary>
        Vector2 Position { get; set; }

        ///// <summary>
        ///// Allows the pointer to animate if input is blocked.
        ///// </summary>
        ////bool AllowInput { get; set; }

        /// <summary>
        /// Called when the UI tells the cursor to move to the render element that has focus.
        /// </summary>
        /// <param name="activeWorkspace"></param>
        /// <param name="focusElement"></param>
        /// <param name="focusContentRect"></param>
        /// <param name="focusAnimatedContentRect"></param>
        void MoveToFocus(Workspace activeWorkspace,
                         IRenderElement focusElement,
                         Rectangle focusContentRect,
                         Rectangle focusAnimatedContentRect);

        /// <summary>
        /// The cursor's update event for animations and physics.
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws the focus indicator.
        /// </summary>
        /// <param name="canvas">Canvas to use for drawing.</param>
        /// <param name="focusElement">The element that has focus.</param>
        /// <param name="activeWorkspace">The workspace which is currently active.</param>
        /// <param name="focusContentRect">The screen coordinates of the content area of the focus element.</param>
        void Draw(GameTime gameTime,
                  ICanvas canvas,
                  float scale);
    }
}
