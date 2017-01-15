using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib
{
	public interface IScreenConfiguration
	{
		IReadOnlyList<ScreenInfo> Screens { get; }

		ScreenInfo PrimaryScreen { get; }
	}
}
