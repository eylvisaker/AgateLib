using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssBorderImage : ICssPropertyFromText
	{
		public CssBorderImage()
		{
			Initialize();
		}

		private void Initialize()
		{
			Source = string.Empty;
			Slice = new CssBorderImageComponent();
			Width = new CssBorderImageComponent();
			Outset = new CssBorderImageComponent();
			Repeat = CssBorderImageRepeat.Initial;
		}

		public string Source { get; set; }
		public CssBorderImageComponent Slice { get; set; }
		public CssBorderImageComponent Width { get; set; }
		public CssBorderImageComponent Outset { get; set; }
		public CssBorderImageRepeat Repeat { get; set; }

		public void SetValueFromText(string value)
		{
			if (value == "none")
			{
				Initialize();
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}

	public class CssBorderImageComponent : CssBoxComponent
	{

	}

	public enum CssBorderImageRepeat
	{
		Initial,
		Inherit,
		Stretch,
		Repeat,
		Round,
	}
}
