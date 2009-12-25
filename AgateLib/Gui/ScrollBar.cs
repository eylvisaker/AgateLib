﻿//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
	public abstract class ScrollBar  : Widget 
	{
		int mMinValue = 0;
		int mMaxValue = 100;
		int mValue;
		int mLargeChange = 10;
		int mSmallChange = 1;

		internal bool MouseDownInDecrease { get; set; }
		internal bool MouseDownInIncrease { get; set; }
		internal bool MouseDownInThumb { get; set; }
		internal bool MouseDownInPageDecrease { get; set; }
		internal bool MouseDownInPageIncrease { get; set; }

		public event EventHandler ValueChanged;
		private void OnValueChanged()
		{
			if (ValueChanged != null)
				ValueChanged(this, EventArgs.Empty);
		}

		public int Value
		{
			get { return mValue; }
			set
			{
				if (value < mMinValue)
					throw new ArgumentOutOfRangeException();
				if (value > mMaxValue)
					throw new ArgumentOutOfRangeException();

				mValue = value;
				OnValueChanged();
			}
		}

		public int LargeChange
		{
			get { return mLargeChange; }
			set
			{
				if (mSmallChange < 1)
					throw new ArgumentOutOfRangeException();
			
				mLargeChange = value;
			}
		}
		public int SmallChange
		{
			get { return mSmallChange; }
			set
			{
				if (mSmallChange < 1)
					throw new ArgumentOutOfRangeException();

				mSmallChange = value;
			}
		}

		public int MinValue
		{
			get { return mMinValue; }
			set
			{
				mMinValue = value;

				if (MaxValue < MinValue)
					MaxValue = MinValue;

				if (Value < MinValue)
					Value = MinValue;

			}
		}

		public int MaxValue
		{
			get { return mMaxValue; }
			set
			{
				mMaxValue = value;

				if (MinValue > MaxValue)
					MinValue = MaxValue;
					
				if (Value > MaxValue)
					Value = MaxValue;
				
			}
		}
	}

	public class VerticalScrollBar : ScrollBar
	{	}

	public class HorizontalScrollBar : ScrollBar
	{ }
}