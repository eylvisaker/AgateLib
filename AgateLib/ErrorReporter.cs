//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.IO;

namespace AgateLib
{
	/// <summary>
	/// Class which is used to handle error reports.
	/// </summary>
	public class ErrorReporter
	{
		private bool wroteHeader;

		/// <summary>
		/// Gets or sets the file name to which errors are recorded.
		/// Defaults to "errorlog.txt"
		/// </summary>
		public string ErrorFile { get; set; } = "logs/errorlog.txt";

		/// <summary>
		/// Gets or sets whether or not a stack trace is automatically used.
		/// </summary>
		/// <example>
		/// You may find it useful to turn this on during a debug build, and
		/// then turn if off when building the release version.  The following
		/// code accomplishes that.
		/// <code>
		/// #if _DEBUG
		///     AgateLib.AgateApp.ErrorReporting.AutoStackTrace = false;
		/// #endif
		/// </code>
		/// </example>
		public bool AutoStackTrace { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating how AgateLib should deal with issues that may
		/// cause problems when porting to another platform.
		/// </summary>
		public CrossPlatformDebugLevel CrossPlatformDebugLevel { get; set; } = CrossPlatformDebugLevel.Comment;

		/// <summary>
		/// Saves an error message to the ErrorFile.
		/// It is recommended to use an overload which takes an exception parameter,
		/// if there is an exception available which provides more information.
		/// </summary>
		/// <param name="level"></param>
		/// <param name="message"></param>
		public void Report(ErrorLevel level, string message)
		{
			Report(level, message, null);
		}
		/// <summary>
		/// Saves an error message to the ErrorFile.
		/// Outputs a stack trace and shows a dialog box if the ErrorLevel 
		/// is Bug or Fatal.
		/// </summary>
		/// <param name="message">A message to print out before the 
		/// exception's message.</param>
		/// <param name="e"></param>
		/// <param name="level"></param>
		public void Report(ErrorLevel level, string message, Exception e)
		{
			switch (level)
			{
				case ErrorLevel.Bug:
				case ErrorLevel.Fatal:
					Report(level, message, e, true, true);
					break;

				case ErrorLevel.Comment:
				case ErrorLevel.Warning:
					Report(level, message, e, AutoStackTrace, false);
					break;
			}
		}

		/// <summary>
		/// Saves an error message to the ErrorFile.
		/// </summary>
		/// <param name="message">A message to print out before the 
		/// exception's message.</param>
		/// <param name="e"></param>
		/// <param name="level"></param>
		/// <param name="printStackTrace">Bool value indicating whether or not 
		/// a stack trace should be written out.  </param>
		/// <param name="showDialog">Bool value indicating whether or not a 
		/// message box should pop up with an OK button, informing the user about the 
		/// error.  If false, the error is silently written to the ErrorFile.</param>
		public void Report(ErrorLevel level, string message, Exception e, bool printStackTrace, bool showDialog)
		{
			StringBuilder b = new StringBuilder();

			b.Append(LevelText(level));
			b.Append(": ");
			b.AppendLine(message);

			if (e != null)
			{
				b.Append(e.GetType().Name);
				b.Append(": ");
				b.AppendLine(e.Message);

				if (printStackTrace)
					b.AppendLine(e.StackTrace);
			}

			b.AppendLine();

			string text = b.ToString();

			// show the error dialog if AgateWinForms.dll is present.
			//if (showDialog && Drivers.Registrar.WinForms != null)
			//{
			//	Drivers.Registrar.WinForms.ShowErrorDialog(message, e, level);
			//}

			using (StreamWriter filewriter = OpenErrorFile())
			{
				filewriter?.Write(text);
			}

			Log.WriteLine(text);
		}

		/// <summary>
		/// Reports a cross platform error, according to the setting of AgateApp.CrossPlatformDebugLevel.
		/// </summary>
		/// <param name="message"></param>
		public void ReportCrossPlatformError(string message)
		{
			switch (CrossPlatformDebugLevel)
			{
				case CrossPlatformDebugLevel.Comment:
					Report(ErrorLevel.Warning, message, null);
					break;

				case CrossPlatformDebugLevel.Exception:
					throw new AgateCrossPlatformException(message);

			}
		}

		private StreamWriter OpenErrorFile()
		{
			try
			{
				if (wroteHeader == true)
				{
					Stream stream = AgateApp.UserFiles.OpenWrite(ErrorFile, FileOpenMode.Append);

					return new StreamWriter(stream);
				}
				else
				{
					var stream = AgateApp.UserFiles.OpenWrite(ErrorFile);
					StreamWriter writer = new StreamWriter(stream);

					WriteHeader(writer);

					wroteHeader = true;

					return writer;
				}
			}
			catch (Exception e)
			{
				string message = "Could not open file " + ErrorFile + ".\r\n" +
					"Error message: " + e.Message + "\r\n" +
					"Errors cannot be saved to a text file.";

				Log.WriteLine(message);

				return null;
			}
		}

		private void WriteHeader(StreamWriter writer)
		{
			writer.WriteLine("Error Log started " + DateTime.Now.ToString());
			writer.WriteLine("");
		}

		private string LevelText(ErrorLevel level)
		{
			switch (level)
			{
				case ErrorLevel.Comment: return "COMMENT";
				case ErrorLevel.Warning: return "WARNING";
				case ErrorLevel.Fatal: return "ERROR";
				case ErrorLevel.Bug: return "BUG";
			}

			return "ERROR";
		}
	}
}
