﻿//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Geometry.CoordinateSystems
{
	/// <summary>
	/// Constructs a coordinate system which matches the pixels coordinates of the display window,
	/// up to an optional maximum height and width.
	/// </summary>
	public class NativeCoordinates : ICoordinateSystemCreator
	{
		public Rectangle DetermineCoordinateSystem(Size displayWindowSize)
		{
			Rectangle retval = new Rectangle(Point.Empty, displayWindowSize);

			if (MaxSize != null)
			{
				retval.Width = Math.Min(retval.Width, MaxSize.Value.Width);
				retval.Height = Math.Min(retval.Height, MaxSize.Value.Height);
			}

			return retval;
		}

		public Size? MaxSize { get; set; }
	}
}