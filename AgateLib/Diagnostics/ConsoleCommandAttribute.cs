using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Use this attribute on public methods of a ICommandVocabulary object to signify that
	/// those methods are commands the user can enter.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ConsoleCommandAttribute : Attribute
	{
		/// <summary>
		/// Constructs a ConsoleMethodAttribute
		/// </summary>
		/// <param name="name">The name of the command the user types to execute this method.</param>
		public ConsoleCommandAttribute(string name = null)
		{
			Name = name;
		}

		/// <summary>
		/// The name of the command the user types to execute this method.
		/// </summary>
		public string Name { get; set; }
	}
}
