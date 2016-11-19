using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.UserInterface.Venus.LayoutModel;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Fulfillment
{
	public class WidgetBuilder
	{
		private readonly ITypeResolver typeResolver;
		private readonly IEnumerable<WidgetLayoutModel> models;

		private readonly Dictionary<string, Widget> widgets = new Dictionary<string, Widget>();

		public WidgetBuilder(ITypeResolver typeResolver, IEnumerable<WidgetLayoutModel> models)
		{
			Condition.RequireArgumentNotNull(typeResolver, nameof(typeResolver));
			Condition.RequireArgumentNotNull(models, nameof(models));

			this.typeResolver = typeResolver;
			this.models = models.ToList();
		}

		public IReadOnlyDictionary<string, Widget> Widgets => widgets;

		public void BuildWidgets()
		{
			foreach (var model in models)
			{
				foreach (var widgetProperties in model.Widgets)
				{
					RealizeWidgetModel(widgetProperties);
				}
			}
		}

		public Widget WidgetOrDefault(string name) => widgets.ContainsKey(name) == false ? null : widgets[name];

		private Widget RealizeWidgetModel(WidgetProperties widgetProperties)
		{
			var widget = GetOrCreateWidget(widgetProperties.Name, widgetProperties.Type);

			ApplyWidgetProperties(widget, widgetProperties);

			return widget;
		}


		private void ApplyWidgetProperties(Widget widget, WidgetProperties widgetProperties)
		{
			widget.Enabled = widgetProperties.Enabled;

			ApplyProperty(widget, w => w.Name, widgetProperties.Name);
			ApplyProperty(widget, w => w.X, widgetProperties.X);
			ApplyProperty(widget, w => w.Y, widgetProperties.Y);
			ApplyProperty(widget, w => w.Width, widgetProperties.Width);
			ApplyProperty(widget, w => w.Height, widgetProperties.Height);
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

			foreach (var childProperties in widgetProperties.Children.Values)
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

		private Widget GetOrCreateWidget(string name, string type)
		{
			if (widgets.ContainsKey(name) == false)
			{
				Widget widget = (Widget)Activator.CreateInstance(typeResolver.Resolve(type));

				widgets[name] = widget;
			}

			var result = widgets[name];

			return result;
		}

	}
}
