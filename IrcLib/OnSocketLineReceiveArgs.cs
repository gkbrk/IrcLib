using System;

namespace IrcLib
{
	public class OnSocketLineReceiveArgs : EventArgs
	{
		public string Line;
		public OnSocketLineReceiveArgs (string line)
		{
			Line = line;
		}
	}
}

