using System.Collections.Generic;
using System.Xml.Linq;
using CHAOS.Serialization;
using Chaos.Portal.Core.Data.Model;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Api.Result
{
  [Serialize]
  public class QuestionDto : IResult
  {
    [Serialize("Id"), JsonProperty("Id")]
    public string Identifier { get; set; }

    [Serialize]
    public AnswerDto UserAnswer { get; set; }

    [Serialize]
    public string Type { get; set; }

    public QuestionDto(string type)
    {
      Type = type;
      Input = new List<XElement>();
      Output = new OutputDto();
    }

    public string Fullname
    {
      get { return "Question, 1.0.0"; }
    }

    public static QuestionDto CreateStartDateTimeAnswer()
    {
      return new QuestionDto("Monitor:Event:StartAtDateTime");
    }
    
    public static QuestionDto CreateEndDateTimeAnswer()
    {
      return new QuestionDto("Monitor:Event:EndAtDateTime");
    }

    
    public IEnumerable<XElement> Input { get; set; }

    [JsonIgnore]
    public OutputDto Output { get; set; }

    [JsonProperty("Output")]
    public IDictionary<string, object> Values
    {
      get
      {
        if(Output == null) return null;

        var dict = new Dictionary<string, object>();

        foreach (var singleValue in Output.SingleValues)
          dict.Add(singleValue.Key, singleValue.Value);

        foreach (var multiValueResult in Output.MultiValues)
          dict.Add(multiValueResult.Key, multiValueResult.Value.Values);

        foreach (var complexValueResult in Output.ComplexValues)
          dict.Add(complexValueResult.Key, complexValueResult.Value.Values);

        return dict;
      }
    }
  }
}