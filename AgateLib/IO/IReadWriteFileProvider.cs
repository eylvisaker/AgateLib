using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public interface IReadWriteFileProvider : IReadFileProvider, IWriteFileProvider
	{
	}
}
