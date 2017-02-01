using System.IO;

namespace AgateLib.Settings
{
	internal interface ISettingsData
	{
		string Filename { get; set; }

		object Data { get; }

		void Save(TextWriter stream);
	}
}