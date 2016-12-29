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
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public interface IFont : IDisposable
	{
		Color Color { get; set; }
		OriginAlignment DisplayAlignment { get; set; }
		int FontHeight { get; }
		FontStyles Style { get; set; }
		int Size { get; set; }
		TextImageLayout TextImageLayout { get; set; }
		double Alpha { get; set; }
		string Name { get; }

		void DrawText(string text);
		void DrawText(Point dest, string text);
		void DrawText(int x, int y, string text);
		void DrawText(int x, int y, string text, params object[] Parameters);
		void DrawText(double x, double y, string text);
		void DrawText(PointF dest, string text);

		Size MeasureString(string text);
	}
}