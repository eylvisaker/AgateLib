using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.Venus.Hierarchy;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.RenderModel
{
	public class RenderItem
	{
		private readonly Widget widget;
		private readonly WidgetProperties properties;

		private RenderItem parentCoordinates;

		private Rectangle mClientRect;

		public RenderItem(Widget widget, WidgetProperties properties)
		{
			this.widget = widget;
			this.properties = properties;
		}

		public int X
		{
			get { return mClientRect.X; }
			set { mClientRect.X = value; }
		}
		public int Y
		{
			get { return mClientRect.Y; }
			set { mClientRect.Y = value; }
		}
		public int Width
		{
			get { return mClientRect.Width; }
			set { mClientRect.Width = value; }
		}
		public int Height
		{
			get { return mClientRect.Height; }
			set { mClientRect.Height = value; }
		}

		public List<RenderItem> Children { get; private set; } = new List<RenderItem>();
		public RenderItem Parent { get; set; }
		public RenderItem ParentCoordinateSystem
		{
			get
			{
				return parentCoordinates ?? Parent;
			}
			set { parentCoordinates = value; }
		}

		public Rectangle ClientToScreen(Rectangle value, bool translateForScroll = true)
		{
			Rectangle translated = value;

			translated.Location = ClientToScreen(value.Location, translateForScroll);

			return translated;
		}
		public Point ClientToScreen(Point clientPoint, bool translateForScroll = true)
		{
			if (Parent == null)
				return clientPoint;

			Point translated = ClientToParent(clientPoint);

			if (translateForScroll)
			{
				translated.X -= ScrollOffset.X;
				translated.Y -= ScrollOffset.Y;
			}

			return ParentCoordinateSystem.ClientToScreen(translated);
		}

		public Point ClientToParent(Point clientPoint)
		{
			Point translated = clientPoint;

			translated.X += X;
			translated.Y += Y;

			return translated;
		}
		public Point ParentToClient(Point parentClientPoint)
		{
			Point translated = parentClientPoint;

			translated.X -= X;
			translated.Y -= Y;

			return translated;
		}

		public Point ScrollOffset
		{
			get
			{
				var container = widget as Container;
				if (container == null)
					return Point.Empty;
				return container.ScrollOffset;
			}
		}
	}
}
