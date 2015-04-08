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
using System.Text;

using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;

using Vector2 = SharpDX.Vector2;
using ImageFileFormat = AgateLib.DisplayLib.ImageFileFormat;
using SharpDX.DXGI;
using AgateLib.Diagnostics;
using Windows.UI.Core;
using SharpDX.SimpleInitializer;

namespace AgateLib.Platform.WindowsStore.DisplayImplementation
{
	public class SDX_Display : DisplayImpl
	{
		#region --- Private Variables ---

		private SharpDX.SimpleInitializer.SharpDXContext sdxContext { get { return InitializerContext; } }

		private D3DDevice mDevice;
		private SDX_FrameBuffer mRenderTarget;

		private bool mInitialized = false;

		// variables for drawing primitives
		PositionColor[] mLines = new PositionColor[5];
		PositionColor[] mFillRectVerts = new PositionColor[6];

		private bool mVSync = true;

		private bool mHasDepth;
		private bool mHasStencil;
		private float mDepthClear = 1.0f;
		private int mStencilClear = 0;

		Format mDepthStencilFormat;
		SharpDX.Direct3D11.DepthStencilState mDepthStencilState;

		#endregion

		AgateLib.DisplayLib.Surface mBlankSurface;

		public Format DepthStencilFormat
		{
			get { return mDepthStencilFormat; }
		}

		internal static CoreWindow MainThreadCoreWindow { get; set; }
		public static SharpDXContext InitializerContext { get; private set; }

		//public VertexDeclaration SurfaceDeclaration
		//{
		//	get { return mPosColorDecl; }
		//}

		//public DisplayMode DisplayMode
		//{
		//	get
		//	{
		//		return mDirect3Dobject.GetAdapterDisplayMode(0);
		//	}
		//}

		#region --- Creation / Destruction ---

		public SDX_Display()
		{
			MainThreadCoreWindow = CoreWindow.GetForCurrentThread();
		}

		void context_DeviceReset(object sender, SharpDX.SimpleInitializer.DeviceResetEventArgs e)
		{
			OnDeviceReset();

			PixelBuffer mBlankBuffer = new PixelBuffer(PixelFormat.RGBA8888, new Size(16,16));
			mBlankBuffer.Clear(Color.White);

			mBlankSurface = new DisplayLib.Surface(mBlankBuffer);

			mDepthStencilState = new SharpDX.Direct3D11.DepthStencilState(mDevice.Device,
				new SharpDX.Direct3D11.DepthStencilStateDescription
				{
					IsDepthEnabled = true,
					IsStencilEnabled = false,
					DepthComparison = SharpDX.Direct3D11.Comparison.LessEqual
				});

			Core.InitializeDefaultResources();
		}

		public override void Initialize()
		{
			Report("SharpDX driver instantiated for display.");
		}

		private void InitializeDeviceWrapper()
		{
			mDevice = new D3DDevice(sdxContext);
		}

		#endregion

		#region --- Implementation Specific Public Properties ---

		public D3DDevice D3D_Device
		{
			get { return mDevice; }
		}

		#endregion

		#region --- Events ---

		public event EventHandler DeviceReset;
		public event EventHandler DeviceLost;
		public event EventHandler DeviceAboutToReset;

		private void OnDeviceReset()
		{
			Log.WriteLine("{0} Device Reset", DateTime.Now);

			if (DeviceReset != null)
				DeviceReset(this, EventArgs.Empty);
		}

		private void OnDeviceLost()
		{
			Log.WriteLine("{0} Device Lost", DateTime.Now);

			if (DeviceLost != null)
				DeviceLost(this, EventArgs.Empty);
		}
		private void OnDeviceAboutToReset()
		{
			Log.WriteLine("{0} Device About to Reset", DateTime.Now);

			if (DeviceAboutToReset != null)
				DeviceAboutToReset(this, EventArgs.Empty);
		}


		#endregion
		#region --- Event Handlers ---

