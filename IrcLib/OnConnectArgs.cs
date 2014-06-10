using System;

namespace IrcLib
{
	public class OnConnectArgs : EventArgs
	{
		public string Host;
		public int Port;
		public OnConnectArgs (string host, int port)
		{
			Host = host;
			Port = port;
		}
	}
}

