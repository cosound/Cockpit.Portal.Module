namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;
  using Portal.Core.Data.Model;

  public class QuestionnaireResult : IResult
  {
    [Serialize("Id")]
    public string Identity { get; set; }
    
    [Serialize("Name")]
    public string Name { get; set; }

    [Serialize("Slides")]
    public IList<SlideResult> Slides { get; set; }

    public QuestionnaireResult()
    {
      Slides = new List<SlideResult>();
    }

    [Serialize]
    public string Fullname { get { return "Questionnaire, 1.0"; } }
  }
}