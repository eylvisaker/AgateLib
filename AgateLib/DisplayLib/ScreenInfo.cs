﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public class ScreenInfo
	{
		public string DeviceName { get; set; }

		public Rectangle Bounds { get; set; }

		public bool IsPrimary { get; set; }

		public IntPtr SystemIndex { get; set; }
	}
}
