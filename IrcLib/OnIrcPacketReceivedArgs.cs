using System;

namespace IrcLib
{
	public class OnIrcPacketReceivedArgs : EventArgs
	{
		public IrcPacket2 Packet;
		public OnIrcPacketReceivedArgs (IrcPacket2 packet)
		{
			Packet = packet;
		}
	}
}

