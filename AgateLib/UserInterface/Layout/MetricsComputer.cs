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
using AgateLib.Geometry;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Layout.MetricsCalculators;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Layout
{
	public class MetricsComputer
	{
		private AgateWidgetAdapter adapter;
		IWidgetMetricsCalculator defaultCalculator = new DefaultMetricsCalculator();

		public MetricsComputer(AgateWidgetAdapter adapter)
		{
			this.adapter = adapter;
		}

		public bool ComputeNaturalSize(Widget item, WidgetStyle style)
		{
			var calculator = FindCalculator(item);

			return calculator.ComputeNaturalSize(style);
		}
		
		private IWidgetMetricsCalculator FindCalculator(Widget item)
		{
			return defaultCalculator;
		}
		

		public bool ComputeBoxSize(WidgetStyle widget, int? maxWidth, int? maxHeight)
		{
			var calculator = FindCalculator(widget.Widget);

			return calculator.ComputeBoxSize(widget, maxWidth, maxHeight);
		}
	}
}
