using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ERY.AgateLib.WinForms
{
    public static class FormUtil
    {
        /// <summary>
        /// Creates a System.Windows.Forms.Form object for rendering to.
        /// </summary>
        /// <param name="frm">Returns the created form.</param>
        /// <param name="renderTarget">Returns the control which is rendered into.</param>
        /// <param name="position">Position of the window.</param>
        /// <param name="title">Title of the window.</param>
        /// <param name="clientWidth">Width of client area, in pixels.</param>
        /// <param name="clientHeight">Height of client area, in pixels.</param>
        /// <param name="startFullscreen">True if we should start with a full-screen window.</param>
        /// <param name="allowResize">True if we should allow the user to resize the window.</param>
        /// <param name="hasFrame">True if a frame and title bar should be present.</param>
        public static void InitializeWindowsForm(
            out Form frm,
            out Control renderTarget,
            WindowPosition position,
            string title, int clientWidth, int clientHeight, bool startFullscreen, bool allowResize, bool hasFrame)
        {
            DisplayWindowForm mainForm = new DisplayWindowForm();

            // set output values
            frm = mainForm;
            renderTarget = mainForm.RenderTarget;

            // set properties
            frm.Text = title;
            frm.ClientSize = new System.Drawing.Size(clientWidth, clientHeight);
            frm.KeyPreview = true;
            frm.Icon = Icons.AgateLib;

            if (hasFrame == false)
                frm.FormBorderStyle = FormBorderStyle.None;
            else if (allowResize == false)
            {
                frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                frm.MaximizeBox = false;
            }

            Point centerPoint = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - frm.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - frm.Height) / 2);

            switch (position)
            {
                case WindowPosition.DefaultAgate:
                case WindowPosition.AboveCenter:
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new System.Drawing.Point(centerPoint.X, centerPoint.Y / 2);

                    break;

                case WindowPosition.CenterScreen:
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    break;

                case WindowPosition.DefaultOS:
                    frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                    break;
            }
        }

        /// <summary>
        /// Gets the official icon for AgateLib.
        /// </summary>
        public static Icon AgateLibIcon
        {
            get { return Icons.AgateLib; }
        }

        /// <summary>
        /// Converts a System.Windows.Forms.Keys value to a KeyCode value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static KeyCode TransformWinFormsKey(System.Windows.Forms.Keys id)
        {
            KeyCode myvalue;

            // sometimes windows reports Shift and sometimes ShiftKey.. what gives?
            switch (id)
            {
                case System.Windows.Forms.Keys.Alt:
                case System.Windows.Forms.Keys.Menu:
                    myvalue = KeyCode.Alt;
                    break;

                case System.Windows.Forms.Keys.Control:
                case System.Windows.Forms.Keys.ControlKey:
                    myvalue = KeyCode.Control;
                    break;

                case System.Windows.Forms.Keys.Shift:
                case System.Windows.Forms.Keys.ShiftKey:
                    myvalue = KeyCode.Shift;
                    break;

                default:
                    myvalue = (KeyCode)id;
                    break;
            }
            return myvalue;
        }
    }
}
