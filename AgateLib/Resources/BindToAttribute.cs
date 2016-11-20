using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;

namespace AgateLib.Resources
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class BindToAttribute : Attribute
	{
		string name;

		public BindToAttribute(string name)
		{
			Name = name;
		}

		public string Name
		{
			get { return name; }
			set
			{
				Condition.Requires<ArgumentOutOfRangeException>(string.IsNullOrWhiteSpace(name), "BindTo attribute must have a string which is not null or whitespace.");

				name = value;
			}
		}
	}
}
