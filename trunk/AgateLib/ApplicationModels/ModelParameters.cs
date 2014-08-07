using AgateLib.ApplicationModels.CoordinateSystems;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public class ModelParameters
	{
		public ModelParameters()
		{
			AssetLocations = new AssetLocations();
			CoordinateSystem = new NaturalCoordinates();

			AutoCreateDisplayWindow = true;
			CreateFullScreenWindow = true;
			ApplicationName = "AgateLib Application";
			VerticalSync = true;
		}
		public ModelParameters(string[] args) : this()
		{
			Arguments = args;
		}

		public string ApplicationName { get; set; }

		public bool AutoCreateDisplayWindow { get; set; }
		public Size DisplayWindowSize { get; set; }
		public bool CreateFullScreenWindow { get; set; }

		public bool VerticalSync { get; set; }

		public string[] Arguments { get; set; }

		public AssetLocations AssetLocations { get; set; }

		/// <summary>
		/// Gets or sets the object which determines how to set the coordinate
		/// system for the automatically created display window. If 
		/// AutoCreateDisplayWindow is false, this property is not used.
		/// </summary>
		public ICoordinateSystemCreator CoordinateSystem { get; set; }
	}
}
