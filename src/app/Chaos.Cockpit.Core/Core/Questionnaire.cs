namespace Chaos.Cockpit.Core.Core
{
  using System.Collections.Generic;
  using Api.Result;
  using Data.InMemory;

  public class Questionnaire : IEntiy
  {
    public string Identity { get; set; }
    
    public string Name { get; set; }

    public IList<Slide> Slides { get; set; }

    public Questionnaire()
    {
      Slides = new List<Slide>();
    }
  }
}