		void mDevice_DeviceReset(object sender, EventArgs e)
		{
			OnDeviceReset();
		}

		void mDevice_DeviceLost(object sender, EventArgs e)
		{
			OnDeviceLost();
		}


		#endregion

		#region --- Creation of objects ---

		protected override AgateLib.DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader builtInShaderType)
		{
			return Shaders.ShaderFactory.CreateBuiltInShader(builtInShaderType);
		}
		[Obsolete]
		ShaderCompilerImpl CreateShaderCompiler()
		{
			throw new NotImplementedException();
			//return new HlslCompiler(this);
		}



		#endregion

		#region --- BeginFrame stuff and DeltaTime ---

		public bool InFrame { get; private set; }

		protected override void OnBeginFrame()
		{
			if (mRenderTarget is FrameBufferWindow)
				FrameCount++;

			mRenderTarget.BeginRender();

			SetClipRect(new AgateLib.Geometry.Rectangle(new AgateLib.Geometry.Point(0, 0), mRenderTarget.Size));
			mDevice.DeviceContext.OutputMerger.SetDepthStencilState(mDepthStencilState);

			InFrame = true;
		}
		protected override void OnEndFrame()
		{
			mDevice.DrawBuffer.Flush();
			mRenderTarget.EndRender();

			InFrame = false;
		}

		#endregion

		#region --- Clip Rect stuff ---

		public override void SetClipRect(AgateLib.Geometry.Rectangle newClipRect)
		{
			if (newClipRect.X < 0)
			{
				newClipRect.Width += newClipRect.X;
				newClipRect.X = 0;
			}
			if (newClipRect.Y < 0)
			{
				newClipRect.Height += newClipRect.Y;
				newClipRect.Y = 0;
			}
			if (newClipRect.Right > mRenderTarget.Width)
			{
				newClipRect.Width -= newClipRect.Right - mRenderTarget.Width;
			}
			if (newClipRect.Bottom > mRenderTarget.Height)
			{
				newClipRect.Height -= newClipRect.Bottom - mRenderTarget.Height;
			}

			if (mRenderTarget.Width == 0 || mRenderTarget.Height == 0)
				return;

			var view = new SharpDX.Viewport();

			view.X = newClipRect.X;
			view.Y = newClipRect.Y;
			view.Width = newClipRect.Width;
			view.Height = newClipRect.Height;
			view.MinDepth = 0;
			view.MaxDepth = 1;

			if (view.Width == 0 || view.Height == 0)
			{
				throw new AgateLib.AgateException("Cannot set a cliprect with a width / height of zero.");
			}

			mDevice.DeviceContext.Rasterizer.SetViewport(view);
			mCurrentClipRect = newClipRect;

			if (Display.Shader is AgateLib.DisplayLib.Shaders.IShader2D)
			{
				var s2d = (AgateLib.DisplayLib.Shaders.IShader2D)Display.Shader;

				s2d.CoordinateSystem = newClipRect;
			}
		}

		private Stack<AgateLib.Geometry.Rectangle> mClipRects = new Stack<AgateLib.Geometry.Rectangle>();
		private AgateLib.Geometry.Rectangle mCurrentClipRect;

		#endregion
		#region --- Methods for drawing to the back buffer ---

		public override void Clear(Color color)
		{
			mDevice.DrawBuffer.Flush();

			mDevice.Clear(color, mDepthClear, mStencilClear);
		}
		public override void Clear(Color color, Rectangle destRect)
		{
			throw new NotImplementedException();
			mDevice.DrawBuffer.Flush();
		}

