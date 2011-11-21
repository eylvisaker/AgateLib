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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateOTK
{
	abstract class PrimitiveRenderer
	{
		public abstract void DrawLine(Point a, Point b, Color color);

		public abstract void DrawRect(RectangleF rect, Color color);
		public abstract void FillRect(RectangleF rect, Color color);
		public abstract void FillRect(RectangleF rect, Gradient color);

		public abstract void FillPolygon(PointF[] pts, int startIndex, int length, Color color);

	}
}
