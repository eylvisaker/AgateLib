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
using System.Collections.Generic;
using AgateLib.UserInterface.StyleModel;

namespace AgateLib.UserInterface.Layout
{
	public interface ILayoutAssembler
	{
		/// <summary>
		/// Returns true if this layout assembler can do the layout for the specified widget.
		/// </summary>
		/// <param name="containerStyle">WidgetStyle object for the container.</param>
		/// <returns></returns>
		bool CanDoLayoutFor(WidgetStyle containerStyle);

		/// <summary>
		/// Performs layout fo the specified container and children.
		/// </summary>
		/// <param name="layoutBuilder">The layout builder that will be used to perform layout of child container contents.</param>
		/// <param name="container">The container this object is doing layout for.</param>
		/// <param name="layoutChildren">The container children that participate in layout.</param>
		/// <param name="maxWidth">The maximum width of the container client area.</param>
		/// <param name="maxHeight">The maximum height of the container client area.</param>
		void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren,
			int? maxWidth = null, int? maxHeight = null);

		/// <summary>
		/// Computes the natural size of the widget and assigns it to the widget.Metrics.NaturalBoxSize property.
		/// Returns true if this value is different from the stored value.
		/// </summary>
		/// <param name="layoutBuilder">The layout builder that will be used to perform layout of child container contents.</param>
		/// <param name="widget">The container layout is being done on.</param>
		/// <returns></returns>
		bool ComputeNaturalSize(ILayoutBuilder layoutBuilder, WidgetStyle widget);
	}
}