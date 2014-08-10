using AgateLib.DisplayLib;
using AgateLib.Geometry;
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
		public void RedoLayout(Gui gui)
		{
			RedoLayout(gui, Display.RenderTarget.Size);
		}
		public void RedoLayout(Gui gui, Size renderTargetSize)
		{
			gui.Desktop.Width = renderTargetSize.Width;
			gui.Desktop.Height = renderTargetSize.Height;

			RedoLayout(gui.Desktop);
		}

		private void RedoLayout(Container container)
		{
			var containerStyle = mAdapter.GetStyle(container);
			var containerAnim = containerStyle.Animator;
			CssBoxModel containerBox = containerStyle.BoxModel;

			containerAnim.ClientRect.X = 0;
			containerAnim.ClientRect.Y = 0;

			int maxWidth = ComputeMaxWidthForContainer(containerStyle);
			Point nextPos = Point.Empty;
			int maxHeight = 0;

			maxWidth -= containerBox.Left + containerBox.Right;
			
			int largestWidth = 0;
			int bottom = 0;

			containerAnim.ClientRect.Width = maxWidth;

			int? fixedContainerWidth = ConvertDistance(container, containerStyle.Data.Position.Width, true);
			int? fixedContainerHeight = ConvertDistance(container, containerStyle.Data.Position.Height, true);
			if (fixedContainerWidth != null)
				maxWidth = (int)fixedContainerWidth;

			foreach (var child in container.Children)
			{
				var style = mAdapter.GetStyle(child);
				child.Font = style.Font;

				var sz = ComputeSize(child, containerStyle);
				var box = style.BoxModel;
				int? fixedWidth = ConvertDistance(child, style.Data.Position.Width, true);
				int? fixedHeight = ConvertDistance(child, style.Data.Position.Height, false);

				if (fixedWidth != null) sz.Width = (int)fixedWidth;
				if (fixedHeight != null) sz.Height = (int)fixedHeight;

				switch (containerStyle.Data.Layout.Kind)
				{
					case CssLayoutKind.Flow:
						if (nextPos.X + sz.Width + style.BoxModel.Left + style.BoxModel.Right > maxWidth)
						{
							nextPos.X = 0;
							nextPos.Y += maxHeight;
							maxHeight = 0;
						}
						break;
				}

				var anim = style.Animator;

				anim.ClientRect.X = nextPos.X + box.Left;
				anim.ClientRect.Y = nextPos.Y + box.Top;
				anim.ClientRect.Width = sz.Width;
				anim.ClientRect.Height = sz.Height;

				anim.ClientWidgetOffset = new Point(
					box.Padding.Left + box.Border.Left,
					box.Padding.Top + box.Border.Top);

				anim.WidgetSize = new Size(
					anim.ClientRect.Width + box.Padding.Left + box.Padding.Right + box.Border.Left + box.Border.Right,
					anim.ClientRect.Height + box.Padding.Top + box.Padding.Bottom + box.Border.Bottom + box.Border.Top);

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

			containerAnim.ClientRect.Width = Math.Min(largestWidth, maxWidth);
			containerAnim.ClientRect.Height = bottom;

			if (fixedContainerWidth != null)
				containerAnim.ClientRect.Width = (int)fixedContainerWidth;
			if (fixedContainerHeight != null)
				containerAnim.ClientRect.Height = (int)fixedContainerHeight;

			switch (containerStyle.Data.Layout.Kind)
			{
				case CssLayoutKind.Column:
					foreach (var child in container.Children)
					{
						var style = mAdapter.GetStyle(child);
						var anim = style.Animator;
						var box = style.BoxModel;
						int width = containerAnim.ClientRect.Width - box.Left - box.Right;

						if (style.Data.Position.MinWidth.Automatic == false)
						{
							int minwidth = mAdapter.CssDistanceToPixels(containerStyle, style.Data.Position.MinWidth, true);
							width = Math.Max(width, minwidth);
						}
						if (style.Data.Position.MaxWidth.Automatic == false)
						{
							int maxwidth = mAdapter.CssDistanceToPixels(containerStyle, style.Data.Position.MaxWidth, true);
							width = Math.Min(width, maxwidth);
						}

						anim.ClientRect.Width = width;
					}
					break;
			}
		}

		private int ComputeMaxWidthForContainer(CssStyle style)
		{
			Container container = (Container)style.Widget;
			CssStyleData styleData = style.Data;

			if (container.Parent == null)
				return container.Width;

			var parentStyle = mAdapter.GetStyle(container.Parent);

			int availableWidth = parentStyle.Animator.ClientRect.Width - container.X;

			if (styleData.Position.MaxWidth.Automatic)
				return availableWidth;
			else
			{
				int maxWidth = mAdapter.CssDistanceToPixels(style, styleData.Position.MaxWidth, true);

				return Math.Min(availableWidth, maxWidth);
			}
		}

		private Size ComputeSize(Widget control, CssStyle parentStyle)
		{
			return ComputeSize(control, parentStyle.Data);
		}
		private Size ComputeSize(Widget control, CssStyleData parentStyle)
		{
			if (control is Container)
				return ComputeContainerSize((Container)control);

			var style = mAdapter.GetStyle(control);

			return control.ComputeSize(
				ConvertDistance(control, style.Data.Position.MinWidth, true),
				ConvertDistance(control, style.Data.Position.MinHeight, false),
				ConvertDistance(control, style.Data.Position.MaxWidth, true),
				ConvertDistance(control, style.Data.Position.MaxHeight, false));
		}

		private int? ConvertDistance(Widget control, CssDistance cssDistance, bool width, bool autoIsNull = true)
		{
			if (cssDistance.Automatic && autoIsNull)
				return null;

			return mAdapter.CssDistanceToPixels(control, cssDistance, width);
		}

		private Size ComputeContainerSize(Container container)
		{
			RedoLayout(container);
			var anim = mAdapter.GetStyle(container).Animator;

			return new Size(anim.ClientRect.Width, anim.ClientRect.Height);
		}
	}
}
