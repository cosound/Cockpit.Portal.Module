using CHAOS.Serialization;
using Chaos.Portal.Core.Data.Model;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Api.Result
{
  public class SearchResult : AResult
  {
    [Serialize("Id"), JsonProperty("Id")]
    public string Identifier { get; set; }

    [Serialize]
    public string Title { get; set; }
  }
}