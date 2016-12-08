using System;
using AgateLib.UserInterface;

namespace AgateLib.Resources.Managers.UserInterface
{
	public interface IUserInterfaceResourceManager : IDisposable
	{
		void InitializeFacet(IUserInterfaceFacet facet);
	}
}