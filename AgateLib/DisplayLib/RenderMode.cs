using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib
{
	public abstract class RenderMode
	{
		class StretchMode { }

		public string Name { get; }
	}

	public interface IRenderMode
	{
		string Name { get; }
	}
}
