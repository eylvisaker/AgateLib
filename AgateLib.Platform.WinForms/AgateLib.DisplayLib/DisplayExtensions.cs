using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgateLib.DisplayLib
{
	public static class DisplayExtensions
	{

		/// <summary>
		/// Indicates a DisplayWindow object should be created that renders
		/// to a Windows.Forms.Control object. If no back buffer size is 
		/// specified, the backbuffer will be the size of the control.
		/// </summary>
		/// <param name="builder">The DisplayWindowBuilder fluent object.</param>
		/// <param name="renderTarget"></param>
		/// <returns></returns>
		public static DisplayWindowBuilder RenderToControl(
			this DisplayWindowBuilder builder, Control renderTarget)
		{
			builder.CreateWindowParams.IsFullScreen = false;
			builder.CreateWindowParams.RenderToControl = true;
			builder.CreateWindowParams.RenderTarget = renderTarget;

			return builder;
		}
	}
}
