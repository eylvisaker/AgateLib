using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	public static class ConsoleTokenizer
	{
		public static string[] Tokenize(string command)
		{
			// regular expression obtained from 
			// http://stackoverflow.com/questions/554013/regular-expression-to-split-on-spaces-unless-in-quotes
			//
			var regexMatches = Regex.Matches(command, @"((""((?<token>.*?)(?<!\\)"")|(?<token>[\w]+))(\s)*)");

			string[] tokens = (from Match m in regexMatches
							   where m.Groups["token"].Success
							   select m.Groups["token"].Value).ToArray();

			return tokens;
		}
	}
}