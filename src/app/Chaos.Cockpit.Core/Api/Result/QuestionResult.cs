namespace Chaos.Cockpit.Core.Api.Result
{
  using CHAOS.Serialization;
  using Portal.Core.Data.Model;

  [Serialize]
  public class QuestionResult : IResult
  {
    [Serialize("Id")]
    public string Identifier { get; set; }

    public string Fullname { get { return "Question, 1.0.0"; } }
  }

  public class BooleanQuestionResult : QuestionResult
  {
    [Serialize]
    public string Value { get; set; }

    [Serialize]
    public string Fullname { get { return "BooleanQuestion, 1.0"; } }
  }

  public class AbQuestionResult : QuestionResult
  {
    [Serialize]
    public string Text { get; set; }
    
    [Serialize]
    public string Url1 { get; set; }

    [Serialize]
    public string Url2 { get; set; }

    [Serialize]
    public string Fullname { get { return "ABQuestion, 1.0"; } }
  }
}