namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;
  using Portal.Core.Data.Model;

  public class Questionnaire : IResult
  {
    [Serialize("Id")]
    public string Identity { get; set; }
    
    [Serialize("Name")]
    public string Name { get; set; }

    [Serialize("Slides")]
    public IList<Slide> Slides { get; set; }

    public Questionnaire()
    {
      Slides = new List<Slide>();
    }

    [Serialize]
    public string Fullname { get { return "Questionnaire, 1.0"; } }
  }
}