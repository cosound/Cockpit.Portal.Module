using Chaos.Cockpit.Core.Data.InMemory;

namespace Chaos.Cockpit.Core.Core
{
  public class Question : IEntity
  {
    public string Identifier { get; set; }

    public Answer UserAnswer { get; set; }
  }

  public class BooleanQuestion : Question
  {
    public string Value { get; set; }
  }

  public class AbQuestion : Question
  {
    public string Text { get; set; }
    public string Url1 { get; set; }
    public string Url2 { get; set; }
  }
}
