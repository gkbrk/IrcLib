using System;

namespace IrcLib
{
	public class OnPublicMessageArgs : EventArgs
	{
		public string User;
		public string Channel;
		public string Message;
		public OnPublicMessageArgs (string user, string channel, string message)
		{
			User = user;
			Channel = channel;
			Message = message;
		}
	}
}

