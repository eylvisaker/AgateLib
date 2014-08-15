using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AgateLib.Resources.DC
{
	public class FontResource : AgateResource
	{
		public FontResource()
		{
			FontSurfaces = new List<FontSurfaceResource>();
		}
		public FontResource(string name, params FontSurfaceResource[] contents )
			:this()
		{
			FontName = name;

			foreach (var c in contents)
				FontSurfaces.Add(c);
		}
		
		public string FontName { get; set; }
		public IList<FontSurfaceResource> FontSurfaces { get; set; }
	}
}
