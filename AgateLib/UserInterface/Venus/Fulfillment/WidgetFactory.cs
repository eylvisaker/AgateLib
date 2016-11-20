using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

		public IEnumerable<Widget> RealizeFacetModel(FacetModel facetModel, Action<string, Widget> widgetCreated)
		{
			Condition.RequireArgumentNotNull(facetModel, nameof(facetModel));
			Condition.RequireArgumentNotNull(widgetCreated, nameof(widgetCreated));

			return RealizeEachWidget(facetModel, widgetCreated).ToList();
		}

		private IEnumerable<Widget> RealizeEachWidget(IEnumerable<KeyValuePair<string, WidgetProperties>> facetModel,
			Action<string, Widget> widgetCreated)
		{
			foreach (var uiElement in facetModel)
			{
				Widget widget = RealizeWidget(uiElement.Value, widgetCreated);
				widget.Name = uiElement.Key;

				widgetCreated(widget.Name, widget);

				yield return widget;
			}
		}

		private Widget RealizeWidget(WidgetProperties widgetModel, Action<string, Widget> widgetCreated)
		{
			for (int i = activators.Count - 1; i >= 0; i--)
			{
				var activator = activators[i];

				if (activator.CanCreate(widgetModel.Type) == false)
					continue;

				Widget widget = activator.Create(widgetModel.Type);

				ApplyProperties(widget, widgetModel);

				Condition.Requires<InvalidOperationException>(widget != null,
					$"The activator advertised it could create widget of type {widgetModel.Type} for {widgetModel.Name} but returned null when asked to create it.");

				var children = RealizeEachWidget(widgetModel.Children, widgetCreated).ToList();

				if (children.Any())
				{
					var container = widget as Container;

					if (container == null)
						throw new InvalidOperationException($"The widget {widgetModel.Name} has children defined but is of type {widgetModel.Type} which is not a container.");

					container.Children.AddRange(children);
				}

				return widget;
			}

			throw new InvalidOperationException($"Failed to activate widget {widgetModel.Name} of type {widgetModel.Type}.");
		}

		private void ApplyProperties(Widget widget, WidgetProperties model)
		{
			widget.Enabled = model.Enabled;
			widget.Name = model.Name;
			widget.X = model.X;
			widget.Y = model.Y;
			widget.Width = model.Width;
			widget.Height = model.Height;

			ApplyReflectionProperty(widget, "Text", model.Text);
		}

		private void ApplyReflectionProperty(Widget widget, string propertyName, object value)
		{
			if (value == null)
				return;

			var type = widget.GetType();
			var property = type.GetRuntimeProperty(propertyName);
			property.SetMethod.Invoke(widget, new [] { value });
		}
	}
}
