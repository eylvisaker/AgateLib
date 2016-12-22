using System;

namespace AgateLib.Diagnostics
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class JoinArgsAttribute : Attribute
	{
	}
}