using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Resources.Managers.UserInterface
{
	public interface IFacetInspector<T>
	{
		PropertyMap<T> BuildPropertyMap(IUserInterfaceFacet facet);
	}
}
