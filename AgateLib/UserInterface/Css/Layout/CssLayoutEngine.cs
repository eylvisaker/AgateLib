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
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout
{
	public class CssLayoutEngine : IGuiLayoutEngine
	{
		private CssAdapter mAdapter;

		public CssLayoutEngine(CssAdapter adapter)
		{
			this.mAdapter = adapter;
		}
		public void UpdateLayout(Gui gui)
		{
			UpdateLayout(gui, Display.Coordinates.Size);
		}
		public void UpdateLayout(Gui gui, Size renderTargetSize)
		{
			bool totalRefresh = false;

			totalRefresh |= gui.Desktop.Width != renderTargetSize.Width;
			totalRefresh |= gui.Desktop.Height != renderTargetSize.Height;
			totalRefresh |= gui.Desktop.LayoutDirty;

			gui.Desktop.Width = renderTargetSize.Width;
			gui.Desktop.Height = renderTargetSize.Height;

			SetDesktopAnimatorProperties(gui.Desktop);

			RedoLayout(gui.Desktop, totalRefresh);

			if (totalRefresh || gui.Desktop.Descendants.Any(x => x.LayoutDirty))
			{
				RedoFixedLayout(gui.Desktop);

				gui.Desktop.LayoutDirty = false;
				foreach (var w in gui.Desktop.Descendants)
					w.LayoutDirty = false;
			}
		}

		private void SetDesktopAnimatorProperties(Desktop desktop)
		{
			var style = mAdapter.GetStyle(desktop);

			style.Animator.ClientRect = new Rectangle(0, 0, desktop.Width, desktop.Height);
		}

		private void RedoFixedLayout(Desktop desktop)
		{
			var deskStyle = mAdapter.GetStyle(desktop);
			deskStyle.Animator.ClientRect = new Rectangle(0, 0, desktop.Width, desktop.Height);

			foreach (var child in desktop.Descendants.Where(x => {
				var pos = mAdapter.GetStyle(x).Data.Position;
				return pos == CssPosition.Absolute || pos == CssPosition.Fixed;} ))
			{
				var style = mAdapter.GetStyle(child);
				var sz = style.Animator.ClientRect.Size;
				var box = style.BoxModel;

				switch (style.Data.Position)
				{
					case CssPosition.Absolute:
					case CssPosition.Fixed:
						SetFixedCoordinates(style, box, sz);
						break;
				}
			}
		}
		private void SetFixedCoordinates(CssStyle style, CssBoxModel box, Size sz)
		{
			var anim = style.Animator;
			var position = style.Data.PositionData;
			var parentStyle = mAdapter.GetStyle(style.Animator.ParentCoordinateSystem);

			if (position.Left.Automatic == false)
			{
				var targetLeft = ConvertDistance(style.Widget, position.Left, true, false).Value;

				anim.ClientX = targetLeft + box.Left;
			}
			if (position.Right.Automatic == false)
			{
				int targetRight = ConvertDistance(style.Widget, position.Right, true, false).Value;
				targetRight = parentStyle.Animator.ClientRect.Width - targetRight;

				anim.ClientX = targetRight - anim.ClientRect.Width - box.Right;
			}
			if (position.Top.Automatic == false)
			{
				int targetTop = ConvertDistance(style.Widget, position.Top, false, false).Value;

				anim.ClientY = targetTop + box.Top;
			}
			if (position.Bottom.Automatic == false)
			{
				int targetBottom = ConvertDistance(style.Widget, position.Bottom, false, false).Value;
				targetBottom = parentStyle.Animator.ClientRect.Height - targetBottom;

				anim.ClientY = targetBottom - anim.ClientRect.Height - box.Bottom;
			}
		}

		public CssAdapter Adapter { get { return mAdapter; } }

		private void RedoLayout(Container container, bool forceRefresh = false)
		{
			var containerStyle = mAdapter.GetStyle(container);
			var containerAnim = containerStyle.Animator;
			CssBoxModel containerBox = containerStyle.BoxModel;

			if (forceRefresh == false)
			{
				if (container.LayoutDirty == false &&
					container.Descendants.Any(x => x.LayoutDirty) == false)
				{
					return;
				}
			}

			containerAnim.ClientX = 0;
			containerAnim.ClientY = 0;

			int maxWidth = ComputeMaxWidthForContainer(containerStyle);
			Point nextPos = Point.Empty;
			int maxHeight = 0;

			maxWidth -= containerBox.Left + containerBox.Right;

			int largestWidth = 0;
			int bottom = 0;

			containerAnim.ClientWidth = maxWidth;

			int? fixedContainerWidth = ConvertDistance(container, containerStyle.Data.PositionData.Width, true);
			int? fixedContainerHeight = ConvertDistance(container, containerStyle.Data.PositionData.Height, true);
			if (fixedContainerWidth != null)
				maxWidth = (int)fixedContainerWidth;

			bool resetNextPosition = false;

			foreach (var child in container.Children)
			{
				var style = mAdapter.GetStyle(child);
				child.Font = style.Font;

				if (child.Visible == false)
					continue;
				if (style.Data.Display == CssDisplay.None)
					continue;

				var sz = ComputeSize(child, containerStyle, forceRefresh);
				var box = style.BoxModel;
				int? fixedWidth = ConvertDistance(child, style.Data.PositionData.Width, true);
				int? fixedHeight = ConvertDistance(child, style.Data.PositionData.Height, false);

				if (fixedWidth != null) sz.Width = (int)fixedWidth;
				if (fixedHeight != null) sz.Height = (int)fixedHeight;

				int? minWidth = ConvertDistance(child, style.Data.PositionData.MinWidth, true, true);
				int? minHeight = ConvertDistance(child, style.Data.PositionData.MinHeight, true, true);

				if (minWidth != null && sz.Width < (int)minWidth) sz.Width = (int)minWidth;
				if (minHeight != null && sz.Height < (int)minHeight) sz.Height = (int)minHeight;

				bool resetPosition = false;

				switch (containerStyle.Data.Layout.Kind)
				{
					case CssLayoutKind.Flow:
						if (resetNextPosition)
						{
							resetPosition = true;
							resetNextPosition = false;
						}
						if (style.Data.Display == CssDisplay.Block)
						{
							resetPosition = true;
							resetNextPosition = true;
						}

						if (nextPos.X + sz.Width + style.BoxModel.Left + style.BoxModel.Right > maxWidth)
							resetPosition = true;

						if (resetPosition)
						{
							nextPos.X = 0;
							nextPos.Y += maxHeight;
							maxHeight = 0;
						}
						break;
				}

				var anim = style.Animator;
				anim.IncludeInLayout = true;
				anim.ClientWidth = sz.Width;
				anim.ClientHeight = sz.Height;

				switch (style.Data.Position)
				{
					case CssPosition.Absolute:
						anim.IncludeInLayout = false;
						child.ParentCoordinateSystem = TopLevelWidget(child, x => mAdapter.GetStyle(x).Data.Position != CssPosition.Static);
						break;

					case CssPosition.Fixed:
						anim.IncludeInLayout = false;
						child.ParentCoordinateSystem = TopLevelWidget(child, x => x is Desktop);
						break;
				}

				if (anim.IncludeInLayout)
				{
					anim.ClientX = nextPos.X + box.Left;
					anim.ClientY = nextPos.Y + box.Top;
				}

				anim.ClientWidgetOffset = new Point(
					box.Padding.Left + box.Border.Left,
					box.Padding.Top + box.Border.Top);

				if (anim.IncludeInLayout)
				{
					switch (containerStyle.Data.Layout.Kind)
					{
						case CssLayoutKind.Flow:
							nextPos.X += anim.ClientRect.Width + box.Left + box.Right;
							largestWidth = Math.Max(largestWidth, nextPos.X);
							break;

						case CssLayoutKind.Column:
							nextPos.X = 0;
							nextPos.Y += anim.ClientRect.Height + box.Top + box.Bottom;
							largestWidth = Math.Max(largestWidth, anim.ClientRect.Width + box.Right + box.Left);
							break;
					}

					maxHeight = Math.Max(maxHeight, anim.ClientRect.Height + box.Top + box.Bottom);
					bottom = Math.Max(bottom, anim.ClientRect.Y + anim.ClientRect.Height + box.Bottom); // only add box.Bottom here, because box.Top is taken into account in child.Y.
				}
			}

			containerAnim.ClientWidth = Math.Min(largestWidth, maxWidth);
			containerAnim.ClientHeight = bottom;

			if (fixedContainerWidth != null)
				containerAnim.ClientWidth = (int)fixedContainerWidth;
			if (fixedContainerHeight != null)
				containerAnim.ClientHeight = (int)fixedContainerHeight;

			switch (containerStyle.Data.Layout.Kind)
			{
				case CssLayoutKind.Column:
					foreach (var child in container.Children)
					{
						var style = mAdapter.GetStyle(child);
						var anim = style.Animator;
						var box = style.BoxModel;
						int width = containerAnim.ClientRect.Width - box.Left - box.Right;

						if (anim.IncludeInLayout == false)
							continue;

						if (style.Data.PositionData.MinWidth.Automatic == false)
						{
							int minwidth = mAdapter.CssDistanceToPixels(containerStyle, style.Data.PositionData.MinWidth, true);
							width = Math.Max(width, minwidth);
						}
						if (style.Data.PositionData.MaxWidth.Automatic == false)
						{
							int maxwidth = mAdapter.CssDistanceToPixels(containerStyle, style.Data.PositionData.MaxWidth, true);
							width = Math.Min(width, maxwidth);
						}

						anim.ClientWidth = width;
					}
					break;
			}

		}

		private Container TopLevelWidget(Widget child)
		{
			return TopLevelWidget(child, x => true);
		}
		private Container TopLevelWidget(Widget child, Func<Widget, bool> validMatch)
		{
			var retval = child.Parent;

			if (retval == null)
				return null;

			if (validMatch(retval))
				return retval;

			return TopLevelWidget(retval, validMatch);
		}

		private int ComputeMaxWidthForContainer(CssStyle style)
		{
			Container container = (Container)style.Widget;
			CssStyleData styleData = style.Data;

			if (container.Parent == null)
				return container.Width;

			var parentStyle = mAdapter.GetStyle(container.Parent);

			int availableWidth = parentStyle.Animator.ClientRect.Width - container.X;

			if (styleData.PositionData.MaxWidth.Automatic)
				return availableWidth;
			else
			{
				int maxWidth = mAdapter.CssDistanceToPixels(style, styleData.PositionData.MaxWidth, true);

				return Math.Min(availableWidth, maxWidth);
			}
		}

		private Size ComputeSize(Widget control, CssStyle parentStyle, bool forceRefresh)
		{
			return ComputeSize(control, parentStyle.Data, forceRefresh);
		}
		private Size ComputeSize(Widget control, CssStyleData parentStyle, bool forceRefresh)
		{
			if (control is Container)
				return ComputeContainerSize((Container)control, forceRefresh);

			mAdapter.SetFont(control);
			var style = mAdapter.GetStyle(control);

			return control.ComputeSize(
				ConvertDistance(control, style.Data.PositionData.MinWidth, true),
				ConvertDistance(control, style.Data.PositionData.MinHeight, false),
				ConvertDistance(control, style.Data.PositionData.MaxWidth, true),
				ConvertDistance(control, style.Data.PositionData.MaxHeight, false));
		}

		private int? ConvertDistance(Widget control, CssDistance cssDistance, bool width, bool autoIsNull = true)
		{
			if (cssDistance.Automatic && autoIsNull)
				return null;

			return mAdapter.CssDistanceToPixels(control, cssDistance, width);
		}

		private Size ComputeContainerSize(Container container, bool forceRefresh)
		{
			RedoLayout(container, forceRefresh);
			var anim = mAdapter.GetStyle(container).Animator;

			return new Size(anim.ClientRect.Width, anim.ClientRect.Height);
		}
	}
}
