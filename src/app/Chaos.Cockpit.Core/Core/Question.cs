using Chaos.Cockpit.Core.Data.InMemory;

namespace Chaos.Cockpit.Core.Core
{
  using System.Collections.Generic;

  public class Question : IKey
  {
    public string Id { get; set; }
    public Answer UserAnswer { get; set; }
    public string Type { get; set; }
    public IDictionary<string, object> Data { get; set; }
    public IDictionary<string, Output> Output { get; set; }

    public Question(string type)
    {
      Type = type;
      Output = new Dictionary<string, Output>();
      Data = new Dictionary<string, object>();
    }
  }
}
