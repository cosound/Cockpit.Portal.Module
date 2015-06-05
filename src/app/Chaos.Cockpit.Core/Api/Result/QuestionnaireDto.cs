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

    [Serialize]
    public string Css { get; set; }

    [Serialize]
    public string Version { get; set; }

    [Serialize]
    public bool LockQuestion { get; set; }

    [Serialize]
    public bool EnablePrevious { get; set; }

    [Serialize]
    public string FooterLabel { get; set; }

    [Serialize]
    public uint CurrentSlideIndex { get; set; }

    [JsonIgnore]
    public IList<SlideDto> Slides { get; set; }

    public QuestionnaireDto()
    {
      Slides = new List<SlideDto>();
    }

    [Serialize]
    public string Fullname { get { return "Questionnaire, 1.0"; } }
  }
}