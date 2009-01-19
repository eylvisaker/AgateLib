using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.WinForms
{
    using Drivers;

    class WinFormsDriver : IDesktopDriver
    {

        #region IWinForms Members

        public IUserSetSystems CreateUserSetSystems()
        {
            return new SetSystemsForm();
        }

        public void ShowErrorDialog(string message, Exception e, ErrorLevel level)
        {
            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
            System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.Asterisk;

            switch (level)
            {
                case ErrorLevel.Comment:
                    icon = System.Windows.Forms.MessageBoxIcon.Information;
                    break;

                case ErrorLevel.Warning:
                    icon = System.Windows.Forms.MessageBoxIcon.Warning;
                    break;

                case ErrorLevel.Fatal:
                    icon = System.Windows.Forms.MessageBoxIcon.Error;
                    break;

                case ErrorLevel.Bug:
                    icon = System.Windows.Forms.MessageBoxIcon.Hand;

                    break;
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("An error has occured:");
            builder.AppendLine(message);

            if (e != null)
            {
                builder.AppendLine(e.Message);
                builder.AppendLine();
                builder.AppendLine(e.StackTrace);
            }

            System.Windows.Forms.MessageBox.Show
                (builder.ToString(), level.ToString(), buttons, icon);
        }

        #endregion
    }
}