		public override void DrawLine(Point a, Point b, Color color)
		{
			mDevice.DrawBuffer.Flush();

			mLines[0] = new PositionColor(a.X, a.Y, 0, color.ToArgb());
			mLines[1] = new PositionColor(b.X, b.Y, 0, color.ToArgb());

			mDevice.SetDeviceStateTexture(null);
			//mDevice.AlphaArgument1 = TextureArgument.Diffuse;

			//mDevice.Device.VertexDeclaration = mPosColorDecl;
			//mDevice.Device.DrawUserPrimitives(SlimDX.Direct3D9.PrimitiveType.LineList, 1, mLines);

			//mDevice.AlphaArgument1 = TextureArgument.Texture;
		}
		public override void DrawLines(Point[] pt, Color color)
		{
			mDevice.DrawBuffer.Flush();

			if (pt.Length > mLines.Length)
				mLines = new PositionColor[pt.Length];

			for (int i = 0; i < pt.Length; i++)
			{
				mLines[i] = new PositionColor(pt[i].X, pt[i].Y, 0, color.ToArgb());
			}

			mDevice.SetDeviceStateTexture(null);
			//mDevice.Device.SetTextureStageState(0, TextureStage.ColorArg0, TextureArgument.Diffuse);
			//mDevice.Device.VertexDeclaration = mPosColorDecl; 
			//mDevice.Device.DrawUserPrimitives(Direct3D.PrimitiveType.LineStrip, pt.Length - 1, mLines);
			//mDevice.Device.SetTextureStageState(0, TextureStage.ColorArg0, TextureArgument.Current);
		}
		public override void DrawRect(Rectangle rect, Color color)
		{
			DrawRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
		}
		public override void DrawRect(RectangleF rect, Color color)
		{
			mDevice.DrawBuffer.Flush();

			int c = color.ToArgb();

			mLines[0] = new PositionColor(rect.X, rect.Y, 0, c);
			mLines[1] = new PositionColor(rect.Right, rect.Y, 0, c);
			mLines[2] = new PositionColor(rect.Right, rect.Bottom, 0, c);
			mLines[3] = new PositionColor(rect.X, rect.Bottom, 0, c);
			mLines[4] = new PositionColor(rect.X, rect.Y, 0, c);

			mDevice.SetDeviceStateTexture(null);
			//mDevice.Device.SetTextureStageState(0, TextureStage.ColorArg0, TextureArgument.Diffuse);
			//mDevice.Device.VertexDeclaration = mPosColorDecl;
			//mDevice.Device.DrawUserPrimitives(Direct3D.PrimitiveType.LineStrip, 4, mLines);
			//mDevice.Device.SetTextureStageState(0, TextureStage.ColorArg0, TextureArgument.Current);
		}

		PositionColor[] polygonVerts = new PositionColor[10];

		public override void FillPolygon(PointF[] pts, int startIndex, int length, Color color)
		{
			if (polygonVerts.Length < length)
				polygonVerts = new PositionColor[length];

			int clr = color.ToArgb();

			for (int i = 0; i < length; i++)
			{
				polygonVerts[i].Position =
					new AgateLib.Geometry.Vector3(pts[startIndex + i].X, pts[startIndex + i].Y, 0);
				polygonVerts[i].Color = clr;
			}

			mDevice.DrawBuffer.Flush();

			mDevice.AlphaBlend = true;

			mDevice.SetDeviceStateTexture(null);
			//mDevice.AlphaArgument1 = TextureArgument.Diffuse;

			//mDevice.Device.VertexDeclaration = mPosColorDecl;
			//mDevice.Device.DrawUserPrimitives(Direct3D.PrimitiveType.TriangleFan, length - 2, polygonVerts);
			//mDevice.AlphaArgument1 = TextureArgument.Texture;
		}
		public override void FillRect(Rectangle rect, Color color)
		{
			FillRect((RectangleF)rect, new Gradient(color));
		}
		public override void FillRect(RectangleF rect, Color color)
		{
			FillRect(rect, new Gradient(color));
		}
		public override void FillRect(Rectangle rect, Gradient color)
		{
			mBlankSurface.ColorGradient = color;

			mBlankSurface.Draw(rect);

		}
		public override void FillRect(RectangleF rect, Gradient color)
		{
			FillRect((Rectangle)rect, color);
		}




