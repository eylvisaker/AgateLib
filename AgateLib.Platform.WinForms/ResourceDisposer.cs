using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms
{
	/// <summary>
	/// Disposes of resources when the end of a using block is reached.
	/// </summary>
	public class ResourceDisposer : IDisposable
	{
		private readonly List<IDisposable> resources;

		public ResourceDisposer(params IDisposable[] resources)
		{
			this.resources = resources.ToList();
		}

		public void Dispose()
		{
			foreach (var resource in resources)
			{
				resource?.Dispose();
			}
		}
	}
}
