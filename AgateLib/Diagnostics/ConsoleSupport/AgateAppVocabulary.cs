using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	class AgateAppVocabulary : IVocabulary
	{
		public string Namespace => "application";

		[ConsoleCommand("Changes the speed of the game clock. A value of 1 is full speed, 0.5 is half speed, etc.")]
		private void Speed(double amount)
		{
			AgateApp.GameClock.ClockSpeed = amount;
		}
	}
}
