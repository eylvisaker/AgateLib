using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			get { return Display.Impl.Supports(DisplayBoolCaps.FullScreen); }
		}
		/// <summary>
		/// Indicates whether or not the screen resolution can be changed.
		/// If the Display driver supports full screen but not mode switching,
		/// then a DisplayWindow which is created with as a full screen window
		/// cannot change resolutions after it is initially set.
		/// </summary>
		public bool SupportsFullScreenModeSwitching
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.FullScreenModeSwitching); }
		}
		/// <summary>
		/// Indicates whether setting Surface.SetScale has any visible effect.
		/// </summary>
		public bool SupportsScaling
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.Scaling); }
		}
		/// <summary>
		///  Indicates whether setting Surface.RotationAngle has any visible effect.
		/// </summary>
		public bool SupportsRotation
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.Rotation); }
		}
		/// <summary>
		/// Indicates whether setting Surface.Color has any visible effect.
		/// </summary>
		public bool SupportsColor
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.Color); }
		}
		/// <summary>
		/// Indicates whether Surface gradients are supported.  If not, then setting Surface.ColorGradient
		/// color of a surface is the same as setting the Surface.Color with the average of the
		/// gradient colors.
		/// </summary>
		public bool SupportsGradient
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.Gradient); }
		}
		/// <summary>
		/// Indicates whether setting Surface.Alpha has any visible effect.
		/// </summary>
		public bool SupportsSurfaceAlpha
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.SurfaceAlpha); }
		}
		/// <summary>
		/// Indicates whether the alpha channel in surface pixels is used.
		/// </summary>
		public bool SupportsPixelAlpha
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.PixelAlpha); }
		}
		/// <summary>
		/// Indicates whether or not lighting is supported.
		/// </summary>
		[Obsolete]
		public bool SupportsLighting
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.Lighting); }
		}
		/// <summary>
		/// Indicates the maximum number of lights which can be used.
		/// </summary>
		[Obsolete]
		public int MaxLights
		{
			get { return 8; }
		}
		/// <summary>
		/// Indicates whether there is hardware acceleration available for 2D and 3D drawing.
		/// </summary>
		public bool IsHardwareAccelerated
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.IsHardwareAccelerated); }
		}
		/// <summary>
		/// Indicates whether or not vertex/pixel shaders are supported.
		/// </summary>
		public bool SupportsShaders
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.Shaders); }
		}
		/// <summary>
		/// Indicates which shader language is supported.
		/// </summary>
		public AgateLib.DisplayLib.Shaders.ShaderLanguage ShaderLanguage
		{
			get { return AgateLib.DisplayLib.Shaders.ShaderLanguage.Hlsl; }
		}
		/// <summary>
		/// Indicates whether the driver can create a bitmap font from an operating
		/// system font.
		/// </summary>
		public bool CanCreateBitmapFont
		{
			get { return Display.Impl.Supports(DisplayBoolCaps.CanCreateBitmapFont); }
		}
	}

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
		/// Indicates whether or not lighting is supported.
		/// </summary>
		[Obsolete]
		Lighting,
		/// <summary>
		/// Indicates whether there is hardware acceleration available for 2D and 3D drawing.
		/// </summary>
		IsHardwareAccelerated,
		/// <summary>
		/// Indicates whether or not vertex/pixel shaders are supported.
		/// </summary>
		Shaders,
		/// <summary>
		/// Indicates whether the driver can create a bitmap font from an operating
		/// system font.
		/// </summary>
		CanCreateBitmapFont,
	}
}
