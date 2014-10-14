namespace Chaos.Cockpit.Core.Api.Result
{
  using CHAOS.Serialization;
  using Portal.Core.Data.Model;

  [Serialize]
  public class Answer : AResult
  {
    [Serialize]
    public string Identifier { get; set; }
  }

  public class BooleanAnswer : Answer
  {
    [Serialize]
    public bool Value { get; set; }
  }
}