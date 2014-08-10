using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class Window : Container
	{
		string mBackgroundStyle = "paper";
		string mFrameStyle = "paper";

		public Window()
		{
		}
		public Window(string name)
		{
		}
		public Window(params Widget[] contents)
			: base(contents)
		{
		}

		public FontSurface FontSmall { get; set; }

		public string BackgroundStyle
		{
			get { return mBackgroundStyle; }
			set
			{
				mBackgroundStyle = value;
			}
		}
		public string FrameStyle
		{
			get { return mFrameStyle; }
			set
			{
				mFrameStyle = value;
			}
		}

		public override void DrawImpl()
		{
			// draw children after drawing frame
			base.DrawImpl();
		}
		public override void Update(double delta_t, ref bool processInput)
		{
			base.Update(delta_t, ref processInput);
		}

		public override void OnInputButtonDown(AgateLib.InputLib.KeyCode button, ref bool handled)
		{
			base.OnInputButtonDown(button, ref handled);
		}

	}
}
