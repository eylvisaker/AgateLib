using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Venus.LayoutModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public class WidgetFactory : IWidgetFactory
	{
		List<IWidgetActivator> activators = new List<IWidgetActivator>();

		public WidgetFactory(IWidgetActivator defaultActivator)
		{
			activators.Add(defaultActivator);
		}

		public IWidgetActivator DefaultActivator
		{
			get { return activators.First(); }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(DefaultActivator));
				activators[0] = value;
			}
		}

		public void AddActivator(IWidgetActivator activator)
		{
			Condition.Requires<InvalidOperationException>(activators.Contains(activator) == false,
				"Cannot add an activator if it already exists in the factory");

			activators.Add(activator);
		}

		public void RemoveActivator(IWidgetActivator activator)
		{
			Condition.Requires<InvalidOperationException>(activator != DefaultActivator,
				"Cannot remove the default activator.");

			activators.Remove(activator);
		}

		public void RealizeFacetModel(FacetModel facetModel, Action<string, Widget> widgetCreated)
		{
			Condition.RequireArgumentNotNull(facetModel, nameof(facetModel));
			Condition.RequireArgumentNotNull(widgetCreated, nameof(widgetCreated));
			
			foreach(var uiElement in facetModel)
			{
				Widget widget = RealizeWidget(uiElement.Value);
				widget.Name = uiElement.Key;

				widgetCreated(widget.Name, widget);
			}
		}

		private Widget RealizeWidget(WidgetProperties value)
		{
			for (int i = activators.Count - 1; i >= 0; i--)
			{
				var activator = activators[i];

				if (activator.CanCreate(value.Type))
				{
					Widget result = activator.Create(value.Type);

					Condition.Requires<InvalidOperationException>(result != null,
						$"The activator advertised it could create widget of type {value.Type} for {value.Name} but returned null when asked to create it.");

					return result;
				}
			}

			throw new InvalidOperationException($"Failed to activate widget {value.Name} of type {value.Type}.");
		}
		
	}
}
