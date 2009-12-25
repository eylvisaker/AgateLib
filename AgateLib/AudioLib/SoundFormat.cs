using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.AudioLib
{
	/// <summary>
	/// Enum describing what format the audio data is in.
	/// </summary>
	public enum SoundFormat
	{
		/// <summary>
		/// Raw 16 bit signed PCM data.
		/// </summary>
		RawInt16,
		/// <summary>
		/// Wav format.
		/// </summary>
		Wave,
	}
}
