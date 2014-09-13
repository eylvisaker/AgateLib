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
		public void Draw(Gui gui)
		{
		}
		public void Update(Gui gui, double deltaTime)
		{ }


		public Gesture ActiveGesture { get;set;}
	}
	public class FakeLayout : IGuiLayoutEngine
	{
		public void UpdateLayout(Gui gui)
		{
		}
	}
}