		#endregion

		#region --- Display Mode changing stuff ---

		protected override void OnRenderTargetChange(FrameBuffer oldRenderTarget)
		{
			mRenderTarget = (SDX_FrameBuffer)RenderTarget.Impl;
			mDevice.RenderTarget = mRenderTarget;
		}

		//private Format GetDepthFormat(Format backbufferFormat)
		//{
		//	Format[] formats = new Format[]
		//	{
		//		Format.D24X4S4,
		//		Format.D24X8,
		//		Format.D15S1,
		//		Format.D32,
		//		Format.D16,
		//	};

		//	var adapter = mDirect3Dobject.Adapters.DefaultAdapter.Adapter;
		//	Format deviceFormat = GetDeviceFormat(backbufferFormat);

		//	foreach (var depthFormat in formats)
		//	{
		//		if (mDirect3Dobject.CheckDeviceFormat(adapter, DeviceType.Hardware, deviceFormat,
		//			Usage.DepthStencil, ResourceType.Surface, depthFormat) == false)
		//		{
		//			continue;
		//		}
		//		if (mDirect3Dobject.CheckDepthStencilMatch(adapter, DeviceType.Hardware,
		//			deviceFormat, backbufferFormat, depthFormat) == false)
		//		{
		//			continue;
		//		}

		//		return depthFormat;
		//	}

		//	return Format.Unknown;
		//}
		//private Format GetDeviceFormat(Format backbufferFormat)
		//{
		//	switch (backbufferFormat)
		//	{
		//		case Format.A8R8G8B8: return Format.X8R8G8B8;
		//		case Format.A8B8G8R8: return Format.X8B8G8R8;
		//		case Format.A1R5G5B5: return Format.X1R5G5B5;

		//		default:
		//			return backbufferFormat;
		//	}
		//}

		//static Format[] probeScreenFormats = new Format[] 
		//	{ Format.X8B8G8R8, Format.X8R8G8B8, /*Format.R8G8B8, Format.R5G6B5,*/};

		//public override ScreenMode[] EnumScreenModes()
		//{
		//	List<ScreenMode> modes = new List<ScreenMode>();

		//	foreach (var fmt in probeScreenFormats)
		//	{
		//		DisplayModeCollection dxmodes = mDirect3Dobject.Adapters[0].GetDisplayModes(fmt);
		//		ConvertDisplayModesToScreenModes(modes, dxmodes);
		//	}

		//	return modes.ToArray();
		//}

		//private static void ConvertDisplayModesToScreenModes(List<ScreenMode> modes, IEnumerable<DisplayMode> dxmodes)
		//{
		//	foreach (DisplayMode mode in dxmodes)
		//	{
		//		int bits;

		//		switch (mode.Format)
		//		{
		//			case Format.A8B8G8R8:
		//			case Format.X8B8G8R8:
		//			case Format.X8R8G8B8:
		//			case Format.A8R8G8B8:
		//				bits = 32;
		//				break;

		//			case Format.R8G8B8:
		//				bits = 24;
		//				break;

		//			case Format.R5G6B5:
		//			case Format.X4R4G4B4:
		//			case Format.X1R5G5B5:
		//				bits = 16;
		//				break;

		//			default:
		//				continue;
		//		}

		//		modes.Add(new ScreenMode(mode.Width, mode.Height, bits));
		//	}
		//}
		//Format GetDisplayModeTrialPixelFormat(int bpp)
		//{
		//	switch (bpp)
		//	{
		//		case 32:
		//			return Format.X8R8G8B8;
		//		case 24:
		//			return Format.R8G8B8;
		//		case 16:
		//			return Format.R5G6B5;
		//		case 15:
		//			return Format.X1R5G5B5;
		//		default:
		//			throw new ArgumentException("Bits per pixel must be 32, 16, or 15.");
		//	}
		//}
		//private void SelectBestDisplayMode(PresentParameters present, int bpp)
		//{
		//	Format fmt = GetDisplayModeTrialPixelFormat(bpp);

