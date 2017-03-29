//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.X11;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	public class OpenTkAdapter
	{
		public GraphicsMode CreateGraphicsMode()
		{
			var newMode = new GraphicsMode(
				GraphicsMode.Default.ColorFormat, GraphicsMode.Default.Depth,
				0, 0, new ColorFormat(0), 2, false);

			return newMode;
		}

		public IWindowInfo CreateWindowInfo(GraphicsMode mode, Control renderTarget)
		{
			if (renderTarget.InvokeRequired)
			{
				IWindowInfo result = null;

				renderTarget.Invoke(new Action(() => { result = CreateWindowInfo(mode, renderTarget); }));

				return result;
			}

			switch (AgateApp.Platform.PlatformType)
			{
				case PlatformType.Windows:
					return Utilities.CreateWindowsWindowInfo(renderTarget.Handle);
				case PlatformType.MacOS:
					return Utilities.CreateMacOSCarbonWindowInfo(renderTarget.Handle, false, true);
				case PlatformType.Linux:
					return CreateX11WindowInfo(renderTarget.Handle, mode);
				default:
					throw new InvalidOperationException("Platform not implemented.");
			}
		}

		protected IWindowInfo CreateX11WindowInfo(IntPtr handle, GraphicsMode mode)
		{
			var xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms");
			if (xplatui == null)
				throw new PlatformNotSupportedException(
					"System.Windows.Forms.XplatUIX11 missing. Unsupported platform or Mono runtime version, aborting.");

			// get the required handles from the X11 API.
			var display = (IntPtr)GetStaticFieldValue(xplatui, "DisplayHandle");
			var rootWindow = (IntPtr)GetStaticFieldValue(xplatui, "RootWindow");
			var screen = (int)GetStaticFieldValue(xplatui, "ScreenNo");

			// get the X11 Visual info for the display.
			var info = new XVisualInfo();
			info.VisualID = mode.Index.Value;
			int dummy;
			info = (XVisualInfo)Marshal.PtrToStructure(
				XGetVisualInfo(display, XVisualInfoMask.ID, ref info, out dummy), typeof(XVisualInfo));

			// set the X11 colormap.
			SetStaticFieldValue(xplatui, "CustomVisual", info.Visual);
			SetStaticFieldValue(xplatui, "CustomColormap",
				XCreateColormap(display, rootWindow, info.Visual, 0));

			var infoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(info));
			Marshal.StructureToPtr(info, infoPtr, false);

			var window = Utilities.CreateX11WindowInfo(
				display, screen, handle, rootWindow, infoPtr);

			return window;
		}

		#region --- X11 imports ---

		[StructLayout(LayoutKind.Sequential)]
		private struct XVisualInfo
		{
			public readonly IntPtr Visual;
			public IntPtr VisualID;
			public readonly int Screen;
			public readonly int Depth;
			public readonly XVisualClass Class;
			public readonly long RedMask;
			public readonly long GreenMask;
			public readonly long blueMask;
			public readonly int ColormapSize;
			public readonly int BitsPerRgb;

			public override string ToString()
			{
				return string.Format("id ({0}), screen ({1}), depth ({2}), class ({3})",
					VisualID, Screen, Depth, Class);
			}
		}

		[DllImport("libX11")]
		public static extern IntPtr XCreateColormap(IntPtr display, IntPtr window, IntPtr visual, int alloc);

		[DllImport("libX11", EntryPoint = "XGetVisualInfo")]
		private static extern IntPtr XGetVisualInfoInternal(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template,
			out int nitems);

		private static IntPtr XGetVisualInfo(IntPtr display, XVisualInfoMask vinfo_mask, ref XVisualInfo template,
			out int nitems)
		{
			return XGetVisualInfoInternal(display, (IntPtr)(int)vinfo_mask, ref template, out nitems);
		}

		[Flags]
		internal enum XVisualInfoMask
		{
			No = 0x0,
			ID = 0x1,
			Screen = 0x2,
			Depth = 0x4,
			Class = 0x8,
			Red = 0x10,
			Green = 0x20,
			Blue = 0x40,
			ColormapSize = 0x80,
			BitsPerRGB = 0x100,
			All = 0x1FF
		}

		#endregion

		#region --- Utility functions for reading/writing non-public static fields through reflection ---

		protected static object GetStaticFieldValue(Type type, string fieldName)
		{
			return type.GetField(fieldName,
				BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		}

		protected static void SetStaticFieldValue(Type type, string fieldName, object value)
		{
			type.GetField(fieldName,
				BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, value);
		}

		#endregion
	}
}
