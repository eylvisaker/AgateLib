﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public interface IFile
	{
		bool Exists(string path);
	}
}
