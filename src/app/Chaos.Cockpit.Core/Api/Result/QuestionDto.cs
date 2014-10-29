namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;
  using Newtonsoft.Json;
  using Portal.Core.Data.Model;

  [Serialize]
  public class QuestionDto : IResult
  {
    [Serialize("Id"), JsonProperty("Id")]
    public string Identifier { get; set; }

    [Serialize]
    public AnswerDto UserAnswer { get; set; }

    [Serialize]
    public string Type { get; set; }

    [Serialize]
    public IDictionary<string, string> Data { get; set; }

    private QuestionDto(string type)
    {
      Data = new Dictionary<string, string>();
    }

    public static QuestionDto CreateBooleanAnswer()
    {
      return new QuestionDto("BooleanQuestion, 1.0")
        {
          Data = new Dictionary<string, string>
            {
              {"Text",""}
            }
        };
    }

    public static QuestionDto CreateMultipleChoiceAnswer()
    {
      return new QuestionDto("ABQuestion, 1.0")
      {
        Data = new Dictionary<string, string>
            {
              {"Text",""},
              {"Url1",""},
              {"Url2",""}
            }
      };
    }

    public string Fullname
    {
      get { return "Question, 1.0.0"; }
    }
  }
}