using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.InputLib;

namespace AgateLib.Gui
{
	public class TextBox : Widget
	{
		int mInsertionPoint;
		int mSelectionStart;
		int mSelectionLength;

		public override bool CanHaveFocus
		{
			get { return true; }
		}
		public int InsertionPoint
		{
			get { return mInsertionPoint; }
		}
		/// <summary>
		/// Moves the insertion point to a new location.
		/// </summary>
		/// <param name="newLoc">The new location of the insertion point.  This must be between 0
		/// and Text.Length, inclusive.</param>
		/// <param name="expandSel">Pass true to expand the current selection to include text
		/// up to the new location.  Pass false to clear the selection.</param>
		public void MoveInsertionPoint(int newLoc, bool expandSel)
		{
			if (newLoc < 0) newLoc = 0;
			if (newLoc > Text.Length) newLoc = Text.Length;

			if (SelectionLength == 0)
				SelectionStart = InsertionPoint;

			mInsertionPoint = newLoc;
			ResetIPBlinking();

			if (expandSel)
				ExpandSelection();
			else
				SelectionLength = 0;
		}

		protected internal override void SendMouseDown(AgateLib.InputLib.InputEventArgs e)
		{
			base.SendMouseDown(e);
			if (Root != null)
			{
				Root.ThemeEngine.MouseDownInWidget(this, PointToClient(e.MousePosition));
			}
		}
		protected internal override void SendMouseMove(InputEventArgs e)
		{
			base.SendMouseMove(e);
			
		}
		protected internal override void SendMouseUp(InputEventArgs e)
		{
			base.SendMouseUp(e);
			if (Root != null)
			{
				Root.ThemeEngine.MouseUpInWidget(this, PointToClient(e.MousePosition));
			}
		}

		protected internal override void SendKeyDown(AgateLib.InputLib.InputEventArgs e)
		{
			ResetIPBlinking();
			base.SendKeyDown(e);

			switch (e.KeyCode)
			{
				case KeyCode.Shift:
				case KeyCode.ShiftLeft:
				case KeyCode.ShiftRight:
				case KeyCode.Control:
				case KeyCode.ControlLeft:
				case KeyCode.ControlRight:
				case KeyCode.Alt:
				case KeyCode.CapsLock:
					return;

				case KeyCode.Escape:
					DeleteSelectedText();
					break;

				case KeyCode.Up:
					TypeUp();
					break;

				case KeyCode.Down:
					TypeDown();
					break;

				case KeyCode.Left:
					TypeLeft();
					break;

				case KeyCode.Right:
					TypeRight();
					break;

				case KeyCode.Home:
					TypeHome();
					break;

				case KeyCode.End:
					TypeEnd();
					break;

				case KeyCode.BackSpace:
					TypeBackspace();

					break;

				case KeyCode.Delete:
					TypeDelete();
					break;

				default:
					TypeKey(e.KeyString);
					break;
			}

			SetDirty();
		}

