using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderElement : IRenderable
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

        /// <summary>
        /// Gets the class name used to identify this widget to the styling
        /// </summary>
        string StyleClass { get; }

        /// <summary>
        /// Gets the identifier of this widget for styling.
        /// </summary>
        string StyleId { get; }        

        /// <summary>
        /// Gets the name of the widget.
        /// </summary>
        [Obsolete("Use StyleId instead?")]
        string Name { get; }

        /// <summary>
        /// Gets whether or not the widget can receive input focus.
        /// </summary>
        bool CanHaveFocus { get; }

        /// <summary>
        /// Gets the aggregated style of the render element.
        /// </summary>
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

            Display = new WidgetDisplay(Props.Style);
        }

        public WidgetDisplay Display { get; }

        public virtual IEnumerable<IRenderElement> Children => null;

        public virtual IRenderElement Focus
        {
            get => null;
            set => throw new NotImplementedException();
        }

        public IRenderElementStyle Style => Display.Style;

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

        public void DrawChildren(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            renderContext.DrawChildren(clientArea, Children);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(StyleTypeIdentifier);

            if (!string.IsNullOrWhiteSpace(StyleClass))
                result.Append($".{StyleClass}");

            if (!string.IsNullOrWhiteSpace(StyleId))
                result.Append($"#{StyleId}");

            return result.ToString();
        }

        IRenderElement IRenderable.Render() => this;
    }

    public class RenderElementProps
    {
        public string StyleId { get; set; }

        public string StyleClass { get; set; }

        /// <summary>
        /// Style elements specified by the parent.
        /// </summary>
        public InlineElementStyle Style { get; set; }
    }
}
