using AgateLib.UserInterface.Css.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
	class ProgressBarDefaultStyle : BlankDefaultStyle
	{
		public override void SetDefaultStyle(CssStyle style)
		{
			style.Data.PositionData.MinWidth = CssDistance.FromString("40px");
			style.Data.PositionData.MinHeight = CssDistance.FromString("4px");
			style.Data.Border.SetValueFromText("1px solid black");
		}
	}
}
