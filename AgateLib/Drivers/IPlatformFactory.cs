using AgateLib.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Drivers
{
	public interface IPlatformFactory
	{
		PlatformInfo CreatePlatformInfo();

		IStopwatch CreateStopwatch();

		IO.IFile CreateFile();

		Diagnostics.AgateConsole CreateConsole();

		IO.IPath CreatePath();

		IEnumerable<System.Reflection.Assembly> GetSerializationSearchAssemblies(Type objectType);
	}
}
