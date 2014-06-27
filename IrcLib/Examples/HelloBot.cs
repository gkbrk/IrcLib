using System;
using IrcLib;

namespace HelloBot
{
	public class HelloBot
	{
		public static void Main(string[] args)
		{
			IrcConnection ic = new IrcConnection();

			ic.OnConnect += (object sender, OnConnectArgs e) => {
				ic.SetNick("TheHelloBot");
				ic.SendUserPacket("HelloBot");
			};

			ic.OnWelcome += (object sender, EventArgs e) => {
				ic.JoinChannel("#HelloBotTest");
			};

			ic.OnPublicMessage += (object sender, OnPublicMessageArgs e) => {
				if(e.Message.ToLower().Contains("hello"))
				{
					//If someone on the channel says hello
					ic.SendMessage(e.Channel, "Hi there!"); //Reply with "Hi there!"
				}
			};

			ic.Connect("irc.freenode.net", 6667); //Connects to Freenode.
			ic.RunLoop(); //Runs the main loop so it can process packets.
		}
	}
}