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
using System.Linq;
using System.Text;
using AgateLib.Geometry;

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
		public AgateLib.DisplayLib.Shaders.ShaderLanguage ShaderLanguage
		{
			get 
			{ 
				var shad = Display.Impl.SupportedShaderLanguages.ToList(); 
			
				if (shad.Count == 0)
					return Shaders.ShaderLanguage.None;
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

		public Size ScreenResolution
		{
			get { return Display.Impl.CapsSize(DisplaySizeCaps.NativeScreenResolution); }
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
}
