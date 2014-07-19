using System;

namespace AgateLib
{
	public interface ICommandProcessor
	{
		ConsoleDictionary Commands { get; }
		event DescribeCommandHandler DescribeCommand;
		void ExecuteCommand(string[] tokens);
	}
}
