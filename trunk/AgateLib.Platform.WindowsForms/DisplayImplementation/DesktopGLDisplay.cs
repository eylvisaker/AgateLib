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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using AgateLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using OpenTK.Graphics.OpenGL;
using PixelFormat = AgateLib.DisplayLib.PixelFormat;
using AgateLib.OpenGL;
using AgateLib.Platform.WindowsForms.WinForms;

namespace AgateLib.Platform.WindowsForms.DisplayImplementation
{
	/// <summary>
	/// OpenGL 3.1 compatible.  
	/// </summary>
	public sealed class GL_Display : DisplayImpl, AgateLib.OpenGL.IGL_Display
	{ 
		GL_FrameBuffer mRenderTarget;
		Stack<Rectangle> mClipRects = new Stack<Rectangle>();
		Rectangle mCurrentClip = Rectangle.Empty;
		private bool mVSync = true;
		private bool mSupportsFramebufferArb;
		private bool mSupportsFramebufferExt;
		private bool mNonPowerOf2Textures;
		private bool mSupportsShaders;
		private decimal mGLVersion;

		System.Windows.Forms.Form mFakeWindow;
		DisplayWindow mFakeDisplayWindow;

		PrimitiveRenderer mPrimitives;

		bool mGL3;

		public Surface WhiteSurface
		{
			get;
			private set;
		}
		public bool SupportsNonPowerOf2Textures
		{
			get { return mNonPowerOf2Textures; }
			private set { mNonPowerOf2Textures = value; }
		}

		internal event EventHandler ProcessEventsEvent;

		protected override void ProcessEvents()
		{
			if (ProcessEventsEvent != null)
				ProcessEventsEvent(this, EventArgs.Empty);
		}

		protected override void OnRenderTargetChange(FrameBuffer oldRenderTarget)
		{
			mRenderTarget = RenderTarget.Impl as GL_FrameBuffer;
			mRenderTarget.MakeCurrent();

			OnRenderTargetResize();
		}
		protected override void OnRenderTargetResize()
		{

		}

		public override PixelFormat DefaultSurfaceFormat
		{
			get { return PixelFormat.RGBA8888; }
		}

		#region --- Object Factory ---

		protected override AgateLib.DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader builtInShaderType)
		{
			return ShaderFactory.CreateBuiltInShader(builtInShaderType);
		}
		public override DisplayWindowImpl CreateDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
		{
			if (windowParams.IsFullScreen && windowParams.RenderToControl == false)
				return new GL_GameWindow(owner, windowParams);
			else
				return new GL_DisplayControl(owner, windowParams);

			//if (windowParams.RenderToControl)
			//{
			//    return new GL_DisplayControl(windowParams);
			//}
			//else
			//{
			//    return new GL_GameWindow(windowParams);
			//}
		}
		public override SurfaceImpl CreateSurface(string fileName)
		{
			return new GL_Surface(fileName);
		}
		protected override VertexBufferImpl CreateVertexBuffer(VertexLayout layout, int vertexCount)
		{
			if (mGL3)
				return new AgateLib.OpenGL.GL3.GLVertexBuffer(layout, vertexCount);
			else
				return new AgateLib.OpenGL.Legacy.LegacyVertexBuffer(layout, vertexCount);
		}
		protected override IndexBufferImpl CreateIndexBuffer(IndexBufferType type, int size)
		{
			return new GL_IndexBuffer(type, size);
		}

