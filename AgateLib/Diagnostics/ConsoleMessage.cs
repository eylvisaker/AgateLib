//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Content;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Represents a message in the console window.
	/// </summary>
	public class ConsoleMessage
	{
		string text;

		/// <summary>
		/// Gets or sets the text of a console message.
		/// </summary>
		public string Text
		{
			get => text;
			set
			{
				text = value;
				Layout = null;
			}
		}

		/// <summary>
		/// Gets or sets the time the console message was logged.
		/// </summary>
		public long Time { get; set; }

		/// <summary>
		/// Gets or sets the type of console message. This is used to determine how the console message is displayed.
		/// </summary>
		public ConsoleMessageType MessageType { get; set; }

		internal IContentLayout Layout { get; set; }

		internal void ClearCache()
		{
			Layout = null;
		}
	}
}
