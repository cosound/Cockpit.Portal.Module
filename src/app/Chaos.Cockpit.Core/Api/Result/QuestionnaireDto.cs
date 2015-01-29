namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;
  using Newtonsoft.Json;
  using Portal.Core.Data.Model;

  public class QuestionnaireDto : IResult
  {
    [Serialize("Id"), JsonProperty("Id")]
    public string Identity { get; set; }
    
    [Serialize("Name")]
    public string Name { get; set; }

    [Serialize("Slides")]
    public IList<SlideDto> Slides { get; set; }

    public QuestionnaireDto()
    {
      Slides = new List<SlideDto>();
    }

    [Serialize]
    public string Fullname { get { return "Questionnaire, 1.0"; } }
  }
}