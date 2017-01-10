using System;

namespace AgateLib.Resources
{
	public interface IAgateResourceManager : IDisposable
	{
		void InitializeContainer(object container);
	}
}