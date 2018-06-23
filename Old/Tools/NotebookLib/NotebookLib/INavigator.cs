using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib
{
    public interface INavigator
    {
        /// <summary>
        /// Return -1 to show no panel.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Override this.  No need to call base.RefreshAllPages().
        /// </summary>
        void RefreshAllPages();

        /// <summary>
        /// Create the properties object for the user to interact with through the
        /// properties window in the designer.
        /// </summary>
        /// <returns></returns>
        NavigatorProperties CreateProperties();

        /// <summary>
        /// Return the minimum size of the navigator.
        /// Note that only the width or height of the returned value is respected,
        /// based on the position of the navigator.
        /// </summary>
        Size NavMinSize { get; }
        /// <summary>
        /// Return the maximum size of the navigator.
        /// Note that only the width or height of the returned value is respected,
        /// based on the position of the navigator.
        /// </summary>
        Size NavMaxSize { get; }

        void OnUpdateLayout();
    }
}
