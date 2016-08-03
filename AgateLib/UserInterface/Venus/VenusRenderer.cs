using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class VenusRenderer : IGuiRenderer
	{
		private Gui myGui;
		private Gesture activeGesture;
		private bool inTransition;

		public Gui MyGui
		{
			get { return myGui; }
			set { myGui = value; }
		}

		public void Draw()
		{
		}

		public void Update(double deltaTime)
		{
		}

		public Gesture ActiveGesture
		{
			get { return activeGesture; }
			set { activeGesture = value; }
		}

		public bool InTransition
		{
			get { return inTransition; }
		}
	}
}
