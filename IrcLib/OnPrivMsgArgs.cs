using System;

namespace IrcLib
{
	public class OnPrivMsgArgs : EventArgs
	{
		public string From;
		public string Message;
		public OnPrivMsgArgs (string from_who, string message)
		{
			From = from_who;
			Message = message;
		}
	}
}

