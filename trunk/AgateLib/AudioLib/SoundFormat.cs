using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.AudioLib
{
	/// <summary>
	/// Class describing what format the raw audio data is in.
	/// </summary>
	public class SoundFormat
	{
		public SoundFormat()
		{
			BitsPerSample = 16;
			Channels = 1;
			SamplingFrequency = 44100;
		}

		public int BitsPerSample { get; set; }
		public int Channels { get; set; }
		public int SamplingFrequency { get; set; }

		/// <summary>
		/// Creates and returns a SoundFormat object
		/// for a 16-bit, single channel stream at the 
		/// specified sampling frequency.
		/// </summary>
		/// <param name="samplingFrequency">The sampling frequency for the stream.</param>
		/// <returns></returns>
		public static SoundFormat Pcm16(int samplingFrequency)
		{
			return new SoundFormat { BitsPerSample = 16, Channels = 1, SamplingFrequency = samplingFrequency };
		}
	}
}
