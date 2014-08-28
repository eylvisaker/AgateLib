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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D9;

namespace AgateSDX
{
	/*
	public struct DX_PositionColorNormalTexture
	{
		public float X, Y, Z;
		public float nx, ny, nz;
		public int Color;
		public float Tu, Tv;

		public static VertexFormat Format =
			VertexFormat.PositionNormal | VertexFormat.Diffuse | VertexFormat.Texture1;

		public PositionColorNormalTexture(float x, float y, float z, int color, float tu, float tv,
			float nx, float ny, float nz)
		{
			X = x;
			Y = y;
			Z = z;
			Color = color;
			Tu = tu;
			Tv = tv;
			this.nx = nx;
			this.ny = ny;
			this.nz = nz;
		}

		public override string ToString()
		{
			return string.Format("X: {0} Y: {1} Z: {2} Color: {3} Tu: {4}, Tv: {5}",
				X, Y, Z, Color, Tu, Tv);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PositionColored
	{
		public float x, y, z;
		public int Color;

		public Vector3 Position
		{
			get { return new Vector3(x, y, z); }
			set
			{
				x = value.X;
				y = value.Y;
				z = value.Z;
			}
		}
		public PositionColored(float x, float y, float z, AgateLib.Geometry.Color clr)
			: this(x, y, z, clr.ToArgb())
		{ }
		public PositionColored(float x, float y, float z, int clr)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.Color = clr;
		}
		public static VertexFormat Format =
			VertexFormat.Position | VertexFormat.Diffuse;
	}
	 * */
}
