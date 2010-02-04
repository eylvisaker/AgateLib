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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.Gui.Cache;

namespace AgateLib.Gui.ThemeEngines.Mercury.Cache
{
	class ScrollBarCache : WidgetCache
	{
		public bool DownInDecrease { get; set; }
		public bool DownInIncrease { get; set; }
		public bool DownInPageDecrease { get; set; }
		public bool DownInPageIncrease { get; set; }

		public bool MouseInDecrease { get; set; }
		public bool MouseInIncrease { get; set; }
		public bool MouseInPageDecrease { get; set; }
		public bool MouseInPageIncrease { get; set; }

		public double LastUpdate { get; set; }

		public bool DraggingThumb { get; set; }
		public Point ThumbGrabSpot { get; set; }
	}
}