		//	DisplayModeCollection modes = mDirect3Dobject.Adapters[0].GetDisplayModes(fmt);

		//	DisplayMode selected = new DisplayMode();
		//	int diff = 0;

		//	foreach (DisplayMode mode in modes)
		//	{
		//		if (mode.Width < present.BackBufferWidth)
		//			continue;

		//		if (mode.Height < present.BackBufferHeight)
		//			continue;

		//		int thisDiff = Math.Abs(present.BackBufferWidth - mode.Width)
		//			+ Math.Abs(present.BackBufferHeight - mode.Height);

		//		int bits = 0;

		//		switch (mode.Format)
		//		{
		//			case Format.A8B8G8R8:
		//			case Format.X8B8G8R8:
		//				thisDiff += 10;
		//				goto case Format.X8R8G8B8;

		//			case Format.X8R8G8B8:
		//			case Format.A8R8G8B8:
		//				bits = 32;
		//				break;

		//			case Format.R5G6B5:
		//			case Format.X4R4G4B4:
		//			case Format.X1R5G5B5:
		//				bits = 16;
		//				break;

		//			default:
		//				System.Diagnostics.Debug.Print("Unknown backbuffer format {0}.", mode.Format);
		//				continue;
		//		}

		//		thisDiff += Math.Abs(bits - bpp);

		//		// first mode by default, or any mode which is a better match.
		//		if (selected.Height == 0 || thisDiff < diff)
		//		{
		//			selected = mode;
		//			diff = thisDiff;
		//		}

		//	}

		//	present.BackBufferFormat = selected.Format;
		//	present.BackBufferWidth = selected.Width;
		//	present.BackBufferHeight = selected.Height;
		//}


		#endregion

		#region --- Drawing Helper Functions ---

		#endregion

		#region --- Pixel Format ---

		internal int GetPixelPitch(Format format)
		{
			switch (format)
			{
				case Format.B8G8R8A8_Typeless:
				case Format.B8G8R8A8_UNorm:
				case Format.B8G8R8A8_UNorm_SRgb:
				case Format.B8G8R8X8_Typeless:
				case Format.B8G8R8X8_UNorm:
				case Format.B8G8R8X8_UNorm_SRgb:
					return 4;

				case Format.B5G5R5A1_UNorm:
				case Format.B5G6R5_UNorm:
					return 2;

				default:
					throw new NotSupportedException("Format not supported.");
			}
		}
		internal PixelFormat GetPixelFormat(Format format)
		{
			throw new NotImplementedException();
			//switch (format)
			//{
			//	case Format.B8G8R8A8_UNorm: return PixelFormat.BGRA8888;
			//	case Format.A8B8G8R8: return PixelFormat.RGBA8888;
			//	case Format.X8B8G8R8: return PixelFormat.RGBA8888; // TODO: fix this one
			//	case Format.X8R8G8B8: return PixelFormat.BGRA8888; // TODO: fix this one

			//	default:
			//		throw new NotSupportedException("Format not supported.");

			//}
		}
		public override PixelFormat DefaultSurfaceFormat
		{
			get
			{
				throw new NotImplementedException();
				//return GetPixelFormat(DisplayMode.Format); 
			}
		}

		#endregion

		public override void FlushDrawBuffer()
		{
			mDevice.DrawBuffer.Flush();
		}

		protected override void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
		{
			throw new NotImplementedException();
			//FormUtil.SavePixelBuffer(pixelBuffer, filename, format);
		}

		protected override void HideCursor()
		{
		}
		protected override void ShowCursor()
		{
		}

