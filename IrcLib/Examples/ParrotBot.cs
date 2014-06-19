using System;
using IrcLib;

namespace ParrotBot
{
	public class ParrotBot
	{
		public static void Main(string[] args)
		{
			IrcConnection ic = new IrcConnection();

			ic.OnConnect += (object sender, OnConnectArgs e) => {
				ic.SetNick("TheParrotBot");
				ic.SendUserPacket("ParrotBot");
			};

			ic.OnWelcome += (object sender, EventArgs e) => {
				ic.JoinChannel("#ParrotBotTest");
			};

			ic.OnPublicMessage += (object sender, OnPublicMessageArgs e) => {
				//When the bot gets a public message from a channel
				//send the message back to the channel
				ic.SendMessage(e.Channel, e.Message);
			};

			ic.Connect("irc.freenode.net", 6667); //Connects to Freenode.
			ic.RunLoop(); //Runs the main loop so it can process packets.
		}
	}
}

