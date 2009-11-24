using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
	public class CheckBox : Widget
	{
		public CheckBox()
		{
			Name = "Checkbox";
		}
		public CheckBox(string text)
		{
			Name = text;
			Text = text;
		}
		private bool mChecked;

		public bool Checked
		{
			get { return mChecked; }
			set
			{
				mChecked = value;

				OnCheckChanged();
			}
		}

		public override bool CanHaveFocus
		{
			get
			{
				return true;
			}
		}

		bool mouseDownIn;
		protected internal override void OnMouseDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (Enabled == false)
				return;

			mouseDownIn = true;
		}
		protected internal override void OnMouseUp(AgateLib.InputLib.InputEventArgs e)
		{
			if (MouseIn && mouseDownIn)
				Checked = !Checked;

			mouseDownIn = false;

		}

		protected internal override void SendKeyDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (e.KeyCode == AgateLib.InputLib.KeyCode.Space)
			{
				Checked = !Checked;
			}
		}

		private void OnCheckChanged()
		{
			if (CheckChanged != null)
				CheckChanged(this, EventArgs.Empty);
		}

		public event EventHandler CheckChanged;
	}
}
