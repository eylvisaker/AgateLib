using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
	public interface ISurfaceProvider
	{
		Surface GetSurface(string name);
		FontSurface GetFont(string name);
	}
}
