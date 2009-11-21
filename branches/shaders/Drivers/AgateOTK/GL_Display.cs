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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
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
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using AgateLib.ImplementationBase;
using OpenTK.Graphics.OpenGL;
using PixelFormat = AgateLib.DisplayLib.PixelFormat;

namespace AgateOTK
{
	public sealed class GL_Display : DisplayImpl
	{
		GL_IRenderTarget mRenderTarget;
		GLState mState;
		Stack<Rectangle> mClipRects = new Stack<Rectangle>();
		Rectangle mCurrentClip = Rectangle.Empty;
		private bool mVSync = true;
		private int mMaxLightsUsed = 0;
		private bool mSupportsFramebuffer;
		private bool mNonPowerOf2Textures;
		private bool mSupportsShaders;
		private decimal mGLVersion;

		System.Windows.Forms.Form mFakeWindow;
		DisplayWindow mFakeDisplayWindow;

		public bool NonPowerOf2Textures
		{
			get { return mNonPowerOf2Textures; }
			private set { mNonPowerOf2Textures = value; }
		}

		public bool SupportsFramebuffer
		{
			get { return mSupportsFramebuffer; }
			set { mSupportsFramebuffer = value; }
		}

		internal event EventHandler ProcessEventsEvent;

		protected override void ProcessEvents()
		{
			if (ProcessEventsEvent != null)
				ProcessEventsEvent(this, EventArgs.Empty);
		}

		protected override void OnRenderTargetChange(IRenderTarget oldRenderTarget)
		{
			mRenderTarget = RenderTarget.Impl as GL_IRenderTarget;
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

		protected override AgateLib.DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader BuiltInShaderType)
		{
			return Shaders.ShaderFactory.CreateBuiltInShader(BuiltInShaderType);
		}
		public override DisplayWindowImpl CreateDisplayWindow(CreateWindowParams windowParams)
		{
			return new GL_DisplayControl(windowParams);

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
			return new GL_VertexBuffer(layout, vertexCount);
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

			return AgateLib.WinForms.BitmapFontUtil.ConstructFromOSFont(options);
		}
		public override FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			return AgateLib.WinForms.BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
		}

		internal void SetupGLOrtho(Rectangle ortho)
		{
			SetOrthoProjection(ortho);

			GL.Enable(EnableCap.Texture2D);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
		}

		protected override void OnBeginFrame()
		{
			mRenderTarget.BeginRender();
		}
		protected override void OnEndFrame()
		{
			mState.DrawBuffer.Flush();

			mRenderTarget.EndRender();

			FlushDeleteQueue();
		}


		internal GLState State
		{
			get { return mState; }
		}


		// TODO: Test clip rect stuff.
		public override void SetClipRect(Rectangle newClipRect)
		{
			GL.Viewport(newClipRect.X, mRenderTarget.Height - newClipRect.Bottom,
				newClipRect.Width, newClipRect.Height);

			SetupGLOrtho(newClipRect);

			mCurrentClip = newClipRect;
		}

		public override void FlushDrawBuffer()
		{
			mState.DrawBuffer.Flush();
		}