		public override SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new GL_Surface(surfaceSize);
		}
		public override SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			return new GL_Surface(fileStream);
		}
		public override FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyle style)
		{
			BitmapFontOptions options = new BitmapFontOptions(fontFamily, sizeInPoints, style);

			return AgateLib.Platform.WindowsForms.WinForms.BitmapFontUtil.ConstructFromOSFont(options);
		}
		public override FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			return AgateLib.Platform.WindowsForms.WinForms.BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
		}

		protected override FrameBufferImpl CreateFrameBuffer(Size size)
		{
			if (mGL3 || (mSupportsFramebufferArb && ReadSettingsBool("DisableFramebufferArb") == false))
				return new AgateLib.OpenGL.GL3.FrameBuffer((IGL_Surface)new Surface(size).Impl);
			else if (mSupportsFramebufferExt && ReadSettingsBool("DisableFramebufferExt") == false)
			{
				try
				{
					return new AgateLib.OpenGL.Legacy.FrameBufferExt((IGL_Surface)new Surface(size).Impl);
				}
				catch (Exception e)
				{
					Trace.WriteLine(string.Format("Caught exception {0} when trying to create GL_FrameBuffer_Ext wrapper.", e.GetType()));
					Trace.Indent();
					Trace.WriteLine(e.Message);
					Trace.Unindent();
					Trace.WriteLine("");
					Trace.WriteLine("Disabling frame buffer extension, and falling back onto glCopyTexSubImage2D.");
					Trace.WriteLine("Extensive use of offscreen rendering targets will result in poor performance.");
					Trace.WriteLine("");

					mSupportsFramebufferExt = false;
				}
			}

			return new AgateLib.OpenGL.Legacy.FrameBufferReadPixels((IGL_Surface)new Surface(size).Impl);
		} 

		bool ReadSettingsBool(string name)
		{
			string value;

			if (AgateLib.Core.Settings["AgateLib.OpenGL"].TryGetValue(name, out value) == false)
				return false;

			if (value == "false" || value == "0")
				return false;

			return true;
		}

		public GLDrawBuffer CreateDrawBuffer()
		{
			if (mGL3)
				return new AgateLib.OpenGL.GL3.DrawBuffer();
			else
				return new AgateLib.OpenGL.Legacy.LegacyDrawBuffer();
		}


		#endregion

		protected override void OnBeginFrame()
		{
			mRenderTarget.BeginRender();

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Viewport(0, 0, mRenderTarget.Width, mRenderTarget.Height);
		}
		protected override void OnEndFrame()
		{
			DrawBuffer.Flush();

			mRenderTarget.EndRender();

			FlushDeleteQueue();
		}

		public GLDrawBuffer DrawBuffer
		{
			get { return ((GL_FrameBuffer)RenderTarget.Impl).DrawBuffer; }
		}

		// TODO: Test clip rect stuff.
		public override void SetClipRect(Rectangle newClipRect)
		{
			GL.Viewport(newClipRect.X, mRenderTarget.Height - newClipRect.Bottom,
				newClipRect.Width, newClipRect.Height);

			if (Display.Shader is AgateLib.DisplayLib.Shaders.IShader2D)
			{
				AgateLib.DisplayLib.Shaders.IShader2D s = (AgateLib.DisplayLib.Shaders.IShader2D)Display.Shader;

				s.CoordinateSystem = newClipRect;
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

		#region --- Drawing Primitives ---

		public override void DrawLine(Point a, Point b, Color color)
		{
			DrawBuffer.Flush();
			mPrimitives.DrawLine(a, b, color);
		}

		public override void DrawRect(Rectangle rect, Color color)
		{
			DrawRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
		}
		public override void DrawRect(RectangleF rect, Color color)
		{
			DrawBuffer.Flush();
			mPrimitives.DrawRect(rect, color);
		}

		public override void FillRect(Rectangle rect, Color color)
		{
			FillRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
		}
		public override void FillRect(RectangleF rect, Color color)
		{
			DrawBuffer.Flush();
			mPrimitives.FillRect(rect, color);
		}

		public override void FillRect(Rectangle rect, Gradient color)
		{
			FillRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
		}
		public override void FillRect(RectangleF rect, Gradient color)
		{
			DrawBuffer.Flush();
			mPrimitives.FillRect(rect, color);
		}

		public override void FillPolygon(PointF[] pts, int startIndex, int length, Color color)
		{
			DrawBuffer.Flush();
			mPrimitives.FillPolygon(pts, startIndex, length, color);
		}

		#endregion
		#region --- Initialization ---

		public override void Initialize()
		{
			CreateFakeWindow();

			Report("OpenTK / OpenGL driver instantiated for display.");
		}
		public void InitializeCurrentContext()
		{
			GL.ClearColor(0, 0, 0, 1.0f);                                     // Black Background
			GL.ClearDepth(1);                                                 // Depth Buffer Setup
			GL.Enable(EnableCap.DepthTest);                            // Enables Depth Testing
			GL.DepthFunc(DepthFunction.Lequal);                         // The Type Of Depth Testing To Do

			
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

		private void CreateFakeWindow()
		{
			mFakeWindow = new System.Windows.Forms.Form();
			mFakeDisplayWindow = DisplayWindow.CreateFromControl(mFakeWindow);

			mFakeWindow.Visible = false;

			string vendor = GL.GetString(StringName.Vendor);
			mSupportsShaders = false;

			mGLVersion = DetectOpenGLVersion();
			if (mGLVersion >= 3m)
			{
				if (ReadSettingsBool("EnableGL3"))
				{
					mGL3 = true;
				}
				else
				{
					mGL3 = false;
					mGLVersion = 2.1m;
				}
			}

			LoadExtensions();

			mSupportsFramebufferArb = SupportsExtension("GL_ARB_FRAMEBUFFER_OBJECT");
			mSupportsFramebufferExt = SupportsExtension("GL_EXT_FRAMEBUFFER_OBJECT");
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
                    "OpenGL 1.2 not aviable", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);

                throw new AgateLib.AgateException("OpenGL 1.2 or higher is required, but this system only supports OpenGL " + mGLVersion.ToString() + ".");
            }

			if (mGL3)
				mPrimitives = new AgateLib.OpenGL.GL3.GLPrimitiveRenderer();
			else
				mPrimitives = new AgateLib.OpenGL.Legacy.LegacyPrimitiveRenderer();

			if (SupportsExtension("GL_ARB_FRAGMENT_PROGRAM"))
			{
				mSupportsShaders = true;
			}

			ShaderFactory.Initialize(mGL3);

			Trace.WriteLine(string.Format("OpenGL version {0} from vendor {1} detected.", mGLVersion, vendor));
			Trace.WriteLine("NPOT: " + mNonPowerOf2Textures.ToString());
			Trace.WriteLine("Shaders: " + mSupportsShaders.ToString());

			CreateWhiteSurface();
		}

		string[] extensions;
		private void LoadExtensions()
		{
			if (mGL3)
			{
				// Forward compatible context (GL 3.0+)
				int num_extensions;
				GL.GetInteger(GetPName.NumExtensions, out num_extensions);

				if (GL.GetError() != ErrorCode.NoError)
					throw new OpenTK.Graphics.GraphicsErrorException("Not 3.0 context.");

				extensions = new string[num_extensions];

				for (int i = 0; i < num_extensions; i++)
					extensions[i] = GL.GetString(StringName.Extensions, i).ToLowerInvariant();
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

			decimal retval;

			if (decimal.TryParse(versionString, System.Globalization.NumberStyles.Number,
				System.Globalization.CultureInfo.InvariantCulture, out retval) == false)
			{
				Trace.WriteLine("AgateLib.OpenGL was unable to parse the OpenGL version string.");
				Trace.WriteLine("    The reported string was: " + versionString);
				Trace.WriteLine("    Please report this issue to http://www.agatelib.org along");
				Trace.WriteLine("    with details about your operating system and graphics drivers.");
				Trace.WriteLine("    Falling back to OpenGL 1.1 supported functionality.");

				retval = 1.1m;
			}

			return retval;
		}

		#endregion

		#region --- Shaders ---

		public override void Dispose()
		{
			mFakeDisplayWindow.Dispose();
			mFakeWindow.Dispose();
		}


		private void SetArray(float[] array, Vector3 vec)
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


		protected override void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
		{
			FormUtil.SavePixelBuffer(pixelBuffer, filename, format);
		}

		protected override void HideCursor()
		{
			System.Windows.Forms.Cursor.Hide();

			if (Display.CurrentWindow != null)
			{
				DisplayWindowImpl impl = Display.CurrentWindow.Impl;
				//((GL_DisplayW)impl).HideCursor();
			}
		}
		protected override void ShowCursor()
		{
			System.Windows.Forms.Cursor.Show();

			if (Display.CurrentWindow != null)
			{
				DisplayWindowImpl impl = Display.CurrentWindow.Impl;

				//((GL_FrameBufferExt)impl).ShowCursor();
			}
		}

		#region --- Display Capabilities ---

		public override bool CapsBool(DisplayBoolCaps caps)
		{
			switch (caps)
			{
				case DisplayBoolCaps.Scaling: return true;
				case DisplayBoolCaps.Rotation: return true;
				case DisplayBoolCaps.Color: return true;
				case DisplayBoolCaps.Gradient: return true;
				case DisplayBoolCaps.SurfaceAlpha: return true;
				case DisplayBoolCaps.PixelAlpha: return true;
				case DisplayBoolCaps.IsHardwareAccelerated: return true;
				case DisplayBoolCaps.FullScreen: return true;
				case DisplayBoolCaps.FullScreenModeSwitching: return true;
				case DisplayBoolCaps.CustomShaders: return false;
				case DisplayBoolCaps.CanCreateBitmapFont: return true;
			}

			return false;
		}
		public override Size CapsSize(DisplaySizeCaps displaySizeCaps)
		{
			switch (displaySizeCaps)
			{
				case DisplaySizeCaps.MaxSurfaceSize:
					int val;
					GL.GetInteger(GetPName.MaxTextureSize, out val);

					return new Size(val, val);
			}

			return new Size(0, 0);
		}
		public override IEnumerable<AgateLib.DisplayLib.Shaders.ShaderLanguage> SupportedShaderLanguages
		{
			get { yield return AgateLib.DisplayLib.Shaders.ShaderLanguage.Glsl; }
		}

		#endregion
		#region --- Render States ---

		bool mAlphaBlend;

		protected override bool GetRenderState(RenderStateBool renderStateBool)
		{
			switch (renderStateBool)
			{
				case RenderStateBool.WaitForVerticalBlank: return mVSync;
				case RenderStateBool.AlphaBlend: return mAlphaBlend;

				default:
					throw new NotSupportedException(string.Format(
						"The specified render state, {0}, is not supported by this driver."));
			}
		}
		protected override void SetRenderState(RenderStateBool renderStateBool, bool value)
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
						"The specified render state, {0}, is not supported by this driver."));
			}
		}

		#endregion

		#region --- Deletion queuing ---

		List<int> mTexturesToDelete = new List<int>();


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