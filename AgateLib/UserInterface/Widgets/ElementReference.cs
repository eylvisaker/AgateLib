using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    /// <summary>
    /// Class which tracks the final rendered element.
    /// </summary>
    public class ElementReference
    {
        /// <summary>
        /// Gets the current value of the reference.
        /// </summary>
        public IRenderElement Current { get; internal set; }
    }
}
