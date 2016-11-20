using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Resources.DataModel
{
	public class ResourceDataModel
	{
		public FontModelCollection Fonts { get; set; } = new FontModelCollection();

		public ThemeModelCollection Themes { get; set; } = new ThemeModelCollection();

		public FacetModelCollection Facets { get; set; } = new FacetModelCollection();

		public List<string> FontSources { get; set; } = new List<string>();

		public List<string> ThemeSources { get; set; } = new List<string>();

		public List<string> FacetSources { get; set; } = new List<string>();
	}
}
