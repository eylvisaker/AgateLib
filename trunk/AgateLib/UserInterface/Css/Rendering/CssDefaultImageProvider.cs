using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering
{
	public class CssDefaultImageProvider : ICssImageProvider
	{
		Dictionary<string, Surface> mSurfaces = new Dictionary<string, Surface>();

		public Surface GetImage(string file)
		{
			if (file.StartsWith("url(") && file.EndsWith(")"))
			{
				file = file.Substring(4, file.Length - 5);

				if (mSurfaces.ContainsKey(file) == false)
					mSurfaces.Add(file, new Surface(file));

				return mSurfaces[file];
			}

			throw new System.IO.FileNotFoundException(file);
		}
	}
}
