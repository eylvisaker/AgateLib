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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.OpenGL;
using AgateLib.Platform.WinForms.DisplayImplementation;
using AgateLib.Platform.WinForms.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DefaultAssets;

namespace AgateLib.Platform.WinForms.Factories
{
	class DisplayFactory : IDisplayFactory
	{
		BuiltinResources builtIn;

		public DisplayFactory()
		{
			FullDisplayImpl = new DesktopGLDisplay();
			builtIn = new Resources.BuiltinResources();
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
			if (provider.IsLogicalFilesystem)
			{
				using (var file = provider.OpenReadAsync(filename).GetAwaiter().GetResult())
				{
					return new GL_Surface(file);
				}
			}
			else
			{
				using (var file = File.OpenRead(provider.ResolveFile(filename)))
				{
					return new GL_Surface(provider.ResolveFile(filename));
				}
			}
		}

		public SurfaceImpl CreateSurface(Stream fileStream)
		{
			return new GL_Surface(fileStream);
		}

		public SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new GL_Surface(surfaceSize);
		}


		public SurfaceImpl CreateSurface(PixelBuffer pixels)
		{
			var result = CreateSurface(pixels.Size);

			result.WritePixels(pixels);

			return result;
		}

		public FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyles style)
		{
			BitmapFontOptions options = new BitmapFontOptions(fontFamily, sizeInPoints, style);

			return AgateLib.Platform.WinForms.Fonts.BitmapFontUtil.ConstructFromOSFont(options);
		}

		public FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			return AgateLib.Platform.WinForms.Fonts.BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
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


		public Task InitializeDefaultResourcesAsync(DefaultResources res)
		{
			res.Dispose();

			var sans = builtIn.GetFont("AgateSans");
			var serif = builtIn.GetFont("AgateSerif");
			var mono = builtIn.GetFont("AgateMono");

			res.AgateSans = sans;
			res.AgateSerif = serif;
			res.AgateMono = mono;

			return Task.FromResult(0);
		}
	}
}
