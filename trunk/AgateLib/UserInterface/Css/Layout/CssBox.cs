using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout
{
	public struct CssBox
	{
		private int top, left, right, bottom;

		public int Bottom
		{
			get { return bottom; }
			set { bottom = value; }
		}

		public int Right
		{
			get { return right; }
			set { right = value; }
		}

		public int Left
		{
			get { return left; }
			set { left = value; }
		}

		public int Top
		{
			get { return top; }
			set { top = value; }
		}
	}
}
