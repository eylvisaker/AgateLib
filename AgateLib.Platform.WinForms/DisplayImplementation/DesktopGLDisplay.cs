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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Drivers;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.VertexTypes;
using OpenTK.Graphics.OpenGL;
using PixelFormat = AgateLib.DisplayLib.PixelFormat;
using AgateLib.OpenGL;
using AgateLib.Platform.WinForms.Controls;
using AgateLib.Platform.WinForms.Factories;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// OpenGL 3.1 compatible.  
	/// </summary>
	public sealed class DesktopGLDisplay : DisplayImpl, IGL_Display, IDrawBufferState
	{
		GL_FrameBuffer mRenderTarget;
		private GLSettings settings;
		Stack<Rectangle> mClipRects = new Stack<Rectangle>();
		Rectangle mCurrentClip = Rectangle.Empty;
		private bool mVSync = true;
		private bool mNonPowerOf2Textures;
		private bool mSupportsShaders;
		private decimal mGLVersion;
		List<int> mTexturesToDelete = new List<int>();

		IPrimitiveRenderer mPrimitives;
		private IScreenConfiguration screens;

		bool mAlphaBlend;
		private IDisplayFactory factory;

		public DesktopGLDisplay(IDisplayFactory factory)
		{
			this.factory = factory;
		}

		public IDisplayFactory Factory => factory;

		public override IScreenConfiguration Screens => screens;

		public Surface WhiteSurface { get; private set; }

		public bool SupportsNonPowerOf2Textures
		{
			get { return mNonPowerOf2Textures; }
			private set { mNonPowerOf2Textures = value; }
		}

		public bool GL3 { get; private set; }
		public bool SupportsFramebufferExt { get; internal set; }
		public bool SupportsFramebufferArb { get; private set; }

		public override IPrimitiveRenderer Primitives => mPrimitives;


		protected override void OnRenderTargetChange(FrameBuffer oldRenderTarget)
		{
			mRenderTarget = (GL_FrameBuffer)RenderTarget.Impl;
			mRenderTarget.MakeCurrent();

			OnRenderTargetResize();
		}

		protected override void OnRenderTargetResize()
		{

		}

		public override PixelFormat DefaultSurfaceFormat => PixelFormat.RGBA8888;

		#region --- Object Factory ---

		protected internal override AgateLib.DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(
			AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader builtInShaderType)
		{
			return ShaderFactory.CreateBuiltInShader(builtInShaderType);
		}

		protected internal override VertexBufferImpl CreateVertexBuffer(VertexLayout layout, int vertexCount)
		{
			if (GL3)
				return new AgateLib.OpenGL.GL3.GLVertexBuffer(layout, vertexCount);
			else
				return new AgateLib.OpenGL.Legacy.LegacyVertexBuffer(layout, vertexCount);
		}

		protected internal override IndexBufferImpl CreateIndexBuffer(IndexBufferType type, int size)
		{
			return new GL_IndexBuffer(type, size);
		}

		public GLDrawBuffer CreateDrawBuffer()
		{
			if (GL3)
				return new AgateLib.OpenGL.GL3.DrawBuffer();
			else
				return new AgateLib.OpenGL.Legacy.LegacyDrawBuffer();
		}


		#endregion

		protected override void OnBeginFrame()
		{
			mRenderTarget.BeginRender();
		}

		protected override void OnEndFrame()
		{
			DrawBuffer.Flush();

			mRenderTarget.EndRender();

			FlushDeleteQueue();
		}

		public GLDrawBuffer DrawBuffer => ((GL_FrameBuffer)RenderTarget.Impl).DrawBuffer;

		public override void SetClipRect(Rectangle newClipRect)
		{
			DrawBuffer.Flush();

			GL.Viewport(newClipRect.X, mRenderTarget.Height - newClipRect.Bottom,
				newClipRect.Width, newClipRect.Height);
 
			var shader = Display.Shader as IShader2D;
			if (shader != null)
			{
				shader.CoordinateSystem = newClipRect;
			}

			mCurrentClip = newClipRect;
		}

		public override void FlushDrawBuffer()
		{
			DrawBuffer.Flush();
		}

		public override void Clear(Color color)
		{
			DrawBuffer.Flush();

			GL.ClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);

			ClearBufferMask mask = ClearBufferMask.ColorBufferBit;

			mask |= RenderTarget.HasDepthBuffer ? ClearBufferMask.DepthBufferBit : 0;
			mask |= RenderTarget.HasStencilBuffer ? ClearBufferMask.StencilBufferBit : 0;

			GL.Clear(mask);
		}

		public override void Clear(Color color, Rectangle destRect)
		{
			// hack because FBO's need to be flipped. So we shift the rectangle.
			if (mRenderTarget is AgateLib.OpenGL.GL3.FrameBuffer)
			{
				destRect.Y = mRenderTarget.Height - destRect.Y - destRect.Height;
			}

			// OpenGL apparently doesn't have direct capability to clear within a 
			// destination region, but we can use the glScissor function to restrict
			// the clear to a specified area.
			GL.Scissor(destRect.Left, destRect.Top, destRect.Width, destRect.Height);
			GL.Enable(EnableCap.ScissorTest);

			Clear(color);
			GL.Disable(EnableCap.ScissorTest);
		}

		#region --- Initialization ---

		public override void Initialize()
		{
			settings = AgateApp.Settings.GetOrCreate("AgateLib.OpenGL", () => new GLSettings());

			CreateEventThread();

			// This needs to be created after the hidden window is created
			// because until we create a window, Windows does not assume that
			// the application is DPI aware, so it will report the wrong
			// display dimensions.
			screens = new WinFormsScreenConfiguration();

			Log.WriteLine("OpenTK / OpenGL driver instantiated for display.");
		}

		public void InitializeCurrentContext()
		{
			GL.ClearColor(0, 0, 0, 1.0f); // Black Background
			GL.ClearDepth(1); // Depth Buffer Setup
			GL.Enable(EnableCap.DepthTest); // Enables Depth Testing
			GL.DepthFunc(DepthFunction.Lequal); // The Type Of Depth Testing To Do
		}

		private void CreateWhiteSurface()
		{
			if (WhiteSurface == null)
			{
				int size = 2;

				PixelBuffer buffer = new PixelBuffer(PixelFormat.ARGB8888, new Size(size, size));

				for (int i = 0; i < size; i++)
				{
					for (int j = 0; j < size; j++)
					{
						buffer.SetPixel(i, j, Color.White);
					}
				}

				WhiteSurface = new Surface(buffer);
			}
		}

		private void CreateEventThread()
		{
			EventThread = new WinFormsEventThread();
			EventThread.CreateContextForCurrentThread();

			string vendor = GL.GetString(StringName.Vendor);
			mSupportsShaders = false;

			mGLVersion = DetectOpenGLVersion();
			if (mGLVersion >= 3m)
			{
				if (settings.EnableGL3)
				{
					GL3 = true;
				}
				else
				{
					GL3 = false;
					mGLVersion = 2.1m;
				}
			}

			LoadExtensions();

			SupportsFramebufferArb = SupportsExtension("GL_ARB_FRAMEBUFFER_OBJECT");
			SupportsFramebufferExt = SupportsExtension("GL_EXT_FRAMEBUFFER_OBJECT");
			mNonPowerOf2Textures = SupportsExtension("GL_ARB_NON_POWER_OF_TWO");

			if (mGLVersion >= 3m)
			{
				mNonPowerOf2Textures = true;
				mSupportsShaders = true;
			}
			if (mGLVersion >= 2m)
			{
				mNonPowerOf2Textures = true;
				mSupportsShaders = true;
			}
			if (mGLVersion < 1.2m)
			{
				System.Windows.Forms.MessageBox.Show(
					"Error: OpenGL 1.2 or higher is required, but your system only supports OpenGL " + mGLVersion.ToString(),
					"OpenGL 1.2 not available", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);

				throw new AgateLib.AgateException("OpenGL 1.2 or higher is required, but this system only supports OpenGL " +
												  mGLVersion.ToString() + ".");
			}

			if (GL3)
				mPrimitives = new AgateLib.OpenGL.GL3.GLPrimitiveRenderer(this);
			else
				mPrimitives = new AgateLib.OpenGL.Legacy.LegacyPrimitiveRenderer(this);

			if (SupportsExtension("GL_ARB_FRAGMENT_PROGRAM"))
			{
				mSupportsShaders = true;
			}

			ShaderFactory.Initialize(GL3);

			Trace.WriteLine(string.Format("OpenGL version {0} from vendor {1} detected.", mGLVersion, vendor));
			Trace.WriteLine("NPOT: " + mNonPowerOf2Textures.ToString());
			Trace.WriteLine("Shaders: " + mSupportsShaders.ToString());

			CreateWhiteSurface();
		}

		string[] extensions;

		private void LoadExtensions()
		{
			if (GL3)
			{
				// Forward compatible context (GL 3.0+)
				int num_extensions;
				GL.GetInteger(GetPName.NumExtensions, out num_extensions);

				if (GL.GetError() != ErrorCode.NoError)
					throw new OpenTK.Graphics.GraphicsErrorException("Not 3.0 context.");

				extensions = new string[num_extensions];

				for (int i = 0; i < num_extensions; i++)
					extensions[i] = GL.GetString(StringNameIndexed.Extensions, i).ToLowerInvariant();
			}
			else
			{
				string ext = GL.GetString(StringName.Extensions);

				extensions = ext.Split(' ');

				for (int i = 0; i < extensions.Length; i++)
				{
					//Debug.Print(extensions[i]);
					extensions[i] = extensions[i].ToLowerInvariant();
				}
			}
		}

		private bool SupportsExtension(string name)
		{
			return extensions.Contains(name.ToLowerInvariant());
		}

		private static decimal DetectOpenGLVersion()
		{
			string versionString = GL.GetString(StringName.Version).Trim();

			// Not sure whether OpenGL drivers will universally report version in the machine's
			// culture settings or not.  So we switch the current decimal separator with a period.
			versionString = versionString.Replace(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator, ".");
			int pointLoc = versionString.IndexOf(".");

			// Remove any additional version information.  Some drivers report version string as
			// something like 2.1.8577, which will be problematic for the decimal.Parse call below.
			// We are only interested in the first two numbers, so discard everything else.
			if (versionString.IndexOf(".", pointLoc + 1) > -1)
			{
				versionString = versionString.Substring(0, versionString.IndexOf(".", pointLoc + 1));
			}

			// Some drivers report a version like "2.0 Chromium 1", so dump everything after the space.
			if (versionString.Contains(" "))
			{
				versionString = versionString.Substring(0, versionString.IndexOf(" "));
			}

			decimal result;

			if (decimal.TryParse(versionString, System.Globalization.NumberStyles.Number,
					System.Globalization.CultureInfo.InvariantCulture, out result) == false)
			{
				Trace.WriteLine("AgateLib.OpenGL was unable to parse the OpenGL version string.");
				Trace.WriteLine("    The reported string was: " + versionString);
				Trace.WriteLine("    Please report this issue to http://www.agatelib.org along");
				Trace.WriteLine("    with details about your operating system and graphics drivers.");
				Trace.WriteLine("    Falling back to OpenGL 1.1 supported functionality.");

				result = 1.1m;
			}

			return result;
		}

		#endregion

		#region --- Shaders ---

		protected override void Dispose(bool disposing)
		{
			EventThread.Dispose();

			base.Dispose(disposing);
		}


		private void SetArray(float[] array, Vector3f vec)
		{
			array[0] = vec.X;
			array[1] = vec.Y;
			array[2] = vec.Z;
		}

		private void SetArray(float[] array, Color color)
		{
			array[0] = color.R / 255.0f;
			array[1] = color.G / 255.0f;
			array[2] = color.B / 255.0f;
			array[3] = color.A / 255.0f;
		}

		#endregion


		protected internal override void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
		{
			FormUtil.SavePixelBuffer(pixelBuffer, filename, format);
		}

		protected internal override void HideCursor()
		{
			System.Windows.Forms.Cursor.Hide();

			if (Display.CurrentWindow != null)
			{
				DisplayWindowImpl impl = Display.CurrentWindow.Impl;
				((GL_DisplayWindow)impl).HideCursor();
			}
		}

		protected internal override void ShowCursor()
		{
			System.Windows.Forms.Cursor.Show();

			if (Display.CurrentWindow != null)
			{
				DisplayWindowImpl impl = Display.CurrentWindow.Impl;

				((GL_DisplayWindow)impl).ShowCursor();
			}
		}

		#region --- Display Capabilities ---

		public override bool CapsBool(DisplayBoolCaps caps)
		{
			switch (caps)
			{
				case DisplayBoolCaps.Scaling:
					return true;
				case DisplayBoolCaps.Rotation:
					return true;
				case DisplayBoolCaps.Color:
					return true;
				case DisplayBoolCaps.Gradient:
					return true;
				case DisplayBoolCaps.SurfaceAlpha:
					return true;
				case DisplayBoolCaps.PixelAlpha:
					return true;
				case DisplayBoolCaps.IsHardwareAccelerated:
					return true;
				case DisplayBoolCaps.FullScreen:
					return true;
				case DisplayBoolCaps.FullScreenModeSwitching:
					return true;
				case DisplayBoolCaps.CustomShaders:
					return false;
				case DisplayBoolCaps.CanCreateBitmapFont:
					return true;
			}

			return false;
		}

		public override Size CapsSize(DisplaySizeCaps caps)
		{
			switch (caps)
			{
				case DisplaySizeCaps.NativeScreenResolution:
					return
						System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.ToGeometry();

				case DisplaySizeCaps.MaxSurfaceSize:
					int val;
					GL.GetInteger(GetPName.MaxTextureSize, out val);

					return new Size(val, val);
			}

			return new Size(0, 0);
		}

		public override double CapsDouble(DisplayDoubleCaps caps)
		{
			switch (caps)
			{
				case DisplayDoubleCaps.AspectRatio:
					return AspectRatio(CapsSize(DisplaySizeCaps.NativeScreenResolution));
			}

			return 0;
		}

		private double AspectRatio(Size size)
		{
			return size.Width / (double)size.Height;
		}

		public override IEnumerable<AgateLib.DisplayLib.Shaders.Implementation.ShaderLanguage> SupportedShaderLanguages
		{
			get { yield return AgateLib.DisplayLib.Shaders.Implementation.ShaderLanguage.Glsl; }
		}

		internal WinFormsEventThread EventThread { get; set; }

		#endregion

		#region --- Render States ---

		protected internal override bool GetRenderState(RenderStateBool renderStateBool)
		{
			switch (renderStateBool)
			{
				case RenderStateBool.WaitForVerticalBlank:
					return mVSync;
				case RenderStateBool.AlphaBlend:
					return mAlphaBlend;

				case RenderStateBool.StencilBufferTest:
					return false;
				case RenderStateBool.ZBufferTest:
					return false;
				case RenderStateBool.ZBufferWrite:
					return false;

				default:
					throw new NotSupportedException($"The specified render state, {renderStateBool}, is not supported by this driver.");
			}
		}

		protected internal override void SetRenderState(RenderStateBool renderStateBool, bool value)
		{
			switch (renderStateBool)
			{
				case RenderStateBool.WaitForVerticalBlank:
					mVSync = value;
					break;

				case RenderStateBool.AlphaBlend:
					mAlphaBlend = value;
					if (value)
						GL.Enable(EnableCap.Blend);
					else
						GL.Disable(EnableCap.Blend);
					break;

				default:
					throw new NotSupportedException(string.Format(
						$"The specified render state, {value}, is not supported by this driver."));
			}
		}

		#endregion

		#region --- Deletion queuing ---

		private void FlushDeleteQueue()
		{
			lock (mTexturesToDelete)
			{
				if (mTexturesToDelete.Count == 0)
					return;

				int[] tex = mTexturesToDelete.ToArray();
				mTexturesToDelete.Clear();

				GL.DeleteTextures(mTexturesToDelete.Count, tex);
			}
		}

		internal void QueueDeleteTexture(int p)
		{
			lock (mTexturesToDelete)
			{
				mTexturesToDelete.Add(p);
			}
		}

		#endregion

	}
}