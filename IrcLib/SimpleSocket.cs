using System;
using System.Net.Sockets;
using System.Text;

namespace IrcLib
{
	public class SimpleSocket : Socket
	{
		public SimpleSocket() : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
		{

		}
		public byte[] recv(int bytes)
		{
			byte[] bytes_read = new byte[bytes];
			Receive(bytes_read);
			return bytes_read;
		}

		public string recv_string(int bytes)
		{
			return Encoding.UTF8.GetString(recv(bytes));
		}

		public string read_until(string until, bool include)
		{
			string read_so_far = "";
			while (!read_so_far.EndsWith(until))
			{
				read_so_far += recv_string(1);
			}
			if (include)
			{
				return read_so_far;
			}else
			{
				return read_so_far.TrimEnd(until.ToCharArray());
			}
		}

		public void send_string(string data)
		{
			Send(Encoding.UTF8.GetBytes(data));
		}


	}
}