using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Drivers
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
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <param name="level"></param>
        void ShowErrorDialog(string message, Exception e, ErrorLevel level);
    }
}
