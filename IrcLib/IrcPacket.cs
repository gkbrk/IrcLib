using System;
using System.Collections.Generic;

namespace IrcLib
{
	public class IrcPacket
	{
		public string Prefix;
		public string Command;
		public List<string> Arguments = new List<string>();
		public IrcPacket ()
		{
		}

		public void Parse(string packet)
		{
			if(packet.StartsWith(":"))
			{
				Prefix = packet.Substring(1, packet.IndexOf(' ')-1);
				packet = packet.Substring(packet.IndexOf(' '));
			}

			if(packet.Contains(" "))
			{
				if(packet.Contains(" :"))
				{
					string last_argument = packet.Substring(packet.IndexOf(" :")+2);
					packet = packet.Substring(0, packet.IndexOf(" :"));
					foreach(string splitted in packet.Split(' '))
					{
						if(String.IsNullOrEmpty(Command))
						{
							Command = splitted;
						}else
						{
							Arguments.Add(splitted);
						}
					}
					Arguments.Add(last_argument);
				}else
				{
					foreach(string splitted in packet.Split(' '))
					{
						if(String.IsNullOrEmpty(Command))
						{
							Command = splitted;
						}else
						{
							Arguments.Add(splitted);
						}
					}
				}
			}else{
				Command = packet;
			}
		}
	}
}

