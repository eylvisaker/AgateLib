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
using AgateLib.IO;
using AgateLib.Quality;
using AgateLib.Resources.DataModel;
using AgateLib.Resources.Managers.Display;
using AgateLib.Resources.Managers.UserInterface;
using AgateLib.UserInterface;

namespace AgateLib.Resources
{
	public class AgateResourceManager : IAgateResourceManager
	{
		private readonly ResourceDataModel data;

		private IUserInterfaceResourceManager uiResourceManager;
		private IDisplayResourceManager displayResourceManager;

		bool disposed = false;

		public AgateResourceManager(string filename) : this(new ResourceDataLoader().Load(filename))
		{ }

		public AgateResourceManager(ResourceDataModel data) : this(data, AgateApp.Assets)
		{
		}

		public AgateResourceManager(ResourceDataModel dataModel, IReadFileProvider fileProvider)
		{
			this.data = dataModel;

			Display = new DisplayResourceManager(data);
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
					return null;

				return displayResourceManager;
			}
			set
			{
				Require.ArgumentNotNull(value, nameof(Display));
				displayResourceManager = value;
			}
		}

		public IUserInterfaceResourceManager UserInterface
		{
			get
			{
				if (disposed)
					return null;

				return uiResourceManager;
			}
			set
			{
				Require.ArgumentNotNull(value, nameof(UserInterface));
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
