using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
	public interface IGuiRenderer
	{
		void Draw(Gui gui);

		void Update(Gui gui, double deltaTime);
	}
}
