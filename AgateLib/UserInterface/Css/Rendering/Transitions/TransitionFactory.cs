using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering.Transitions
{
	public static class TransitionFactory
	{
		static Dictionary<CssTransitionType, Type> mTypes = new Dictionary<CssTransitionType, Type>();

		static TransitionFactory()
		{
			RegisterType(CssTransitionType.None, typeof(NullTransition));
			RegisterType(CssTransitionType.Slide, typeof(SlideTransition));
		}

		public static void RegisterType(CssTransitionType transition, Type type)
		{
			mTypes[transition] = type;
		}

		public static IWidgetTransition CreateTransition(CssTransitionType transition)
		{
			return (IWidgetTransition)Activator.CreateInstance(mTypes[transition]);
		}
	}
}
