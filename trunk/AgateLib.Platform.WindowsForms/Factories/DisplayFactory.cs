﻿using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.OpenGL;
using AgateLib.Platform.WindowsForms.DisplayImplementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.Factories
{
	class DisplayFactory : IDisplayFactory
	{
		public DisplayFactory()
		{
			FullDisplayImpl = new DesktopGLDisplay();
		}

		public DisplayImpl DisplayImpl { get { return FullDisplayImpl; } }
		public DesktopGLDisplay FullDisplayImpl { get; private set; }

		public DisplayWindowImpl CreateDisplayWindow(DisplayWindow owner, DisplayLib.CreateWindowParams windowParams)
		{
			if (windowParams.IsFullScreen && windowParams.RenderToControl == false)
				return new GL_GameWindow(owner, windowParams);
			else
				return new GL_DisplayControl(owner, windowParams);
		}

		public SurfaceImpl CreateSurface(IReadFileProvider provider, string filename)
		{
			return new GL_Surface(provider.ResolveFile(filename));
		}

		public SurfaceImpl CreateSurface(Stream fileStream)
		{
			return new GL_Surface(fileStream);
		}

		public SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new GL_Surface(surfaceSize);
		}

		public FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyles style)
		{
			BitmapFontOptions options = new BitmapFontOptions(fontFamily, sizeInPoints, style);

			return AgateLib.Platform.WindowsForms.WinForms.BitmapFontUtil.ConstructFromOSFont(options);
		}

		public FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			return AgateLib.Platform.WindowsForms.WinForms.BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
		}


		public FrameBufferImpl CreateFrameBuffer(Size size)
		{
			if (FullDisplayImpl.GL3)
				return new AgateLib.OpenGL.GL3.FrameBuffer((IGL_Surface)new Surface(size).Impl);
			
			if (SupportsFramebufferArb && ReadSettingsBool("DisableFramebufferArb") == false)
				return new AgateLib.OpenGL.GL3.FrameBuffer((IGL_Surface)new Surface(size).Impl);

			if (SupportsFramebufferExt && ReadSettingsBool("DisableFramebufferExt") == false)
			{
				try
				{
					return new AgateLib.OpenGL.Legacy.FrameBufferExt((IGL_Surface)new Surface(size).Impl);
				}
				catch (Exception e)
				{
					Log.WriteLine(string.Format("Caught exception {0} when trying to create GL_FrameBuffer_Ext wrapper.", e.GetType()));
					Log.WriteLine(e.Message);
					Log.WriteLine("");
					Log.WriteLine("Disabling frame buffer extension, and falling back onto glCopyTexSubImage2D.");
					Log.WriteLine("Extensive use of offscreen rendering targets will result in poor performance.");
					Log.WriteLine("");

					SupportsFramebufferExt = false;
				}
			}

			return new AgateLib.OpenGL.Legacy.FrameBufferReadPixels((IGL_Surface)new Surface(size).Impl);
		}

		private bool ReadSettingsBool(string name)
		{
			return FullDisplayImpl.ReadSettingsBool(name);
		}

		public bool SupportsFramebufferArb { get { return FullDisplayImpl.SupportsFramebufferArb; } }
		public bool SupportsFramebufferExt
		{
			get { return FullDisplayImpl.SupportsFramebufferExt; }
			set { FullDisplayImpl.SupportsFramebufferExt = value; }
		}
	}
}
