using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsPhone.Factories;
using SharpDX.SimpleInitializer;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AgateLib.Platform.WindowsPhone
{
	static class WindowsPhoneInitializer
	{
		internal static void Initialize(SharpDXContext context, DrawingSurfaceBackgroundGrid renderTarget, AssetLocations assets)
		{
			Core.Initialize(new WindowsPhoneFactory(context, renderTarget), assets);
		}
	}
}
