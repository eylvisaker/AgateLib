//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Fulfillment
{
	public class WidgetFactory : IWidgetFactory
	{
		List<IWidgetActivator> activators = new List<IWidgetActivator>();

		class ActivationDefaults
		{
			public string Name { get; set; }
			public string Type { get; set; }
		}

		public WidgetFactory(IWidgetActivator defaultActivator)
		{
			activators.Add(defaultActivator);
		}

		public IWidgetActivator DefaultActivator
		{
			get { return activators.First(); }
			set
			{
				Require.ArgumentNotNull(value, nameof(DefaultActivator));
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

		/// <summary>
		/// Realizes the widgets in the given facet model.
		/// </summary>
		/// <param name="facetModel">The facet model to build.</param>
		/// <param name="widgetCreated">A callback method that will be called for each widget created.</param>
		/// <returns></returns>
		public IEnumerable<Widget> RealizeFacetModel(FacetModel facetModel, Action<string, Widget> widgetCreated)
		{
			Require.ArgumentNotNull(facetModel, nameof(facetModel));
			Require.ArgumentNotNull(widgetCreated, nameof(widgetCreated));

			return RealizeEachWidget(facetModel, widgetCreated).ToList();
		}

		private IEnumerable<Widget> RealizeEachWidget(
			IEnumerable<WidgetProperties> facetModel,
			Action<string, Widget> widgetCreated)
		{
			foreach (var uiElement in facetModel)
			{
				Widget widget = RealizeWidget(uiElement, widgetCreated);

				yield return widget;
			}
		}

		private IEnumerable<MenuItem> RealizeMenuItems(MenuItemModelCollection menuItemModels, Action<string, Widget> widgetCreated)
		{
			foreach (var menuItemModel in menuItemModels)
			{
				MenuItem menuItem = new MenuItem();

				var contents = RealizeWidget(menuItemModel, widgetCreated, new ActivationDefaults
				{
					Name = menuItemModel.Name,
					Type = "label"
				});

				menuItem.Children.Add(contents);

				menuItem.Name = menuItemModel.MenuItemName;

				widgetCreated(menuItem.Name, menuItem);

				yield return menuItem;
			}
		}


		/// <summary>
		/// Creates a widget and its children from the specified widget model. The widgetCreated callback is called for the widget.
		/// </summary>
		/// <param name="widgetModel"></param>
		/// <param name="widgetCreated"></param>
		/// <returns></returns>
		private Widget RealizeWidget(WidgetProperties widgetModel, Action<string, Widget> widgetCreated, ActivationDefaults defaults = null)
		{
			for (int i = activators.Count - 1; i >= 0; i--)
			{
				var activator = activators[i];
				var type = widgetModel.Type ?? defaults?.Type;
				var name = widgetModel.Name ?? defaults?.Name;

				Condition.Requires<InvalidOperationException>(!string.IsNullOrWhiteSpace(type),
					$"The widget {name} has an invalid type.");

				if (activator.CanCreate(type) == false)
					continue;

				Widget widget = activator.Create(type);
				widget.Name = name;

				Condition.Requires<InvalidOperationException>(widget != null,
					$"The activator advertised it could create widget of type {type} for {name} but returned null when asked to create it.");

				ApplyProperties(widget, widgetModel, defaults);

				widgetCreated(widget.Name, widget);

				RealizeChildren(widgetModel, widgetCreated, widget);

				return widget;
			}

			throw new InvalidOperationException($"Failed to activate widget {widgetModel.Name} of type {widgetModel.Type}.");
		}

		private void RealizeChildren(WidgetProperties widgetModel, Action<string, Widget> widgetCreated, Widget widget)
		{
			var menu = widget as Menu;
			var container = widget as Container;

			if (menu != null)
			{
				var menuItems = RealizeMenuItems(widgetModel.MenuItems, widgetCreated).ToList();

				menu.Items.AddRange(menuItems);
			}
			if (container != null)
			{
				var children = RealizeEachWidget(widgetModel.Children, widgetCreated).ToList();

				if (children.Any())
				{
					if (container == null)
						throw new InvalidOperationException($"The widget {widgetModel.Name} has children defined but is of type {widgetModel.Type} which is not a container.");

					container.Children.AddRange(children);
				}
			}
		}

		private void ApplyProperties(Widget widget, WidgetProperties model, ActivationDefaults defaults)
		{
			widget.WidgetStyle.WidgetProperties = model;
			widget.Enabled = model.Enabled;
			widget.Name = model.Name ?? defaults?.Name;
			widget.X = model.Position?.X ?? 0;
			widget.Y = model.Position?.Y ?? 0;
			widget.Width = model.Size?.Width ?? 0;
			widget.Height = model.Size?.Height ?? 0;

			if (model.Overflow == Overflow.Scroll)
			{
				widget.WidgetStyle.View.AllowScroll = ScrollAxes.Vertical;
			}

			ApplyReflectionProperty(widget, "Text", model.Text);
		}

		private void ApplyReflectionProperty(Widget widget, string propertyName, object value)
		{
			if (value == null)
				return;

			var type = widget.GetType();
			var property = type.GetRuntimeProperty(propertyName);
			property.SetMethod.Invoke(widget, new[] { value });
		}
	}
}
