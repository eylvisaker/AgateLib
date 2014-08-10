using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering.Transitions
{
	public interface IWidgetTransition
	{
		CssStyle Style { get; set; }

		bool Active { get; }

		void Update(double deltaTime);

		bool NeedTransition();

		WidgetAnimator Animator { get; set; }

		void Initialize();
	}
}
