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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.DefaultAssets;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Mathematics.Geometry;
using AgateLib.OpenGL;
using AgateLib.Platform.WinForms.DisplayImplementation;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Quality;

namespace AgateLib.Platform.WinForms.Factories
{
	class DisplayFactory : IDisplayFactory
	{
		private BuiltinResources builtIn;
		private GLSettings settings;

		public DisplayFactory()
		{
			Core = new DesktopGLDisplay(this);
			builtIn = new Resources.BuiltinResources();
		}

		public DisplayImpl DisplayCore => Core;

		public DesktopGLDisplay Core { get; }

		public DisplayWindowImpl CreateDisplayWindow(
			DisplayWindow owner, CreateWindowParams windowParams)
		{
			const string errorPrefix = "Inconsistent window paramters: ";

			if (windowParams.IsFullScreen)
			{
				Require.True<ArgumentException>(windowParams.RenderToControl == false,
					$"{errorPrefix}{nameof(windowParams.IsFullScreen)} " +
					$"and {nameof(windowParams.RenderToControl)} cannot both be true.");

				Require.True<ArgumentException>(windowParams.RenderTarget == null,
					$"{errorPrefix}{nameof(windowParams.IsFullScreen)} is true " +
					$"but {nameof(windowParams.RenderToControl)} is not null.");

				if (windowParams.CompleteDesktop)
				{
					return new GL_DisplayWindowFullDesktop(Core, owner, windowParams);
				}
				return new GL_DisplayWindowFullScreen(Core, owner,
					windowParams);
			}
			
			Require.True<ArgumentException>(
				windowParams.RenderToControl == (windowParams.RenderTarget != null),
				$"{errorPrefix}{nameof(windowParams.RenderToControl)} should be " +
				$"true if and only if {nameof(windowParams.RenderTarget)} is not null.");

			return new GL_DisplayWindowWindowed(Core, owner, 
					windowParams);
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
					return new GL_Surface(file);
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
			if (settings == null)
			{
				settings = AgateApp.Settings.GetOrCreate("AgateLib.OpenGL", () => new GLSettings());
			}

			if (Core.GL3)
				return new AgateLib.OpenGL.GL3.FrameBuffer((IGL_Surface)new Surface(size).Impl);

			if (SupportsFramebufferArb && settings.DisableFramebufferArb == false)
				return new AgateLib.OpenGL.GL3.FrameBuffer((IGL_Surface)new Surface(size).Impl);

			if (SupportsFramebufferExt && settings.DisableFramebufferExt == false)
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
		
		public bool SupportsFramebufferArb { get { return Core.SupportsFramebufferArb; } }
		public bool SupportsFramebufferExt
		{
			get { return Core.SupportsFramebufferExt; }
			set { Core.SupportsFramebufferExt = value; }
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
