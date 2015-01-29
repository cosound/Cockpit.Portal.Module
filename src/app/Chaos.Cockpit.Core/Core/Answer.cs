using System.Collections.Generic;

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

  public class Output
  {
    public string Identifier { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public string ValueType { get; set; }
    public uint MinNoOfValues { get; set; }
    public uint MaxNoOfValues { get; set; }
  }
}