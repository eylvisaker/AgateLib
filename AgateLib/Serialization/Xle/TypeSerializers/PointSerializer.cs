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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle.TypeSerializers
{
	[Obsolete]
	class PointSerializer : XleTypeSerializerBase<Point>
	{
		public override void Serialize(XleSerializationInfo info, Point value)
		{
			info.Write("X", value.X, true);
			info.Write("Y", value.Y, true);
			
		}

		public override Point Deserialize(XleSerializationInfo info)
		{
			return new Point()
			{
				X = info.ReadInt32("X"),
				Y = info.ReadInt32("Y"),
			};
		}
	}
}
