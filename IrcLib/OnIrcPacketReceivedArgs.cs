using System;

namespace IrcLib
{
	public class OnIrcPacketReceivedArgs : EventArgs
	{
		public IrcPacket Packet;
		public OnIrcPacketReceivedArgs (IrcPacket packet)
		{
			Packet = packet;
		}
	}
}

