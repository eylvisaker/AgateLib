using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Hierarchy
{
	class ContainerInitializer
	{
		private static Dictionary<string, Type> widgetTypeMap;
		static ContainerInitializer()
		{
			widgetTypeMap = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

			widgetTypeMap.Add("Label", typeof(Label));
			widgetTypeMap.Add("Panel", typeof(Panel));
			widgetTypeMap.Add("Window", typeof(Window));
		}

		private Dictionary<string, Widget> widgets = new Dictionary<string, Widget>();
		private IEnumerable<LayoutModel> models;

		public ContainerInitializer(IEnumerable<LayoutModel> models)
		{
			Condition.RequireArgumentNotNull(models, nameof(models));

			this.models = models;
		}

		public IEnumerable<Widget> Initialize(IUserInterfaceContainer ui)
		{
			BuildWidgets();

			var type = ui.GetType();

			var fields = type.GetRuntimeFields();
			var baseType = typeof(Widget).GetTypeInfo();

			foreach (var field in fields)
			{
				if (baseType.IsAssignableFrom(field.FieldType.GetTypeInfo()) == false)
					continue;

				var name = GetWidgetName(field);

				var widget = GetWidget(name);

				if (widget != null)
				{
					field.SetValue(ui, widget);
				}
			}

			return widgets.Values;
		}

		private void BuildWidgets()
		{
			foreach (var model in models)
			{
				foreach (var widgetProperties in model.Widgets)
				{
					RealizeWidgetModel(widgetProperties);
				}
			}
		}

		private Widget RealizeWidgetModel(WidgetProperties widgetProperties)
		{
			var widget = GetOrCreateWidget(widgetProperties.Name, TypeOf(widgetProperties.Type));

			ApplyWidgetProperties(widget, widgetProperties);

			return widget;
		}

		private void ApplyWidgetProperties(Widget widget, WidgetProperties widgetProperties)
		{
			ApplyProperty(widget, w => w.Name, widgetProperties.Name);
			ApplyProperty(widget, w => w.Enabled, widgetProperties.Enabled);
			ApplyProperty(widget, "Text", widgetProperties.Text);

			if (widgetProperties.Children.Count > 0)
			{
				BuildWidgetChildren(widget as Container, widgetProperties);
			}
		}

		private void BuildWidgetChildren(Container widget, WidgetProperties widgetProperties)
		{
			if (widget == null)
				throw new InvalidOperationException("Cannot apply children to a widget which is not a container.");

			foreach (var childProperties in widgetProperties.Children)
			{
				var child = RealizeWidgetModel(childProperties);

				widget.Children.Add(child);
			}
		}

		private void ApplyProperty(Widget widget, string propertyName, string value)
		{
			if (value == null)
				return;

			var type = widget.GetType();
			var property = type.GetRuntimeProperty(propertyName);
			property.SetMethod.Invoke(widget, new object[] { value });
		}

		private void ApplyProperty<T>(Widget widget, Expression<Func<Widget, T>> property, T value) where T : class
		{
			if (value == null)
				return;

			var memberExp = (MemberExpression)property.Body;
			var propInfo = (PropertyInfo)memberExp.Member;
			MethodInfo setter = propInfo.SetMethod;

			setter.Invoke(widget, new object[] { value });
		}

		private void ApplyProperty<T>(Widget widget, Expression<Func<Widget, T>> property, T? value) where T : struct
		{
			if (value == null)
				return;

			var memberExp = (MemberExpression)property.Body;
			var propInfo = (PropertyInfo)memberExp.Member;
			MethodInfo setter = propInfo.SetMethod;

			setter.Invoke(widget, new object[] { value.Value });
		}

		private Type TypeOf(string type)
		{
			return widgetTypeMap[type];
		}

		private Widget GetOrCreateWidget(string name, Type type)
		{
			if (widgets.ContainsKey(name) == false)
			{
				Widget widget = (Widget)Activator.CreateInstance(type);

				widgets[name] = widget;
			}

			var result = widgets[name];

			if (result.GetType() != type)
				throw new InvalidOperationException("Widget had wrong type");

			return result;
		}

		private Widget GetWidget(string name) => widgets.ContainsKey(name) == false ? null : widgets[name];

		private string GetWidgetName(FieldInfo field)
		{
			return field.Name;
		}
	}
}
