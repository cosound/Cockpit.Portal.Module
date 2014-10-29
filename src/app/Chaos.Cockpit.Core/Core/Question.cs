using Chaos.Cockpit.Core.Data.InMemory;

namespace Chaos.Cockpit.Core.Core
{
  using System.Collections.Generic;

  public class Question : IEntity
  {
    public string Identifier { get; set; }
    public Answer UserAnswer { get; set; }
    public string Type { get; set; }
    public IDictionary<string, string> Data { get; set; }

    public Question(string type)
    {
      Type = type;
    }

    public static Question CreateBooleanQuestion()
    {
      return new Question("BooleanQuestion, 1.0")
        {
          Data = new Dictionary<string, string>
            {
              {"Text",""}
            }
        };
    }

    public static Question CreateABQuestion()
    {
      return new Question("AbQuestion, 1.0")
      {
        Data = new Dictionary<string, string>
            {
              {"Text",""},
              {"Url1",""},
              {"Url2",""}
            }
      };
    }
  }
}
