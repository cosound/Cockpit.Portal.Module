namespace Chaos.Cockpit.Core.Core
{
  using System.Collections.Generic;
  using Data.InMemory;

  public class Answer : IEntity
  {
    public string Identifier { get; set; }

    public string Type { get; set; }

    public IDictionary<string, string> Data { get; set; }

    public Answer(string type)
    {
      Data = new Dictionary<string, string>();
      Type = type;
    }

    public static Answer CreateBooleanAnswer()
    {
      return new Answer("BooleanAnswer, 1.0");
    }

    public static Answer CreateMultipleChoiceAnswer()
    {
      return new Answer("ABAnswer, 1.0");
    }
  }
}