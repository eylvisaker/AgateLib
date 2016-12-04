using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DefaultAssets
{
	public class DefaultResources : IDisposable
	{
		internal DefaultResources()
		{ }

		public IFont AgateSans { get; set; }
		public IFont AgateSerif { get; set; }
		public IFont AgateMono { get; set; }

		public void Dispose()
		{
			if (AgateSans != null) AgateSans.Dispose();
			if (AgateSerif != null) AgateSerif.Dispose();
			if (AgateMono != null) AgateMono.Dispose();

			AgateSans = null;
			AgateSerif = null;
			AgateMono = null;
		}
	}
}