		#region --- IDisplayCaps Members ---

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
				case DisplayBoolCaps.CustomShaders: return true;
				case DisplayBoolCaps.CanCreateBitmapFont: return true;
			}

			return false;
		}
		public override Size CapsSize(DisplaySizeCaps displaySizeCaps)
		{
			switch (displaySizeCaps)
			{
				case DisplaySizeCaps.MaxSurfaceSize: return mDevice.MaxSurfaceSize;
				case DisplaySizeCaps.NativeScreenResolution: return RenderTargetAdapter.Size;
				default:
					throw new NotImplementedException();
			}
		}

		public override IEnumerable<AgateLib.DisplayLib.Shaders.ShaderLanguage> SupportedShaderLanguages
		{
			get { yield return AgateLib.DisplayLib.Shaders.ShaderLanguage.Hlsl; }
		}

		#endregion

		#region --- 3D stuff ---

		protected override VertexBufferImpl CreateVertexBuffer(
			AgateLib.Geometry.VertexTypes.VertexLayout layout, int vertexCount)
		{
			return new SDX_VertexBuffer(this, layout, vertexCount);
		}
		protected override IndexBufferImpl CreateIndexBuffer(IndexBufferType type, int size)
		{
			return new SDX_IndexBuffer(this, type, size);
		}

		#endregion

		internal event EventHandler VSyncChanged;

		private void OnVSyncChanged()
		{
			if (VSyncChanged != null)
				VSyncChanged(this, EventArgs.Empty);
		}

		protected override bool GetRenderState(RenderStateBool renderStateBool)
		{
			switch (renderStateBool)
			{
				case RenderStateBool.WaitForVerticalBlank: return mVSync;
				//case RenderStateBool.ZBufferTest: return mDevice.Device.GetRenderState<bool>(RenderState.ZEnable);
				//case RenderStateBool.StencilBufferTest: return mDevice.Device.GetRenderState<bool>(RenderState.StencilEnable);
				case RenderStateBool.AlphaBlend: return mDevice.AlphaBlend;
				//case RenderStateBool.ZBufferWrite: return mDevice.Device.GetRenderState<bool>(RenderState.ZWriteEnable);

				default:
					throw new NotSupportedException(string.Format(
						"The specified render state, {0}, is not supported by this driver.", renderStateBool));
			}
		}

		protected override void SetRenderState(RenderStateBool renderStateBool, bool value)
		{
			switch (renderStateBool)
			{
				case RenderStateBool.WaitForVerticalBlank:
					mVSync = value;
					OnVSyncChanged();
					break;

				case RenderStateBool.ZBufferTest:
					//mDevice.Device.SetRenderState(RenderState.ZEnable, value);
					throw new NotImplementedException();
					return;

				case RenderStateBool.StencilBufferTest:
					//mDevice.Device.SetRenderState(RenderState.StencilEnable, value);
					throw new NotImplementedException();
					return;

				case RenderStateBool.AlphaBlend:
					mDevice.AlphaBlend = value;
					break;

				case RenderStateBool.ZBufferWrite:
					//mDevice.Device.SetRenderState(RenderState.ZWriteEnable, value);
					throw new NotImplementedException();
					break;

				default:
					throw new NotSupportedException(string.Format(
						"The specified render state, {0}, is not supported by this driver."));
			}
		}


		public override double CapsDouble(DisplayDoubleCaps caps)
		{
			throw new NotImplementedException();
		}

		protected override void OnRenderTargetResize()
		{
			throw new NotImplementedException();
		}

		public static bool PauseWhenNotRendering { get; set; }

		public static bool RenderingFrame { get; set; }

		public IRenderTargetAdapter RenderTargetAdapter { get; private set; }

		public void ResetRenderTarget(IRenderTargetAdapter adapter)
		{
			if (InitializerContext  != null)
			{
				InitializerContext.Dispose();
				D3D_Device.Dispose();
			}

			RenderTargetAdapter = adapter;

			InitializerContext = new SharpDXContext();
			InitializerContext.DeviceReset += context_DeviceReset;

			InitializeDeviceWrapper();
		}

		public static int FrameCount { get; set; }

		public static bool WaitingForMainThread { get; set; }
	}
}