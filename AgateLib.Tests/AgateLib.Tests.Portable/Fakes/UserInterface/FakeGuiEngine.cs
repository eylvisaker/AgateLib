using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.Fakes
{
	public class FakeRenderer : IGuiRenderer
	{
		public void Draw()
		{
		}
		public void Update(double deltaTime)
		{ }


		public Gesture ActiveGesture { get; set; }


		public bool InTransition
		{
			get { return false; }
		}

		public Gui MyGui { get; set; }
	}
}