		public override void SetOrthoProjection(Rectangle region)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(region.Left, region.Right, region.Bottom, region.Top, -1, 1);
		}

		Matrix4x4 projection = Matrix4x4.Identity;
		Matrix4x4 world = Matrix4x4.Identity;
		Matrix4x4 view = Matrix4x4.Identity;

		public override Matrix4x4 MatrixProjection
		{
			get { return projection; }
			set
			{
				projection = value;
				SetProjection();
			}
		}
		public override Matrix4x4 MatrixView
		{
			get { return view; }
			set
			{
				view = value;
				SetModelview();
			}
		}
		public override Matrix4x4 MatrixWorld
		{
			get { return world; }
			set
			{
				world = value;
				SetModelview();
			}
		}

		private void SetModelview()
		{
			OpenTK.Matrix4 modelview = GeoHelper.ConvertAgateMatrix(view * world, false);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.LoadMatrix(ref modelview);
		}
		private void SetProjection()
		{
			OpenTK.Matrix4 otkProjection = GeoHelper.ConvertAgateMatrix(projection, false);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.LoadMatrix(ref otkProjection);
		}

		public override void Clear(Color color)
		{
			mState.DrawBuffer.Flush();

			GL.ClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, 1.0f);
			GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
		}
		public override void Clear(Color color, Rectangle dest)
		{
			mState.DrawBuffer.Flush();

			DrawRect(dest, Color.FromArgb(255, color));
		}

		public override void DrawLine(Point a, Point b, Color color)
		{
			mState.DrawBuffer.Flush();
			mState.SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Lines);
			GL.Vertex2(a.X, a.Y);
			GL.Vertex2(b.X, b.Y);

			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		public override void DrawRect(Rectangle rect, Color color)
		{
			DrawRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
		}
		public override void DrawRect(RectangleF rect, Color color)
		{
			mState.DrawBuffer.Flush();
			mState.SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Lines);

			GL.Vertex2(rect.Left, rect.Top);
			GL.Vertex2(rect.Right, rect.Top);

			GL.Vertex2(rect.Right, rect.Top);
			GL.Vertex2(rect.Right, rect.Bottom);

			GL.Vertex2(rect.Left, rect.Bottom);
			GL.Vertex2(rect.Right, rect.Bottom);

			GL.Vertex2(rect.Left, rect.Top);
			GL.Vertex2(rect.Left, rect.Bottom);

			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		public override void FillRect(Rectangle rect, Color color)
		{
			FillRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
		}
		public override void FillRect(RectangleF rect, Color color)
		{
			mState.DrawBuffer.Flush();

			mState.SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);

			GL.Begin(BeginMode.Quads);
			GL.Vertex3(rect.Left, rect.Top, 0);                                        // Top Left
			GL.Vertex3(rect.Right, rect.Top, 0);                                         // Top Right
			GL.Vertex3(rect.Right, rect.Bottom, 0);                                        // Bottom Right
			GL.Vertex3(rect.Left, rect.Bottom, 0);                                       // Bottom Left
			GL.End();                                                         // Done Drawing The Quad

			GL.Enable(EnableCap.Texture2D);
		}

		public override void FillRect(Rectangle rect, Gradient color)
		{
			FillRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
		}
		public override void FillRect(RectangleF rect, Gradient color)
		{
			mState.DrawBuffer.Flush();

			GL.Disable(EnableCap.Texture2D);

			GL.Begin(BeginMode.Quads);
			mState.SetGLColor(color.TopLeft);
			GL.Vertex3(rect.Left, rect.Top, 0);                                        // Top Left

			mState.SetGLColor(color.TopRight);
			GL.Vertex3(rect.Right, rect.Top, 0);                                         // Top Right

			mState.SetGLColor(color.BottomRight);
			GL.Vertex3(rect.Right, rect.Bottom, 0);                                        // Bottom Right

			mState.SetGLColor(color.BottomLeft);
			GL.Vertex3(rect.Left, rect.Bottom, 0);                                       // Bottom Left
			GL.End();                                                         // Done Drawing The Quad

			GL.Enable(EnableCap.Texture2D);
		}

		public override void FillPolygon(PointF[] pts, Color color)
		{
			mState.DrawBuffer.Flush();

			GL.Disable(EnableCap.Texture2D);

			mState.SetGLColor(color);

			GL.Begin(BeginMode.TriangleFan);
			for (int i = 0; i < pts.Length; i++)
			{
				GL.Vertex3(pts[i].X, pts[i].Y, 0);
			}
			GL.End();                                                         // Done Drawing The Quad

			GL.Enable(EnableCap.Texture2D);
		}

		public override void Initialize()
		{
			CreateFakeWindow();
			mState = new GLState();
			
			Report("OpenTK / OpenGL driver instantiated for display.");
		}
		public void InitializeCurrentContext()
		{

			//GL.ShadeModel(ShadingModel.Smooth);                         // Enable Smooth Shading
			GL.ClearColor(0, 0, 0, 1.0f);                                     // Black Background
			GL.ClearDepth(1);                                                 // Depth Buffer Setup
			GL.Enable(EnableCap.DepthTest);                            // Enables Depth Testing
			GL.DepthFunc(DepthFunction.Lequal);                         // The Type Of Depth Testing To Do
			//GL.Hint(HintTarget.PerspectiveCorrectionHint,             // Really Nice Perspective Calculations
			//	HintMode.Nicest);

		}
		private void CreateFakeWindow()
		{
			mFakeWindow = new System.Windows.Forms.Form();
			mFakeDisplayWindow = DisplayWindow.CreateFromControl(mFakeWindow);

			mFakeWindow.Visible = false;

			string vendor = GL.GetString(StringName.Vendor);
			mSupportsShaders = false;

			mGLVersion = DetectOpenGLVersion();
			LoadExtensions();

			mSupportsFramebuffer = SupportsExtension("GL_EXT_FRAMEBUFFER_OBJECT");
			mNonPowerOf2Textures = SupportsExtension("GL_ARB_NON_POWER_OF_TWO");

			if (mGLVersion >= 3m)
			{
				mSupportsFramebuffer = true;
				mNonPowerOf2Textures = true;
				mSupportsShaders = true;
			}
			if (mGLVersion >= 2m)
			{
				mNonPowerOf2Textures = true;
				mSupportsShaders = true;
			}

			if (SupportsExtension("GL_ARB_FRAGMENT_PROGRAM"))
			{
				mSupportsShaders = true;
			}

			Trace.WriteLine(string.Format("OpenGL version {0} from vendor {1} detected.", mGLVersion, vendor));
			Trace.WriteLine("Framebuffer: " + mSupportsFramebuffer.ToString());
			Trace.WriteLine("NPOT: " + mNonPowerOf2Textures.ToString());
			Trace.WriteLine("Shaders: " + mSupportsShaders.ToString());

			InitializeShaders();
		}

		string[] extensions;
		private void LoadExtensions()
		{
			try
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
			catch (OpenTK.Graphics.GraphicsErrorException)
			{
				string ext = GL.GetString(StringName.Extensions);

				extensions = ext.Split(' ');

				for (int i = 0; i < extensions.Length; i++)
					Debug.Print(extensions[i]);
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
				Trace.WriteLine("AgateOTK was unable to parse the OpenGL version string.");
				Trace.WriteLine("    The reported string was: " + versionString);
				Trace.WriteLine("    Please report this issue to http://www.agatelib.org along");
				Trace.WriteLine("    with details about your operating system and graphics drivers.");
				Trace.WriteLine("    Falling back to OpenGL 1.1 supported functionality.");

				retval = 1.1m;
			}

			return retval;
		}


		OtkShader mCurrentShader;

		public OtkShader Shader
		{
			get
			{
				return mCurrentShader;
			}
			set
			{
				if (value == null)
					return;

				if (value is OtkShader == false)
					throw new AgateLib.AgateException(string.Format(
						"Shader type is {0} but must be IGlShader.", value.GetType()));

				mCurrentShader = (OtkShader)value;

				if (mCurrentShader == null)
				{
					GL.UseProgram(0);
				}
				else
				{
					GL.UseProgram(mCurrentShader.Handle);
				}

			}
		}
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

		#region --- Shaders ---

		protected override ShaderCompilerImpl CreateShaderCompiler()
		{
			if (this.Supports(DisplayBoolCaps.CustomShaders))
			{
				if (mGLVersion < 2.0m)
					return new ArbShaderCompiler();
				else
					return new GlslShaderCompiler();
			}
			else
				return base.CreateShaderCompiler();
		}

		#endregion


		protected override void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
		{
			AgateLib.WinForms.FormUtil.SavePixelBuffer(pixelBuffer, filename, format);
		}

		protected override void HideCursor()
		{
			System.Windows.Forms.Cursor.Hide();

			if (Display.CurrentWindow != null)
			{
				DisplayWindowImpl impl = Display.CurrentWindow.Impl;
				((GL_IRenderTarget)impl).HideCursor();
			}
		}
		protected override void ShowCursor()
		{
			System.Windows.Forms.Cursor.Show();

			if (Display.CurrentWindow != null)
			{
				DisplayWindowImpl impl = Display.CurrentWindow.Impl;
				((GL_IRenderTarget)impl).ShowCursor();
			}
		}

		#region --- IDisplayCaps Members ---

		public override bool Supports(DisplayBoolCaps caps)
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
				case DisplaySizeCaps.MaxSurfaceSize: return new Size(1024, 1024);
			}

			return new Size(0, 0);
		}
		public override IEnumerable<AgateLib.DisplayLib.Shaders.ShaderLanguage> SupportedShaderLanguages
		{
			get { yield return AgateLib.DisplayLib.Shaders.ShaderLanguage.Glsl; }
		}


		#endregion

		protected override bool GetRenderState(RenderStateBool renderStateBool)
		{
			switch (renderStateBool)
			{
				case RenderStateBool.WaitForVerticalBlank: return mVSync;
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

				default:
					throw new NotSupportedException(string.Format(
						"The specified render state, {0}, is not supported by this driver."));
			}
		}

		#region --- Deletion queuing ---

		List<int> mTexturesToDelete = new List<int>();


		private void FlushDeleteQueue()
		{
			lock (mTexturesToDelete)
			{
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