namespace AgateLib.UserInterface
{
    public class NotebookPage : BinElement<NotebookPageProps>
    {
        public NotebookPage(NotebookPageProps props) : base(props)
        {
        }

        public override void OnChildAction(IRenderElement child, UserInterfaceActionEventArgs action)
        {
            base.OnChildAction(child, action);

            if (action.Action == UserInterfaceAction.Cancel)
            {
                Display.System.SetFocus(Parent);
            }
        }

        public override void OnFocus()
        {
            if (Child.CanHaveFocus)
            {
                Display.System.SetFocus(Child);
            }
        }

        //public override string ToString()
        //    => $"notebookpage:{Props.Title}";
    }

    public class NotebookPageProps : BinElementProps
    {
        /// <summary>
        /// Sets the text to display for the page header. 
        /// If <c>PageHeader</c> is set, this property will be
        /// ignored.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Set this property to customize how the tab for this page looks.
        /// </summary>
        public IRenderable PageHeader { get; set; }
    }
}
