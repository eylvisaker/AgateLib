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