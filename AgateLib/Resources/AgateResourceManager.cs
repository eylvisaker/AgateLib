using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;
using AgateLib.Quality;
using AgateLib.Resources.DataModel;
using AgateLib.Resources.Managers.Display;
using AgateLib.Resources.Managers.UserInterface;
using AgateLib.UserInterface;

namespace AgateLib.Resources
{
	public class AgateResourceManager : IDisposable
	{
		private readonly ResourceDataModel data;
		private readonly IReadFileProvider imageFileProvider;

		private IUserInterfaceResourceManager uiResourceManager;
		private IDisplayResourceManager displayResourceManager;

		bool disposed = false;

		public AgateResourceManager(string filename) : this(new ResourceDataLoader().Load(filename))
		{ }

		public AgateResourceManager(ResourceDataModel data) : this(data, Assets.Images, Assets.UserInterfaceAssets)
		{
		}

		public AgateResourceManager(ResourceDataModel dataModel, IReadFileProvider imageFileProvider, IReadFileProvider fontFileProvider)
		{
			this.data = dataModel;
			this.imageFileProvider = imageFileProvider;

			Display = new DisplayResourceManager(data, imageFileProvider, fontFileProvider);
			UserInterface = new UserInterfaceResourceManager(data, Display);
		}

		public void Dispose()
		{
			displayResourceManager.Dispose();
			uiResourceManager.Dispose();

			disposed = true;
		}

		public ResourceDataModel Data { get { return data; } }

		public IDisplayResourceManager Display
		{
			get
			{
				if (disposed)
					throw new ObjectDisposedException(nameof(AgateResourceManager));

				return displayResourceManager;
			}
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(Display));
				displayResourceManager = value;
			}
		}

		public IUserInterfaceResourceManager UserInterface
		{
			get
			{
				if (disposed)
					throw new ObjectDisposedException(nameof(AgateResourceManager));

				return uiResourceManager;
			}
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(UserInterface));
				uiResourceManager = value;
			}
		}

		public IEnumerable<string> Sprites
		{
			get
			{
				foreach (var sprite in data.Sprites)
					yield return sprite.Key;
			}
		}

		/// <summary>
		/// Initializes an object by setting all its fields that correspond to resource types
		/// with matching resources.
		/// </summary>
		/// <remarks>If the object is a user interface facet, this will initialize the facet.</remarks>
		/// <param name="container"></param>
		public void InitializeContainer(object container)
		{
			if (disposed)
				throw new ObjectDisposedException(nameof(AgateResourceManager));

			var facet = container as IUserInterfaceFacet;

			if (facet != null)
				UserInterface.InitializeFacet(facet);

			Display.InitializeContainer(container);
		}
	}
}
