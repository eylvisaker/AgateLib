using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.IO;
using AgateLib.Resources.DataModel;
using AgateLib.Resources.Managers.UserInterface;
using AgateLib.DisplayLib.Sprites;

namespace AgateLib.Resources.Managers.Display
{
	public class DisplayResourceManager : IDisplayResourceManager
	{
		private readonly IReadFileProvider imageFileProvider;
		private readonly IReadFileProvider fontFileProvider;
		private readonly ITypeInspector<IFont> fontInspector = new TypeInspector<IFont>();
		private readonly ITypeInspector<ISprite> spriteInspector = new TypeInspector<ISprite>();
		private readonly ITypeInspector<ISurface> surfaceInspector = new TypeInspector<ISurface>();

		private ResourceDataModel data;

		private Dictionary<string, Font> fonts = new Dictionary<string, Font>();
		private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
		private Dictionary<string, Surface> surfaces = new Dictionary<string, Surface>();

		public DisplayResourceManager(ResourceDataModel data, IReadFileProvider imageFileProvider, IReadFileProvider fontFileProvider)
		{
			this.data = data;
			this.imageFileProvider = imageFileProvider;
			this.fontFileProvider = fontFileProvider;
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

		public IFont FindFont(string name)
		{
			if (fonts.ContainsKey(name))
				return fonts[name];
			if (data.Fonts.ContainsKey(name) == false)
				return null;

			var fontModel = data.Fonts[name];

			Font result = new Font(name);

			foreach (var fontSurfaceModel in fontModel)
			{
				var surface = GetSurface(fontSurfaceModel.Image, fontFileProvider);

				FontMetrics metrics = new FontMetrics();
				foreach (var glyph in fontSurfaceModel.Metrics)
				{
					metrics.Add((char)glyph.Key, glyph.Value);
				}

				var impl = new BitmapFontImpl(surface, metrics, name);
				var fontSurface = new FontSurface(impl);

				result.AddFont(fontSurface, fontSurfaceModel.Size, fontSurfaceModel.Style);
			}

			fonts[name] = result;

			return fonts[name];
		}

		public IFont GetFont(string name)
		{
			if (data.Fonts.ContainsKey(name) == false)
				throw new AgateResourceException($"Could not find font named {name}");

			var result = FindFont(name);

			if (result == null)
				throw new AgateResourceException($"Could not find font named {name}");

			return result;
		}

		private ISurface GetSurface(string image, IReadFileProvider fileProvider = null)
		{
			fileProvider = fileProvider ?? imageFileProvider;

			if (surfaces.ContainsKey(image) == false)
			{
				if (data.Images.ContainsKey(image) == false)
				{
					using (var file = fileProvider.OpenRead(image))
					{
						surfaces[image] = new Surface(file);
					}
				}
				else
				{
					var dataModel = data.Images[image];

					using (var file = fileProvider.OpenRead(dataModel.Image))
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
					var surface = GetSurface(frameImage, imageFileProvider);
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
