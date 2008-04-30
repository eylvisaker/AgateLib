using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ERY.NotebookLib
{
    public class ClosePageEventArgs : CancelEventArgs 
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

    public delegate void ClosePageHandler(object sender, ClosePageEventArgs args);

}
