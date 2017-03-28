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
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which can be used to query information about what features are supported
	/// by the display driver.
	/// </summary>
	public class DisplayCapsInfo
	{
		internal DisplayCapsInfo()
		{ }

		/// <summary>
		/// Indicates whether or not full screen windows can be created.
		/// </summary>
		public bool SupportsFullScreen
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.FullScreen); }
		}
		/// <summary>
		/// Indicates whether or not the screen resolution can be changed.
		/// If the Display driver supports full screen but not mode switching,
		/// then a DisplayWindow which is created with as a full screen window
		/// cannot change resolutions after it is initially set.
		/// </summary>
		public bool SupportsFullScreenModeSwitching
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.FullScreenModeSwitching); }
		}
		/// <summary>
		/// Indicates whether setting Surface.SetScale has any visible effect.
		/// </summary>
		public bool SupportsScaling
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.Scaling); }
		}
		/// <summary>
		///  Indicates whether setting Surface.RotationAngle has any visible effect.
		/// </summary>
		public bool SupportsRotation
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.Rotation); }
		}
		/// <summary>
		/// Indicates whether setting Surface.Color has any visible effect.
		/// </summary>
		public bool SupportsColor
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.Color); }
		}
		/// <summary>
		/// Indicates whether Surface gradients are supported.  If not, then setting Surface.ColorGradient
		/// color of a surface is the same as setting the Surface.Color with the average of the
		/// gradient colors.
		/// </summary>
		public bool SupportsGradient
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.Gradient); }
		}
		/// <summary>
		/// Indicates whether setting Surface.Alpha has any visible effect.
		/// </summary>
		public bool SupportsSurfaceAlpha
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.SurfaceAlpha); }
		}
		/// <summary>
		/// Indicates whether the alpha channel in surface pixels is used.
		/// </summary>
		public bool SupportsPixelAlpha
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.PixelAlpha); }
		}
		/// <summary>
		/// Indicates whether there is hardware acceleration available for 2D and 3D drawing.
		/// </summary>
		public bool IsHardwareAccelerated
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.IsHardwareAccelerated); }
		}
		/// <summary>
		/// Indicates whether or not vertex/pixel shaders are supported.
		/// </summary>
		public bool SupportsCustomShaders
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.CustomShaders); }
		}
		/// <summary>
		/// Indicates which shader language is supported.
		/// </summary>
		public ShaderLanguage ShaderLanguage
		{
			get 
			{ 
				var shad = Display.Impl.SupportedShaderLanguages.ToList(); 
			
				if (shad.Count == 0)
					return ShaderLanguage.None;
				else
					return shad[0];
			}
		}
		/// <summary>
		/// Indicates whether the driver can create a bitmap font from an operating
		/// system font.
		/// </summary>
		public bool CanCreateBitmapFont
		{
			get { return Display.Impl.CapsBool(DisplayBoolCaps.CanCreateBitmapFont); }
		}

		/// <summary>
		/// Gets the maximum size a surface can be.
		/// </summary>
		public Size MaxSurfaceSize
		{
			get { return Display.Impl.CapsSize(DisplaySizeCaps.MaxSurfaceSize); }
		}

		/// <summary>
		/// Gets the native resolution of the main screen.
		/// </summary>
		public Size NativeScreenResolution
		{
			get { return Display.Impl.CapsSize(DisplaySizeCaps.NativeScreenResolution); }
		}

		/// <summary>
		/// Gets the aspect ratio of the monitor.
		/// </summary>
		public double AspectRatio
		{
			get { return Display.Impl.CapsDouble(DisplayDoubleCaps.AspectRatio); }
		}
	}

	/// <summary>
	/// Enum which is used to specify a Caps value which should return a Size object.
	/// </summary>
	public enum DisplaySizeCaps
	{
		/// <summary>
		/// Value for getting the maximum supported surface size.
		/// </summary>
		MaxSurfaceSize,

		/// <summary>
		/// Value for getting the native screen resolution.
		/// </summary>
		NativeScreenResolution,
	}
	/// <summary>
	/// Enum which is used to specify a Caps value which should return a logical value.
	/// </summary>
	public enum DisplayBoolCaps
	{
		/// <summary>
		/// Indicates whether or not full screen windows can be created.
		/// </summary>
		FullScreen,
		/// <summary>
		/// Indicates whether or not the screen resolution can be changed.
		/// If the Display driver supports full screen but not mode switching,
		/// then a DisplayWindow which is created with as a full screen window
		/// cannot change resolutions after it is initially set.
		/// </summary>
		FullScreenModeSwitching,
		/// <summary>
		/// Indicates whether setting Surface.SetScale has any visible effect.
		/// </summary>
		Scaling,
		/// <summary>
		///  Indicates whether setting Surface.RotationAngle has any visible effect.
		/// </summary>
		Rotation,
		/// <summary>
		/// Indicates whether setting Surface.Color has any visible effect.
		/// </summary>
		Color,
		/// <summary>
		/// Indicates whether Surface gradients are supported.  If not, then setting Surface.ColorGradient
		/// color of a surface is the same as setting the Surface.Color with the average of the
		/// gradient colors.
		/// </summary>
		Gradient,
		/// <summary>
		/// Indicates whether setting Surface.Alpha has any visible effect.
		/// </summary>
		SurfaceAlpha,
		/// <summary>
		/// Indicates whether the alpha channel in surface pixels is used.
		/// </summary>
		PixelAlpha,
		/// <summary>
		/// Indicates whether there is hardware acceleration available for 2D scaling and rotations.
		/// </summary>
		IsHardwareAccelerated,
		/// <summary>
		/// Indicates whether or not vertex/pixel shaders are supported.
		/// </summary>
		CustomShaders,
		/// <summary>
		/// Indicates whether the driver can create a bitmap font from an operating
		/// system font.
		/// </summary>
		CanCreateBitmapFont,
	}

	/// <summary>
	/// Caps values which should return floating point values.
	/// </summary>
	public enum DisplayDoubleCaps
	{
		/// <summary>
		/// Indicates the aspect ratio (width / height) of the main monitor.
		/// </summary>
		AspectRatio,
	}
}