		private bool ShiftKey
		{
			get
			{
				return Keyboard.Keys[KeyCode.Shift] ||
					Keyboard.Keys[KeyCode.ShiftLeft] ||
					Keyboard.Keys[KeyCode.ShiftRight];
			}
		}
		private void TypeKey(string key)
		{
			if (SelectionLength > 0)
			{
				ReplaceSelectedText(key);
				return;
			}

			if (InsertionPoint == Text.Length)
				Text += key;
			else if (InsertionPoint == 0)
				Text = key + Text;
			else
				Text = Text.Substring(0, InsertionPoint) + key + Text.Substring(InsertionPoint);

			MoveInsertionPoint(InsertionPoint + 1, ShiftKey);
		}
		private void TypeDelete()
		{
			if (SelectionLength > 0)
			{
				DeleteSelectedText();
				return;
			}

			if (InsertionPoint == Text.Length)
				return;

			else if (InsertionPoint == 0)
				Text = Text.Substring(1);
			else
				Text = Text.Substring(0, InsertionPoint) + Text.Substring(InsertionPoint + 1);
		}
		private void TypeEnd()
		{
			MoveInsertionPoint(Text.Length, ShiftKey);
		}
		private void TypeHome()
		{
			MoveInsertionPoint(0, ShiftKey);
		}
		private void TypeRight()
		{
			MoveInsertionPoint(InsertionPoint + 1, ShiftKey);

			if (Keyboard.Keys[KeyCode.Shift])
				ExpandSelection();
		}
		private void TypeLeft()
		{
			MoveInsertionPoint(InsertionPoint - 1, ShiftKey);
		}
		private void TypeDown()
		{
		}
		private void TypeUp()
		{
		}
		private void TypeBackspace()
		{
			if (InsertionPoint == 0)
				return;

			if (SelectionLength > 0)
			{
				DeleteSelectedText();
				return;
			}

			if (InsertionPoint == Text.Length)
			{
				MoveInsertionPoint(InsertionPoint - 1, false);
				Text = Text.Substring(0, Text.Length - 1);
			}
			else
			{
				MoveInsertionPoint(InsertionPoint - 1, false);
				Text = Text.Substring(0, InsertionPoint) + Text.Substring(InsertionPoint + 1);
			}
		}

		private string TextBeforeSelection
		{
			get { return Text.Substring(0, SelectionStart); }
		}
		private string TextAfterSelection
		{
			get { return Text.Substring(SelectionEnd); }
		}

		protected internal override bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
		{
			switch (keyCode)
			{
				case KeyCode.Left:
				case KeyCode.Right:
					return true;
				case KeyCode.Up:
				case KeyCode.Down:
				case KeyCode.Enter:
					if (MultiLine)
						return true;
					else
						return false;

				case KeyCode.Escape:
					if (SelectionLength == 0)
						return false;
					else
						return true;

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

				if (mInsertionPoint > Text.Length)
					mInsertionPoint = Text.Length;

				SelectionLength = 0;

				ResetIPBlinking();

				SetDirty();
			}
		}

		private void ResetIPBlinking()
		{
			IPTime = Timing.TotalMilliseconds;
		}

		internal double IPTime { get; set; }

		public bool MultiLine { get; set; }

		#region --- Selection properties and methods ---

		public int SelectionStart
		{
			get { return mSelectionStart; }
			set
			{
				if (value < 0)
					throw new AgateGuiException(
						"Selection start cannot be negative.");
				if (value > Text.Length)
					throw new AgateGuiException(
						"Selection start cannot be larger than text length.");

				mSelectionStart = value;
			}
		}
		public int SelectionLength
		{
			get { return mSelectionLength; }
			set
			{
				if (SelectionStart + value > Text.Length)
					throw new AgateGuiException(
						"The selection cannot extend past the length of the text.");
				if (value < 0)
					throw new AgateGuiException(
						"The selection length cannot be less than zero.");

				mSelectionLength = value;
			}
		}
		int SelectionEnd
		{
			get { return SelectionStart + SelectionLength; }
		}
		public string SelectedText
		{
			get { return Text.Substring(SelectionStart, SelectionLength); }
		}
		public void ReplaceSelectedText(string newText)
		{
			Text = TextBeforeSelection + newText + TextAfterSelection;

			SelectionLength = newText.Length;
			MoveInsertionPoint(SelectionStart + SelectionLength, false);
		}
		public void DeleteSelectedText()
		{
			Text = TextBeforeSelection + TextAfterSelection;

			MoveInsertionPoint(SelectionStart, false);
			SelectionLength = 0;
		}

		/// <summary>
		/// Expands the selection to include the current location of the insertion point.
		/// </summary>
		private void ExpandSelection()
		{
			if (InsertionPoint < SelectionStart)
			{
				SelectionLength += SelectionStart - InsertionPoint;
				SelectionStart = InsertionPoint;
			}
			else if (InsertionPoint > SelectionStart)
			{
				SelectionLength = InsertionPoint - SelectionStart;
			}
		}

		#endregion
	}
}
