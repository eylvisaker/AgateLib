//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms
{
	public static class GeometryExtensions
	{
		/// <summary>
		/// Converts an AgateLib point object to a System.Drawing point object.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public static System.Drawing.Point ToDrawingPoint(this AgateLib.Geometry.Point point)
		{
			return new System.Drawing.Point(point.X, point.Y);
		}

		/// <summary>
		/// Converts an AgateLib size object to a System.Drawing size object.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static System.Drawing.Size ToDrawingSize(this AgateLib.Geometry.Size size)
		{
			return new System.Drawing.Size(size.Width, size.Height);
		}

		/// <summary>
		/// Converts an System.Drawing point object to a AgateLib point object.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public static AgateLib.Geometry.Point ToAgatePoint(this System.Drawing.Point point)
		{
			return new AgateLib.Geometry.Point(point.X, point.Y);
		}

	}
}
