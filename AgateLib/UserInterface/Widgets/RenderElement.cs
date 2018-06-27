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
        /// Gets a read-only collection of children of this element.
        /// </summary>
        IEnumerable<IRenderElement> Children { get; }

        /// <summary>
        /// Gets the child of this element that has focus.
        /// </summary>
        IRenderElement Focus { get; set; }

        /// <summary>
        /// Gets the type identifier used to identify this widget type to the styling
        /// engine.
        /// </summary>
        string StyleTypeIdentifier { get; }

        // TODO: Add StyleClass property.
        // TODO: Add StyleId property.
        

        /// <summary>
        /// Gets the name of the widget.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether or not the widget can receive input focus.
        /// </summary>
        bool CanHaveFocus { get; }

        IRenderElementStyle Style { get; }

        /// <summary>
        /// Draws the content of the widget.
        /// To draw children, call <c >renderContext.DrawChildren</c>.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="offset"></param>
        void Draw(IWidgetRenderContext renderContext, Rectangle clientArea);

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

    public abstract class RenderElement<TProps> : IRenderElement where TProps : RenderElementProps
    {
        public RenderElement(TProps props)
        {
            this.Props = props;
        }

        public WidgetDisplay Display { get; } = new WidgetDisplay();


        public virtual IEnumerable<IRenderElement> Children => null;

        public virtual IRenderElement Focus
        {
            get => null;
            set => throw new NotImplementedException();
        }

        public IRenderElementStyle Style { get; } = new RenderElementStyle();

        public virtual string StyleTypeIdentifier => GetType().Name;

        public string StyleClass => Props.StyleClass;

        public string StyleId => Props.StyleId;

        public string Name => Props.StyleId;

        public virtual bool CanHaveFocus => false;

        public TProps Props { get; }

        public abstract Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize);

        public abstract void Draw(IWidgetRenderContext renderContext, Rectangle clientArea);

        public virtual void ProcessEvent(WidgetEventArgs widgetEventArgs)
        { }

        public virtual void Update(IWidgetRenderContext renderContext)
        {
        }
    }

    public class RenderElementProps
    {
        public string StyleId { get; set; }

        public string StyleClass { get; set; }
    }
}
