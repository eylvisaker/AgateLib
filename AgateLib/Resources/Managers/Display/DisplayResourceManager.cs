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

namespace AgateLib.Resources.Managers.Display
{
	public class DisplayResourceManager : IDisplayResourceManager
	{
		private ITypeInspector<IFont> fontInspector;

		private ResourceDataModel data;

		private Dictionary<string, Font> fonts = new Dictionary<string, Font>();

		public DisplayResourceManager(ResourceDataModel data)
		{
			fontInspector = new TypeInspector<IFont>();

			this.data = data;
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
				var surface = GetSurface(fontSurfaceModel.Image, Assets.UserInterfaceAssets);

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

		private Surface GetSurface(string image, IReadFileProvider fileProvider)
		{
			using (var file = fileProvider.OpenRead(image))
			{
				return new Surface(file);
			}
		}

		public void InitializeContainer(object container)
		{
			var fontPropertyMap = fontInspector.BuildPropertyMap(container);

			foreach (var propertyName in fontPropertyMap.Keys)
			{
				var dest = fontPropertyMap[propertyName];

				dest.Assign(GetFont(propertyName));
			}
		}

	}
}
