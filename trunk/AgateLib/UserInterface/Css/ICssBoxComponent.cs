using System;
namespace AgateLib.UserInterface.Css
{
	public interface ICssBoxComponent
	{
		CssDistance Bottom { get; set; }
		CssDistance Left { get; set; }
		CssDistance Right { get; set; }
		CssDistance Top { get; set; }
	}
}
