using AgateLib.Geometry;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Provides a simple implementation of IMessageTheme.
	/// </summary>
	public class MessageTheme : IMessageTheme
	{
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