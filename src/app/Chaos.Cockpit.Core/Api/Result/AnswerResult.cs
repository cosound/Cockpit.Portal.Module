namespace Chaos.Cockpit.Core.Api.Result
{
  using CHAOS.Serialization;
  using Portal.Core.Data.Model;

  [Serialize]
  public class AnswerResult : AResult
  {
    [Serialize]
    public string Identifier { get; set; }
  }

  public class BooleanAnswerResult : AnswerResult
  {
    [Serialize]
    public bool Value { get; set; }
  }
}