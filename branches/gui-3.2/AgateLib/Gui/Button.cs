using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.InputLib;

namespace AgateLib.Gui
{
	public class Button : Widget
	{
		public Button() { Name = "Button"; }
		public Button(string text) { Name = text; Text = text; }

		bool spaceDownFocus = false;

		public override bool CanHaveFocus
		{
			get
			{
				return true;
			}
		}
		internal bool DrawActivated
		{
			get { return MouseDownIn && MouseIn || spaceDownFocus; }
		}
		internal bool IsDefaultButton
		{
			get
			{
				Container p = this.Parent;

				while (p is Window == false)
					p = p.Parent;

				return ((Window)p).AcceptButton == this;
			}
		}

		protected internal override void OnKeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				spaceDownFocus = true;
			}

			base.OnKeyDown(e);
		}
		protected internal override void OnKeyUp(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				spaceDownFocus = false;
				OnClick();
			}

			base.OnKeyUp(e);
		}
		protected internal override void SendMouseUp(InputEventArgs e)
		{
			if (MouseIn && MouseDownIn)
				OnClick();

			base.SendMouseUp(e);
		}

		private void OnClick()
		{
			if (Click != null)
				Click(this, EventArgs.Empty);
		}
		public event EventHandler Click;
	}
}
