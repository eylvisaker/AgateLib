using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources.Managers.UserInterface
{
	/// <summary>
	/// Inspects an object's type and builds a map of property setters for that object 
	/// for properties that derive from T.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ITypeInspector<T>
	{
		/// <summary>
		/// Builds the property map for the passed object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		PropertyMap<T> BuildPropertyMap(object obj);
	}
}
