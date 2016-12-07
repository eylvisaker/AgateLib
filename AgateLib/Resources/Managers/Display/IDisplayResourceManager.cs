using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Sprites;
using AgateLib.UserInterface.Venus;

namespace AgateLib.Resources.Managers.Display
{
	public interface IDisplayResourceManager : IFontProvider
	{
		void InitializeContainer(object container);

		ISprite GetSprite(string name);
	}
}
