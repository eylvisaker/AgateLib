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
using System.IO;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.DefaultAssets;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.IO;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Drivers
{
	/// <summary>
	///     Interface that the display factory must implement.
	/// </summary>
	public interface IDisplayFactory
	{
		DisplayImpl DisplayCore { get; }

		/// <summary>
		///     Creates a DisplayWindowImpl derived object.
		/// </summary>
		/// <param name="windowParams"></param>
		/// <returns></returns>
		DisplayWindowImpl CreateDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams);

		/// <summary>
		///     Creates a SurfaceImpl derived object.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		SurfaceImpl CreateSurface(IReadFileProvider provider, string filename);

		/// <summary>
		///     Creates a SurfaceImpl derived object from a stream containing
		///     the file contents.
		/// </summary>
		/// <param name="fileStream"></param>
		/// <returns></returns>
		SurfaceImpl CreateSurface(Stream fileStream);

		/// <summary>
		///     Creates a SurfaceImpl derived object that is blank of the specified size.
		/// </summary>
		SurfaceImpl CreateSurface(Size surfaceSize);

		/// <summary>
		///     Creates a Surface implementation from the specified pixel buffer.
		/// </summary>
		/// <param name="pixels"></param>
		/// <returns></returns>
		SurfaceImpl CreateSurface(PixelBuffer pixels);

		/// <summary>
		///     Creates a FrameBufferImpl object of the specified size.
		/// </summary>
		/// <param name="size"></param>
		FrameBufferImpl CreateFrameBuffer(Size size);

		/// <summary>
		///     Initializes the default resources.
		/// </summary>
		/// <param name="res"></param>
		/// <returns></returns>
		Task InitializeDefaultResourcesAsync(DefaultResources res);
	}
}