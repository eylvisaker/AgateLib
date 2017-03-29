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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.Quality;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Provides a simple implementation of IConsoleTheme.
	/// </summary>
	public class ConsoleTheme : IConsoleTheme
	{
		private IDictionary<ConsoleMessageType, MessageTheme> messageThemes =
			new Dictionary<ConsoleMessageType, MessageTheme>();

		/// <summary>
		/// The text which is used to prefix a user entry.
		/// </summary>
		public string EntryPrefix { get; set; } = "> ";

		/// <summary>
		/// Gets or sets the background color for the console.
		/// </summary>
		public Color BackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets the color of recent messages.
		/// </summary>
		public Color RecentMessageColor { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the color of the user's active text input.
		/// </summary>
		public Color EntryColor { get; set; }

		/// <summary>
		/// Indicates the background color for the user's active text input.
		/// </summary>
		public Color EntryBackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets a collection of themes for different message types.
		/// </summary>
		public IDictionary<ConsoleMessageType, MessageTheme> MessageThemes
		{
			get { return messageThemes; }
			set
			{
				Require.ArgumentNotNull(value, nameof(MessageThemes));
				messageThemes = value;
			}
		}

		/// <summary>
		/// Returns true if there is a message theme for every type of message.
		/// </summary>
		public bool IsComplete
		{
			get
			{
				return ((ConsoleMessageType[])Enum.GetValues(typeof(ConsoleMessageType)))
					.All(type => messageThemes.ContainsKey(type));
			}
		}

		IMessageTheme IConsoleTheme.MessageTheme(ConsoleMessage message)
		{
			return MessageThemes[message.MessageType];
		}
	}

	/// <summary>
	/// Interface for an object which defines the theme for the console window.
	/// </summary>
	public interface IConsoleTheme
	{
		/// <summary>
		/// The text which is used to prefix a user entry.
		/// </summary>
		string EntryPrefix { get; }

		/// <summary>
		/// The background color of the console window.
		/// </summary>
		Color BackgroundColor { get; }

		/// <summary>
		/// Indicates the color of the recent message list when the console window is not shown.
		/// </summary>
		Color RecentMessageColor { get; }

		/// <summary>
		/// Indicates the color of the user's active text input.
		/// </summary>
		Color EntryColor { get; }

		/// <summary>
		/// Indicates the background color for the user's active text input.
		/// </summary>
		Color EntryBackgroundColor { get; }

		/// <summary>
		/// Gets the theme data for a message
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		IMessageTheme MessageTheme(ConsoleMessage message);
	}
}
