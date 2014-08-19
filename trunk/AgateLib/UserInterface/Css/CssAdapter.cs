//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.DisplayLib;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Css.Layout.Defaults;
using AgateLib.UserInterface.Css.Properties;
using AgateLib.UserInterface.Css.Selectors;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssAdapter
	{
		Dictionary<Widget, CssStyle> mObjectStyles = new Dictionary<Widget, CssStyle>();
		DefaultStyleCollection mDefaultStyles = new DefaultStyleCollection();
		private CssDocument mDocument;
		
		internal CssAdapter(CssDocument doc)
		{
			Document = doc;
		}

		public CssAdapter(CssDocument doc, Font defaultFont) : this(doc)
		{
			this.DefaultFont = defaultFont;
		}

		public Font DefaultFont { get; set; }

		public CssDocument Document
		{
			get { return mDocument; }
			set
			{
				mDocument = value;

				CurrentMedium = mDocument.DefaultMedium;

				foreach (var style in mObjectStyles.Values)
				{
					style.AppliedBlocks.Clear();
					style.Widget.LayoutDirty = true;
				}
			}
		}
		public DefaultStyleCollection DefaultStyles
		{
			get { return mDefaultStyles; }
		}

		public CssStyle GetStyle(Widget control)
		{
			if (mObjectStyles.ContainsKey(control) == false)
			{
				mObjectStyles.Add(control, new CssStyle(control));
			}

			bool refresh = NeedStyleRefresh(control);

			if (refresh)
			{
				RebuildStyle(control);
			}

			var style = mObjectStyles[control];

			UpdateBoxModel(style);
			return style;
		}

		public int CssDistanceToPixels(Widget widget, CssDistance distance, bool width)
		{
			return CssDistanceToPixels(GetStyle(widget), distance, width);
		}
		public int CssDistanceToPixels(CssStyle style, CssDistance distance, bool width)
		{
			int scale = 1;
			double amount = distance.Amount;

			switch (distance.DistanceUnit)
			{
				case DistanceUnit.Pixels: break;
				case DistanceUnit.Percent:
					amount /= 100.0;

					if (style.Widget.Parent == null)
						scale = 0;
					else
					{
						if (width)
							scale = style.Widget.Parent.Width;
						else
							scale = style.Widget.Parent.Height;
					}

					break;

				case DistanceUnit.FontHeight:
					var font = style.Widget.Font;

					if (font != null)
						scale = font.FontHeight;
					else
						scale = 0;

					break;

				default:
					throw new NotImplementedException();
			}

			return (int)(amount * scale);
		}

		private void UpdateBoxModel(CssStyle style)
		{
			CssBoxModel model = style.BoxModel;

			model.Border = GetBox(style, style.Data.Border);
			model.Padding = GetBox(style, style.Data.Padding);
			model.Margin = GetBox(style, style.Data.Margin);
		}
		private CssBox GetBox(CssStyle style, ICssBoxComponent box)
		{
			CssBox retval = new CssBox();

			retval.Left = CssDistanceToPixels(style, box.Left, true);
			retval.Right = CssDistanceToPixels(style, box.Right, true);
			retval.Top = CssDistanceToPixels(style, box.Top, true);
			retval.Bottom = CssDistanceToPixels(style, box.Bottom, true);

			return retval;
		}

		private void RebuildStyle(Widget control)
		{
			var style = mObjectStyles[control];

			AssignApplicableBlocks(style);

			ResetStyleToDefault(style);
			CopyInheritedProperties(style);

			SaveCachedValues(style);

			foreach (var block in style.AppliedBlocks)
			{
				foreach (var prop in block.Properties)
				{
					try
					{
						block.ApplyProperties(style.Data);
						//mDocument.Binding.ApplyProperty(style.Data, prop.Key, prop.Value);
					}
					catch
					{
						ReportError(string.Format(
							"Failed to apply property {0} with value {1}", prop.Key, prop.Value));
					}
				}
			}

			SetStyleFont(style);
		}

		private void SetStyleFont(CssStyle style)
		{
			style.Font = DefaultFont;
		}

		private void SaveCachedValues(CssStyle style)
		{
			var control = style.Widget;

			style.Cache.Id = control.Name;
			style.Cache.PseudoClass = GetPseudoClass(control);

			var classes = GetCssClasses(style);

			style.Cache.CssClasses.Clear();

			if (classes != null)
				style.Cache.CssClasses.AddRange(classes);

			control.LayoutDirty = false;
		}
		private bool NeedStyleRefresh(Widget control)
		{
			CssStyle style = mObjectStyles[control];

			if (control.LayoutDirty) return true;

			string id = control.Name;
			IEnumerable<string> classes = GetCssClasses(style);

			if (style.Cache.Id != id) return true;
			if (style.Cache.PseudoClass != GetPseudoClass(control)) return true;
			if (classes != null)
			{
				if (style.Cache.CssClasses.All(x => classes.Contains(x)) == false) return true;
				if (classes.All(x => style.Cache.CssClasses.Contains(x)) == false) return true;
			}
			else
				if (style.Cache.CssClasses.Count > 0) return true;

			return false;
		}

		private void CopyInheritedProperties(CssStyle style)
		{
			if (style.Widget.Parent == null)
				return;

			mDefaultStyles[style.Widget.GetType()].InheritParentProperties(style, GetStyle(style.Widget.Parent));
			
		}

		private void ReportError(string p)
		{
		}

		private void ResetStyleToDefault(CssStyle style)
		{
			style.Data.Clear();

			mDefaultStyles[style.Widget.GetType()].SetDefaultStyle(style);
		}


		private void AssignApplicableBlocks(CssStyle style)
		{
			var control = style.Widget;
			string id = control.Name;
			IEnumerable<string> classes = GetCssClasses(style);

			foreach (var block in CurrentMedium.RuleBlocks)
			{
				if (BlockAppliesTo(block, control, id, classes))
				{
					if (style.AppliedBlocks.Contains(block) == false)
					{
						style.AppliedBlocks.Add(block);
					}
				}
				else
				{
					if (style.AppliedBlocks.Contains(block))
					{
						style.AppliedBlocks.Remove(block);
					}
				}
			}
		}

		private bool BlockAppliesTo(CssRuleBlock block, Widget control, string id,
			IEnumerable<string> classes)
		{
			CssPseudoClass pseudoClass = GetPseudoClass(control);

			foreach (var selector in block.Selector.IndividualSelectors)
			{
				if (SelectorAppliesTo(selector, control, id, pseudoClass, classes))
					return true;
			}

			return false;
		}

		private CssPseudoClass GetPseudoClass(Widget control)
		{
			if (control.MouseIn)
				return CssPseudoClass.Hover;
			if (control is Container)
			{
				Container container = (Container)control;

				if (container.ChildHasMouseIn)
					return CssPseudoClass.Hover;
			}
			if (control is MenuItem)
			{
				MenuItem mnuit = (MenuItem)control;

				if (mnuit.Selected)
					return CssPseudoClass.Selected;
			}

			return CssPseudoClass.None;
		}

		private bool SelectorAppliesTo(ICssSelector selector, Widget control,
			string id, CssPseudoClass pseudoClass, IEnumerable<string> classes)
		{
			return selector.Matches(control, id, pseudoClass, classes);
		}

		private IEnumerable<string> GetCssClasses(CssStyle style)
		{
			Widget control = style.Widget;

			if (control.Style != style.ObjectClass)
			{
				style.ObjectClass = control.Style;
			}

			return style.SplitClasses;
		}



		CssMedia CurrentMedium { get; set; }
	}
}
