﻿//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		internal string Path { get; set; }


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
	}
}
