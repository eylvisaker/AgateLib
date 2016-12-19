using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.Sprites;
using AgateLib.UserInterface.Layout;

namespace AgateLib.Resources.Managers.Display
{
	public interface IDisplayResourceManager : IFontProvider, IDisposable
	{
		void InitializeContainer(object container);

		ISprite GetSprite(string name);
	}
}
