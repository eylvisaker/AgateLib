using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Venus
{
	public class Venus : IGuiThemeEngine 
	{
		public Venus()
		{

		}

		public void LoadCss(string text)
		{
			CssData d = new CssData();
			d.Parse(text);

			Css = d;
		}
		
		public CssData Css { get; set; }

		#region IGuiThemeEngine Members

		public void DrawWidget(Widget widget)
		{
			throw new NotImplementedException();
		}

		public Size CalcMinSize(Widget widget)
		{
			throw new NotImplementedException();
		}

		public Size CalcMaxSize(Widget widget)
		{
			throw new NotImplementedException();
		}

		public int ThemeMargin(Widget widget)
		{
			throw new NotImplementedException();
		}

		public Rectangle GetClientArea(Container widget)
		{
			throw new NotImplementedException();
		}

		public Size RequestClientAreaSize(Container widget, Size clientSize)
		{
			throw new NotImplementedException();
		}

		public bool HitTest(Widget widget, Point screenLocation)
		{
			throw new NotImplementedException();
		}

		public void Update(GuiRoot guiRoot)
		{
			throw new NotImplementedException();
		}

		public void MouseDownInWidget(Widget widget, Point clientLocation)
		{
			throw new NotImplementedException();
		}

		public void MouseMoveInWidget(Widget widget, Point clientLocation)
		{
			throw new NotImplementedException();
		}

		public void MouseUpInWidget(Widget widget, Point clientLocation)
		{
			throw new NotImplementedException();
		}

		public void WidgetNeedsUpdate(Widget widget)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IGuiThemeEngine Members


		public WidgetRenderer CreateRenderer(Widget widget)
		{
			return new CssRenderer(this, widget);
		}

		#endregion
	}
}
