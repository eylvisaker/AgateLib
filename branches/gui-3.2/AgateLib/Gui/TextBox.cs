using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
	public class TextBox : Widget
	{
		int mInsertionPoint;

		public override bool CanHaveFocus
		{
			get { return true; }
		}
		public int InsertionPoint
		{
			get { return mInsertionPoint; }
			set
			{
				if (value < 0) value = 0;
				if (value > Text.Length) value = Text.Length;

				mInsertionPoint = value;
			}
		}
		protected internal override void SendKeyDown(AgateLib.InputLib.InputEventArgs e)
		{
			ResetIPBlinking();

			switch (e.KeyCode)
			{
				case AgateLib.InputLib.KeyCode.Shift:
				case AgateLib.InputLib.KeyCode.ShiftLeft:
				case AgateLib.InputLib.KeyCode.ShiftRight:
				case AgateLib.InputLib.KeyCode.Control:
				case AgateLib.InputLib.KeyCode.ControlLeft:
				case AgateLib.InputLib.KeyCode.ControlRight:
				case AgateLib.InputLib.KeyCode.Alt:
				case AgateLib.InputLib.KeyCode.CapsLock:
					return;

				case AgateLib.InputLib.KeyCode.Left:
					InsertionPoint--;
					break;

				case AgateLib.InputLib.KeyCode.Right:
					InsertionPoint++;
					break;

				case AgateLib.InputLib.KeyCode.Home:
					InsertionPoint = 0;
					break;

				case AgateLib.InputLib.KeyCode.End:
					InsertionPoint = Text.Length;
					break;

				case AgateLib.InputLib.KeyCode.BackSpace:
					if (InsertionPoint == 0)
						break;

					if (InsertionPoint == Text.Length)
					{
						InsertionPoint--;
						Text = Text.Substring(0, Text.Length - 1);
					}
					else
					{
						InsertionPoint--;
						Text = Text.Substring(0, InsertionPoint) + Text.Substring(InsertionPoint + 1);
					}

					break;

				case AgateLib.InputLib.KeyCode.Delete:
					if (InsertionPoint == Text.Length)
						break;
					else if (InsertionPoint == 0)
						Text = Text.Substring(1);
					else
						Text = Text.Substring(0, InsertionPoint) + Text.Substring(InsertionPoint + 1);

					break;

				default:
					if (InsertionPoint == Text.Length)
						Text += e.KeyString;
					else if (InsertionPoint == 0)
						Text = e.KeyString + Text;
					else
						Text = Text.Substring(0, InsertionPoint) + e.KeyString + Text.Substring(InsertionPoint);

					InsertionPoint++;

					break;
			}

			SetDirty();
		}
		protected internal override bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
		{
			switch (keyCode)
			{
				case AgateLib.InputLib.KeyCode.Left:
				case AgateLib.InputLib.KeyCode.Right:
					return true;
				case AgateLib.InputLib.KeyCode.Up:
				case AgateLib.InputLib.KeyCode.Down:
				case AgateLib.InputLib.KeyCode.Enter:
					if (MultiLine)
						return true;
					else
						return false;

				default:
					return false;
			}
		}
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;

				if (InsertionPoint > Text.Length)
					InsertionPoint = Text.Length;

				ResetIPBlinking();

				SetDirty();
			}
		}

		private void SetDirty()
		{
			if (Cache == null)
				return;

			Cache.Dirty = true;
		}

		private void ResetIPBlinking()
		{
			IPTime = Timing.TotalMilliseconds;
		}

		internal double IPTime { get; set; }

		public bool MultiLine { get; set; }

		protected internal override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SetDirty();
		}
	}
}
