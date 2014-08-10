using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering
{
	public interface ICssImageProvider
	{
		AgateLib.DisplayLib.Surface GetImage(string name);
	}
}
