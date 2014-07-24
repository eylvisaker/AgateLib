//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Platform.WindowsForms.WinForms
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
			const MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
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
