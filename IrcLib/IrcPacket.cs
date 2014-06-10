using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace IrcLib
{
	public class IrcPacket
	{
		public string Prefix;
		public string Command;
		public string[] Arguments = new string[]{};
		//public List<string> Arguments = new List<string>{};
		public IrcPacket ()
		{
		}

		public void Parse2(string message)
		{
			string trailing = null;
			Prefix = Command = String.Empty;
			Arguments = new string[] { };

			Regex parsingRegex = new Regex(@"^(:(?<prefix>\S+) )?(?<command>\S+)( (?!:)(?<params>.+?))?( :(?<trail>.+))?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
			Match messageMatch = parsingRegex.Match(message);

			if (messageMatch.Success)
			{
				Prefix = messageMatch.Groups["prefix"].Value;
				Command = messageMatch.Groups["command"].Value;
				Arguments = messageMatch.Groups["params"].Value.Split(' ');
				trailing = messageMatch.Groups["trail"].Value;

				if (!String.IsNullOrEmpty(trailing))
					Arguments = Arguments.Concat(new string[] { trailing }).ToArray();
			}
		}
	}
}