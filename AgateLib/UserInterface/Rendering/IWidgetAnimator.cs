using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public interface IWidgetAnimator
	{
		IList<IWidgetAnimator> Children { get; }
		Rectangle ClientRect { get; set; }
		Gesture Gesture { get; set; }
		bool InTransition { get; }
		bool IsDead { get; set; }
		IWidgetAnimator Parent { get; set; }
		WidgetStyle Style { get; }
		bool Visible { get; set; }
		Widget Widget { get; }
		Rectangle WidgetRect { get; }
		IWidgetAnimator ParentCoordinateSystem { get; }

		void Update(double deltaTime);
		Rectangle ClientToScreen(Rectangle rectangle, bool translateForScroll = true);
		Point ClientToScreen(Point translated, bool translateForScroll = true);
		Point ScreenToClient(Point translated);

		int X { get; set; }
		int Y { get; set; }
		int Width { get; set; }
		int Height { get; set; }
	}
}
