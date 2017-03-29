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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.IO;
using AgateLib.Resources.DataModel;
using AgateLib.Resources.Managers.UserInterface;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Resources.Managers.Display
{
	public class DisplayResourceManager : IDisplayResourceManager
	{
		private readonly ITypeInspector<IFont> fontInspector = new TypeInspector<IFont>();
		private readonly ITypeInspector<ISprite> spriteInspector = new TypeInspector<ISprite>();
		private readonly ITypeInspector<ISurface> surfaceInspector = new TypeInspector<ISurface>();

		private ResourceDataModel data;

		private Dictionary<string, Font> fonts = new Dictionary<string, Font>();
		private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
		private Dictionary<string, Surface> surfaces = new Dictionary<string, Surface>();

		public DisplayResourceManager(ResourceDataModel data)
		{
			this.data = data;
		}

		public void Dispose()
		{
			foreach (var item in fonts.Values.Cast<IDisposable>()
								.Concat(sprites.Values)
								.Concat(surfaces.Values))
			{
				item.Dispose();
			}
		}

		/// <summary>
		/// Initializes the resources in the resource container.
		/// </summary>
		/// <param name="container"></param>
		public void InitializeContainer(object container)
		{
			var fontPropertyMap = fontInspector.BuildPropertyMap(container);

			foreach (var propertyName in fontPropertyMap.Keys)
			{
				var dest = fontPropertyMap[propertyName];
				var font = FindFont(propertyName);

				dest.Assign(font ?? Font.AgateSans);
			}

			var spritePropertyMap = spriteInspector.BuildPropertyMap(container);

			foreach (var propertyName in spritePropertyMap.Keys)
			{
				var dest = spritePropertyMap[propertyName];

				dest.Assign(GetSprite(propertyName));
			}

			var surfacePropertyMap = surfaceInspector.BuildPropertyMap(container);

			foreach (var propertyName in surfacePropertyMap.Keys)
			{
				var dest = surfacePropertyMap[propertyName];

				dest.Assign(GetSurface(propertyName));
			}
		}

		/// <summary>
		/// Searches for a font. Returns null if not found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Font FindFont(string name)
		{
			if (fonts.ContainsKey(name))
				return fonts[name];
			if (data.Fonts.ContainsKey(name) == false)
				return null;

			var fontModel = data.Fonts[name];

			Font result = new Font(name);

			foreach (var fontSurfaceModel in fontModel)
			{
				var image = fontSurfaceModel.Image;

				var surface = GetSurface(image);

				FontMetrics metrics = new FontMetrics();
				foreach (var glyph in fontSurfaceModel.Metrics)
				{
					metrics.Add((char)glyph.Key, glyph.Value);
				}

				var impl = new BitmapFontImpl(surface, metrics, name);
				var fontSurface = new FontSurface(impl);

				result.Core.AddFontSurface(new FontSettings(fontSurfaceModel.Size, fontSurfaceModel.Style), fontSurface);
			}

			fonts[name] = result;

			return fonts[name];
		}

		/// <summary>
		/// Returns the font matching the name or throws an exception.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Font GetFont(string name)
		{
			if (data.Fonts.ContainsKey(name) == false)
				throw new AgateResourceException($"Could not find font named {name}");

			var result = FindFont(name);

			if (result == null)
				throw new AgateResourceException($"Could not find font named {name}");

			return result;
		}

		private ISurface GetSurface(string image)
		{
			if (surfaces.ContainsKey(image) == false)
			{
				if (data.Images.ContainsKey(image) == false)
				{
					using (var file = data.FileProvider.OpenRead(image))
					{
						surfaces[image] = new Surface(file);
					}
				}
				else
				{
					var dataModel = data.Images[image];

					using (var file = data.FileProvider.OpenRead(dataModel.Image))
					{
						surfaces[image] = new Surface(file);
					}
				}
			}

			return surfaces[image];
		}

		public ISprite GetSprite(string name)
		{
			if (data.Sprites.ContainsKey(name) == false)
				throw new AgateResourceException($"Could not find sprite named {name}");

			var dataModel = data.Sprites[name];

			if (sprites.ContainsKey(name) == false)
			{
				Sprite result = null;

				var spriteImage = dataModel.Image;

				foreach (var frame in dataModel.Frames)
				{
					var frameImage = frame.Image ?? spriteImage;
					var surface = GetSurface(frameImage);
					Size frameSize = surface.SurfaceSize;

					if (frame.SourceRect != null)
					{
						frameSize = frame.SourceRect.Value.Size;
					}

					if (result == null)
						result = new Sprite(frameSize);

					var spriteFrame = new SpriteFrame(surface);
					spriteFrame.SpriteSize = result.SpriteSize;

					if (frame.SourceRect != null)
					{
						spriteFrame.SourceRect = frame.SourceRect.Value;
					}

					result.Frames.Add(spriteFrame);
				}

				if (result == null)
					throw new AgateResourceException($"Sprite {name} does not have any frames defined.");

				if (dataModel.Animation != null)
				{
					result.AnimationType = dataModel.Animation.Type;
				}

				sprites[name] = result;
			}

			return sprites[name];
		}
	}
}
