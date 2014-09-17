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
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Selectors;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform;
using AgateLib.UserInterface.Css.Rendering.Animators;

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
			Document.Updated += Document_Updated;
			MediumInfo = new CssMediumInfo();

			switch (Core.Platform.DeviceType)
			{
				case DeviceType.Computer:
					MediumInfo.MediaType = MediaType.Screen;
					break;

				case DeviceType.Handheld:
					MediumInfo.MediaType = MediaType.Handheld;
					break;

				case DeviceType.Tablet:
					MediumInfo.MediaType = MediaType.Tablet;
					break;
			}
		}

		public CssAdapter(CssDocument doc, Font defaultFont)
			: this(doc)
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
				lock (Document)
				{
					RebuildStyle(control);
				}
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
			Font font = null;

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
					font = style.Widget.Font;

					if (font != null)
						scale = font.FontHeight;
					else
						scale = 0;

					break;

				case DistanceUnit.FontAverageWidth:
					font = style.Widget.Font;

					if (font != null)
						scale = font.FontHeight / 2;
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
			style.Cache.PseudoClass = style.MatchParameters.PseudoClass;

			style.Cache.CssClasses.Clear();
			style.Cache.CssClasses.AddRange(style.MatchParameters.Classes);

			control.StyleDirty = false;
		}
		private bool NeedStyleRefresh(Widget control)
		{
			CssStyle style = mObjectStyles[control];

			style.MatchParameters.UpdateWidgetProperties();

			if (control.StyleDirty) return true;

			IEnumerable<string> classes = style.MatchParameters.Classes;

			if (style.Cache.Id != style.MatchParameters.Id) return true;
			if (style.Cache.PseudoClass != style.MatchParameters.PseudoClass) return true;
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
			style.MatchParameters.UpdateWidgetProperties();

			foreach (var medium in ApplicableMedia)
			{
				foreach (var block in medium.RuleBlocks)
				{
					if (BlockAppliesTo(block, style.MatchParameters))
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
		}

		private bool BlockAppliesTo(CssRuleBlock block, WidgetMatchParameters wmp)
		{
			foreach (var selector in block.Selector.IndividualSelectors)
			{
				if (SelectorAppliesTo(selector, wmp))
					return true;
			}

			return false;
		}


		private bool SelectorAppliesTo(ICssSelector selector, WidgetMatchParameters wmp)
		{
			return selector.Matches(this, wmp);
		}

		public CssMediumInfo MediumInfo { get; set; }

		public IEnumerable<CssMediaSelector> ApplicableMedia
		{
			get
			{
				yield return Document.DefaultMedium;

				foreach (var medium in Document.Media)
				{
					if (MediaSelectorApplies(medium, MediumInfo))
						yield return medium;
				}
			}
		}

		private bool MediaSelectorApplies(CssMediaSelector selectorGroup, CssMediumInfo medium)
		{
			foreach (var sel in selectorGroup.IndividualSelectors)
			{
				if (sel.Matches(medium.MediaType.ToString()))
					return true;
			}

			return false;
		}


		void Document_Updated(object sender, EventArgs e)
		{
			foreach (var widget in mObjectStyles.Keys)
			{
				widget.LayoutDirty = true;
				widget.StyleDirty = true;

				mObjectStyles[widget].AppliedBlocks.Clear();
			}
		}


		public void SetFont(Widget control)
		{
			var style = GetStyle(control);

			control.Font = style.Font;
			control.Font.Style = style.Data.Font.Weight;
		}
	}
}
