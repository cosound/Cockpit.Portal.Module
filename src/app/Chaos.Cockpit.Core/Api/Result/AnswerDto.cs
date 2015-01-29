namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;
  using Newtonsoft.Json;
  using Portal.Core.Data.Model;

  [Serialize]
  public class AnswerDto : AResult
  {
    [Serialize("Id"), JsonProperty("Id")]
    public string Identifier { get; set; }

    [Serialize]
    public string Type { get; set; }

    [Serialize]
    public IDictionary<string, string> Data { get; set; }

    public AnswerDto(string type)
    {
      Type = type;
      Data = new Dictionary<string, string>();
    }
  }
}