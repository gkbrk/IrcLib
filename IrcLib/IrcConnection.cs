using System;
using System.Net.Sockets;
using System.IO;

namespace IrcLib
{
	public class IrcConnection
	{
		SimpleSocket socket = new SimpleSocket ();

		public event EventHandler<OnConnectArgs> OnConnect;
		public event EventHandler<OnSocketLineReceiveArgs> OnSocketLineReceive;
		public event EventHandler<OnIrcPacketReceivedArgs> OnIrcPacketReceived;
		public event EventHandler<OnPublicMessageArgs> OnPublicMessage;
		public event EventHandler<OnPrivMsgArgs> OnPrivMsg;
		public event EventHandler OnWelcome;
		public event EventHandler OnPingReceived;

		public string Nick;

		public IrcConnection ()
		{
		}

		public void Connect (string host, int port)
		{
			socket.Connect (host, port);

			if (OnConnect != null) {
				OnConnect (this, new OnConnectArgs (host, port));
			}
		}

		public void RunLoop ()
		{
			while (true) {
				RunOnce();
			}
		}

		public void RunOnce ()
		{
			string line = socket.read_until ("\r\n", false);

			if (OnSocketLineReceive != null) {
				OnSocketLineReceive (this, new OnSocketLineReceiveArgs (line));
			}

			IrcPacket packet = new IrcPacket ();
			packet.Parse (line);

			if (OnIrcPacketReceived != null) {
				OnIrcPacketReceived (this, new OnIrcPacketReceivedArgs (packet));
			}

			if (packet.Command == "PRIVMSG") {
				if (packet.Arguments [0].StartsWith ("#")) {
					if (OnPublicMessage != null) {
						OnPublicMessage (this, new OnPublicMessageArgs (packet.Prefix.Split ('!') [0], packet.Arguments [0], packet.Arguments [1]));
					}
				} else {
					if (OnPrivMsg != null) {
						OnPrivMsg (this, new OnPrivMsgArgs (packet.Prefix.Split ('!') [0], packet.Arguments [1]));
					}
				}
			} else if (packet.Command == "PING") {
				SendLine (String.Format ("PONG :{0}", packet.Arguments [0]));

				if (OnPingReceived != null) {
					OnPingReceived (this, EventArgs.Empty);
				}
			} else if (packet.Command == "433") {
				//Command 433 is "Nick in use".
				//This will add underscore to the nickname until we find a free one.
				SetNick (String.Format ("{0}_", Nick));
			} else if (packet.Command == "001") {
				//Command 376 is "End of motd".

				if (OnWelcome != null) {
					OnWelcome (this, EventArgs.Empty);
				}
			}
		}

		public void SendLine (string line)
		{
			socket.send_string (String.Format ("{0}\r\n", line));
		}

		public void SendMessage (string to, string message)
		{
			SendLine (String.Format ("PRIVMSG {0} :{1}", to, message));
		}

		public void JoinChannel (string channel_name)
		{
			SendLine (String.Format ("JOIN {0}", channel_name));
		}

		public void SetNick (string nick)
		{
			Nick = nick;
			SendLine (String.Format ("NICK {0}", nick));
		}

		public void SendUserPacket (string username)
		{
			SendLine (String.Format ("USER {0} 0 * :{0}", username));
		}

		public void Quit (string reason)
		{
			SendLine (String.Format ("QUIT :{0}", reason));
		}

		public string getIdent (string nick)
		{
			string identname = "notIdented";
			bool whoisDone = false;
			EventHandler<OnIrcPacketReceivedArgs> handler = null;
			handler = delegate(object sender, OnIrcPacketReceivedArgs e) {
				if (e.Packet.Command == "330" && e.Packet.Arguments[1] == nick) {
					identname = e.Packet.Arguments[2];
				} else if (e.Packet.Command == "318" && e.Packet.Arguments[1] == nick) {
					OnIrcPacketReceived -= handler;
					whoisDone = true;
				}
			};
			OnIrcPacketReceived += handler;
			SendLine (String.Format ("WHOIS {0}", nick));
			while (!whoisDone) {
				RunOnce ();
			}
			return identname;
		}

		public long getPing()
		{
			long ping = -1;
			EventHandler<OnIrcPacketReceivedArgs> handler = null;
			long currentMillis = -1;
			handler = delegate(object sender, OnIrcPacketReceivedArgs e) {
				if(e.Packet.Command == "PONG" && e.Packet.Arguments[1] == "irclibping")
				{
					ping = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - currentMillis;
					OnIrcPacketReceived -= handler;
				}
			};
			SendLine("PING :irclibping");
			currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			OnIrcPacketReceived += handler;
			while(ping == -1)
			{
				RunOnce();
			}
			return ping;
		}
	}
}