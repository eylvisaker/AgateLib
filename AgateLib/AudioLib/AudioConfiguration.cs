using AgateLib.Quality;

namespace AgateLib.AudioLib
{
	/// <summary>
	/// Configuration setting for audio.
	/// </summary>
	public class AudioConfiguration
	{
		double musicVolume = 1.0;

		/// <summary>
		/// Gets or sets the master volume for music.
		/// The value must be between 0 and 1.
		/// </summary>
		public double MusicVolume
		{
			get => musicVolume;
			set
			{
				Require.ArgumentInRange(0 <= value && value <= 1, nameof(MusicVolume), "Music Volume must be between 0 and 1.");
			}
		}
	}
}