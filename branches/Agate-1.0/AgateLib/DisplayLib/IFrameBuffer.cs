using AgateLib.Geometry;
using System;
namespace AgateLib.DisplayLib
{
	/// <summary>
	/// This interface is implemented by the FrameBuffer class. Its main purpose is
	/// to allow you to create a fake object implementing the interface in order to 
	/// write unit tests for drawing code.
	/// </summary>
	public interface IFrameBuffer
	{
		/// <summary>
		/// Height of the IFrameBuffer object.
		/// </summary>
		int Height { get; }
		/// <summary>
		/// Width of the IFrameBuffer object.
		/// </summary>
		int Width { get; }
		/// <summary>
		/// Size of the IFrameBuffer object. Should equal new Size(Width, Height).
		/// </summary>
		Size Size { get; }
	}
}
