using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets.Gestures
{
	interface IGestureController
	{
		Gesture GestureData { get; set; }

		void Update();


		void OnBegin();

		void OnComplete();

		void OnTimePass();
	}
}
