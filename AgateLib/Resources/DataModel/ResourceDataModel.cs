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
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;

namespace AgateLib.Resources.DataModel
{
	public class ResourceDataModel
	{
		/// <summary>
		/// The IReadFileProvider object that is used to load resources.
		/// </summary>
		internal IReadFileProvider FileProvider { get; set; }

		/// <summary>
		/// The root path of files referred to from this file.
		/// </summary>
		internal string Path { get; private set; }

		public FontResourceCollection Fonts { get; set; } = new FontResourceCollection();

		public ThemeModelCollection Themes { get; set; } = new ThemeModelCollection();

		public FacetModelCollection Facets { get; set; } = new FacetModelCollection();

		public SpriteResourceCollection Sprites { get; set; } = new SpriteResourceCollection();

		public SurfaceModelCollection Images { get; set; } = new SurfaceModelCollection();

		public List<string> FontSources { get; set; } = new List<string>();

		public List<string> ThemeSources { get; set; } = new List<string>();

		public List<string> FacetSources { get; set; } = new List<string>();

		public List<string> SurfaceSources { get; set; } = new List<string>();

		internal void Validate()
		{
			Facets.Validate();
		}

		internal void ApplyPath(string path)
		{
			Path = path;

			Fonts.ApplyPath(path);
		}
	}
}
