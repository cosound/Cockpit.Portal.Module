using System.Net;
using System.Text;

namespace Chaos.Cockpit.Core.Core
{
	public class Http : IHttpGet
	{
		public string Get(string url)
		{
			var req = WebRequest.Create(url);
			req.Method = "GET";

			using (var response = req.GetResponse())
			{
				var buffer = new byte[1024 * 1024];
				var result = "";
				int read;

				while ((read = response.GetResponseStream().Read(buffer, 0, 0)) != 0)
				{
					result += Encoding.UTF8.GetString(buffer, 0, read);
				}

				return result;
			}
		}
	}
}