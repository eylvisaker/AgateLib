using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Interface which provides contextual information about the display.
	/// </summary>
	public interface IDisplayContext
	{
		/// <summary>
		/// Gets the primitive renderer.
		/// </summary>
		IPrimitiveRenderer Primitives { get; }

		/// <summary>
		/// Gets the size of the display area.
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// Gets the render target of the display context.
		/// </summary>
		FrameBuffer RenderTarget { get; }
	}
}