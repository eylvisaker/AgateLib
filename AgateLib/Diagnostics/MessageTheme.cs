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
using AgateLib.DisplayLib;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Provides a simple implementation of IMessageTheme.
	/// </summary>
	public class MessageTheme : IMessageTheme
	{
		/// <summary>
		/// Constructs a MessageTheme object.
		/// </summary>
		/// <param name="foreColor"></param>
		/// <param name="backColor"></param>
		public MessageTheme(Color foreColor, Color? backColor = null)
		{
			ForeColor = foreColor;
			BackColor = backColor ?? BackColor;
		}

		/// <summary>
		/// Text color for the message.
		/// </summary>
		public Color ForeColor { get; set; } = Color.White;

		/// <summary>
		/// Background color for the message.
		/// </summary>
		public Color BackColor { get; set; } = Color.FromArgb(0, 0, 0, 0);
	}


	/// <summary>
	/// Interface for an object which defines the theme for a single message.
	/// </summary>
	public interface IMessageTheme
	{
		/// <summary>
		/// Text color for the message.
		/// </summary>
		Color ForeColor { get; }

		/// <summary>
		/// Background color for the message.
		/// </summary>
		Color BackColor { get; }
	}

}