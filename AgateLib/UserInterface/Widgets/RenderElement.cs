﻿using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderElement
    {
        /// <summary>
        /// Gets the widget display object that contains all the data needed to
        /// render styling and animation of the widget on screen.
        /// </summary>
        WidgetDisplay Display { get; }

        /// <summary>
        /// Gets a read-only collection of children of this widget.
        /// </summary>
        IWidgetChildren Children { get; }

        /// <summary>
        /// Gets the child of this element that has focus.
        /// </summary>
        IRenderElement Focus { get; set; }

        /// <summary>
        /// Gets the type identifier used to identify this widget type to the styling
        /// engine.
        /// </summary>
        string StyleTypeIdentifier { get; }

        /// <summary>
        /// Gets the name of the widget.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether or not the widget can receive input focus.
        /// </summary>
        bool CanHaveFocus { get; }
        
        /// <summary>
        /// Draws the content of the widget.
        /// To draw children, call <c >renderContext.DrawChildren</c>.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="offset"></param>
        void Draw(IWidgetRenderContext renderContext, Point offset);

        /// <summary>
        /// Updates the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        void Update(IWidgetRenderContext renderContext);

        /// <summary>
        /// Compute the ideal size of the content of the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize);

        /// <summary>
        /// Called when the widget receives an input event.
        /// </summary>
        /// <param name="widgetEventArgs"></param>
        void ProcessEvent(WidgetEventArgs widgetEventArgs);
    }
}
