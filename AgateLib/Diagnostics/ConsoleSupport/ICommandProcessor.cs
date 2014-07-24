using System;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	public interface ICommandProcessor
	{
		ConsoleDictionary Commands { get; }
		event DescribeCommandHandler DescribeCommand;
		void ExecuteCommand(string[] tokens);
	}
}
