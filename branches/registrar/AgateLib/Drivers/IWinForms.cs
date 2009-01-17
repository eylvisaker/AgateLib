using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Drivers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWinForms
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IUserSetSystems CreateUserSetSystems();

        /// <summary>
        /// Shows an error dialog using the operating system's methods.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e">The exception causing the error.  This parameter may be null, 
        /// and it is important that the implementation does not choke on a null value for e.</param>
        /// <param name="level"></param>
        void ShowErrorDialog(string message, Exception e, ErrorLevel level);
    }
}
