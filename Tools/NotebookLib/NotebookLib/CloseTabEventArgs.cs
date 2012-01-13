using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NotebookLib
{
    public class ClosePageQueryEventArgs : CancelEventArgs 
    {
        internal ClosePageQueryEventArgs(NotebookPage page)
        {
            mPage = page;
        }

        private NotebookPage mPage;
        public NotebookPage NotebookPage
        {
            get { return mPage; }
        }

    }
    public class ClosePageEventArgs : EventArgs
    {
        internal ClosePageEventArgs(NotebookPage page)
        {
            mPage = page;
        }

        private NotebookPage mPage;
        public NotebookPage NotebookPage
        {
            get { return mPage; }
        }

    }

    public delegate void ClosePageQueryHandler(object sender, ClosePageQueryEventArgs args);
    public delegate void ClosePageHandler(object sender, ClosePageEventArgs args);


}
