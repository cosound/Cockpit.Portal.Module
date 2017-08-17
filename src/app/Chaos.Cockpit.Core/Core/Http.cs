using System;
using System.Net.Sockets;
using System.Text;

namespace Chaos.Cockpit.Core.Core
{
	public class Http : IHttpGet
	{
		public string Get(string url)
		{
			using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				var endOfProtocol = url.IndexOf("://")+3;
				var startOfPort = url.IndexOf(":", endOfProtocol);
				var hostname = url.Substring(endOfProtocol, startOfPort - endOfProtocol);
				var endOfPort = url.IndexOf("/", startOfPort);
				var port = int.Parse(url.Substring(startOfPort + 1, endOfPort - startOfPort - 1));
				var qs = url.Substring(endOfPort);
				socket.Connect(hostname, port);
			
				var requestMessage = String.Format(
					"GET {2} HTTP/1.1\n"+
					"Host: {0}:{1}\n"+
					"User-Agent: CHAOS API 6\n"+
					"Connection: keep-alive\n\n", hostname, port, qs);
				
				var headerBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(requestMessage);
			
				socket.Send(headerBytes, headerBytes.Length, SocketFlags.None);
			
				var buffer = new byte[1024 * 1024];
				var read = 0;
				var result = "";
			
				while ((read = socket.Receive(buffer, SocketFlags.None)) != 0)
				{
					result += Encoding.UTF8.GetString(buffer, 0, read);
					buffer = new byte[1024*1024];
				}
			
				return result.Substring(result.IndexOf("\n\n"));
			}
		}
	}
}