using Chaos.Portal.Core.Data.Model;
using CHAOS.Serialization;

namespace Chaos.Cockpit.Core.Api.Result
{
	public class StringResult : AResult
	{
		[Serialize]
		public string Value { get; set; }

		public StringResult(string value)
		{
			Value = value;
		}
	}
}