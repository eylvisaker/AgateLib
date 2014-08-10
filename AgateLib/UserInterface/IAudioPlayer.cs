using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
	public interface IAudioPlayer
	{
		void PlaySound(string p);

		void PlaySound(Widgets.GuiSound sound);
	}
}
