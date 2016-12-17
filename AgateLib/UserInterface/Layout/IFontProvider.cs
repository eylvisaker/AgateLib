using AgateLib.DisplayLib;

namespace AgateLib.UserInterface.Layout
{
	public interface IFontProvider
	{
		/// <summary>
		/// Returns a font if it exists. Returns null if not.
		/// </summary>
		/// <param name="family"></param>
		/// <returns></returns>
		IFont FindFont(string family);
	}
}