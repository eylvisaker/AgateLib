using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AgateLib.UserInterface
{
    public class Notebook : RenderElement<NotebookProps, NotebookState>
    {
        private class NotebookLayout
        {
            public List<IRenderElement> Tabs = new List<IRenderElement>();
            public List<NotebookPage> Pages;

            public void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size)
            {
                Size tabAreaIdealSize = CalcIdealTabAreaSize(layoutContext, size);

                Point loc = new Point();

                foreach (IRenderElement tab in Tabs)
                {
                    Size tabSize = tab.CalcIdealContentSize(layoutContext, size);

                    tab.Display.Region.SetContentSize(new Size(tabAreaIdealSize.Width, tabSize.Height));

                    tab.Display.Region.SetMarginPosition(loc);

                    loc.Y += tab.Display.MarginRect.Height;
                }

                Size pageSize = new Size(size.Width - tabAreaIdealSize.Width, size.Height);

                foreach (IRenderElement page in Pages)
                {
                    page.Display.MarginRect = new Rectangle(
                        tabAreaIdealSize.Width,
                        0,
                        pageSize.Width,
                        pageSize.Height);
                }
            }

            public Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize)
            {
                Size tabAreaIdealSize = CalcIdealTabAreaSize(layoutContext, maxSize);

                int maxPageWidth = 0;
                int maxPageHeight = 0;

                Size maxPageSize = new Size(maxSize.Width - tabAreaIdealSize.Width, maxSize.Height);

                maxPageSize.Width = Math.Max(maxPageSize.Width, maxSize.Width / 2);

                foreach (var page in Pages)
                {
                    Size pageSize = page.CalcIdealMarginSize(layoutContext, maxSize);

                    maxPageWidth = Math.Max(pageSize.Width, maxPageWidth);
                    maxPageHeight = Math.Max(pageSize.Height, maxPageWidth);
                }

                return new Size(tabAreaIdealSize.Width + maxPageWidth,
                                Math.Max(tabAreaIdealSize.Height, maxPageHeight));
            }

            private Size CalcIdealTabAreaSize(IUserInterfaceLayoutContext layoutContext, Size maxSize)
            {
                // layout tabs vertically on the left
                int maxTabWidth = 0;
                int tabHeight = 0;

                foreach (IRenderElement tab in Tabs)
                {
                    Size tabSize = tab.CalcIdealMarginSize(
                        layoutContext,
                        new Size(maxSize.Width / 2, maxSize.Height / 2));

                    maxTabWidth = Math.Max(maxTabWidth, tabSize.Width);
                    tabHeight += tabSize.Height;
                }

                Size tabAreaIdealSize = new Size(maxTabWidth, tabHeight);
                return tabAreaIdealSize;
            }
        }

        private NotebookLayout layout = new NotebookLayout();
        private List<IRenderElement> children = new List<IRenderElement>();

        public Notebook(NotebookProps props) : base(props)
        {
            SetState(new NotebookState { ActivePageIndex = props.InitialActivePage });

            ReceiveNotebookProps();
        }

        public override IList<IRenderElement> Children
        {
            get => children;
            protected set => children = value.ToList();
        }

        public override Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize)
        {
            return layout.CalcIdealContentSize(layoutContext, maxSize);
        }

        public override void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size)
        {
            layout.DoLayout(layoutContext, size);

            PerformLayoutForChildren(layoutContext, layout.Tabs);
            PerformLayoutForChildren(layoutContext, this.Props.Pages);
        }

        public override void DrawBackgroundAndBorder(IUserInterfaceRenderContext renderContext, Rectangle rtClientDest)
        {
            base.DrawBackgroundAndBorder(renderContext, rtClientDest);
        }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            DrawChildren(renderContext, clientArea, layout.Tabs);

            DrawChild(renderContext, clientArea, Props.Pages[State.ActivePageIndex]);
        }

        public override bool CanHaveFocus => Props.Pages.Where(x => x.Props.Visible).Any();

        public override void OnFocus()
        {
            Display.System.SetFocus(layout.Tabs[State.ActivePageIndex]);
        }

        public override void OnChildAction(IRenderElement child, UserInterfaceActionEventArgs action)
        {
            base.OnChildAction(child, action);

            int focusIndex = children.IndexOf(child);
            int oldFocusIndex = focusIndex;

            UserInterfaceAction button = action.Action;

            if (button == UserInterfaceAction.Down)
            {
                focusIndex++;
            }
            if (button == UserInterfaceAction.Up)
            {
                focusIndex--;
            }

            if (focusIndex < 0 || focusIndex >= layout.Tabs.Count)
            {
                return;
            }

            SetState(s => s.ActivePageIndex = focusIndex);
        }

        protected override void OnReceiveProps()
        {
            base.OnReceiveProps();

            ReceiveNotebookProps();
        }

        protected override void OnReceiveState()
        {
            base.OnReceiveState();

            if (State.ActivePageIndex < layout.Tabs.Count)
            {
                Display.System.SetFocus(layout.Tabs[State.ActivePageIndex]);
            }
        }

        private void ReceiveNotebookProps()
        {
            layout.Tabs.Clear();

            foreach (var page in Props.Pages)
            {
                IRenderElement tab = PageTab(page);

                layout.Tabs.Add(tab);
            }

            layout.Pages = Props.Pages;
        }

        protected override void OnFinalizeChildren()
        {
            children.Clear();
            children.AddRange(FinalizeRendering(layout.Tabs));
            children.AddRange(FinalizeRendering(Props.Pages));
        }

        private IRenderElement PageTab(NotebookPage page)
        {
            ButtonElementProps btnProps = new ButtonElementProps
            {
                Text = page.Props.Title,
                OnFocus = e => SetActivePage(page),
                OnAccept = e => e.System.SetFocus(page),
            };

            if (page.Props.PageHeader != null)
            {
                btnProps.Children = new List<IRenderable> { page.Props.PageHeader };
            }
            else
            {
                btnProps.Children = new List<IRenderable> { new LabelElement(new LabelElementProps { Text = page.Props.Title }) };
            }

            return new ButtonElement(btnProps);
        }

        private void SetActivePage(NotebookPage page)
        {
            int newIndex = Props.Pages.IndexOf(page);

            if (State.ActivePageIndex == newIndex)
                return;

            SetState(s => s.ActivePageIndex = Props.Pages.IndexOf(page));
        }

        protected override ChildReconciliationMode ChildReconciliationMode
            => ChildReconciliationMode.Self;

        protected override void ReconcileChildren(IRenderElement other)
        {
            OnFinalizeChildren();
        }
    }

    public class NotebookProps : RenderElementProps
    {
        public List<NotebookPage> Pages { get; set; }

        public int InitialActivePage { get; set; }

        protected override bool CanDoValueComparison(PropertyInfo property)
        {
            if (property.Name == nameof(Pages))
                return false;

            return base.CanDoValueComparison(property);
        }
    }

    public class NotebookState
    {
        public int ActivePageIndex { get; set; }
    }
}
