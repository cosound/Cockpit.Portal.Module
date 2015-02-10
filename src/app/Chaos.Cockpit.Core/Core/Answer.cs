using System.Collections.Generic;
using Chaos.Cockpit.Core.Api.Result;

namespace Chaos.Cockpit.Core.Core
{
  public class Answer : IKey
  {
    public string Id { get; set; }

    public string Type { get; set; }

    public IDictionary<string, Output> Output { get; set; }
    public IDictionary<string, string> Data { get; set; }

    public Answer(string type)
    {
      Output = new Dictionary<string, Output>();
      Data = new Dictionary<string, string>();
      Type = type;
    }
  }
}