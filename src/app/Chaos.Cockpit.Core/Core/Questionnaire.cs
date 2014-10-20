namespace Chaos.Cockpit.Core.Core
{
  using System.Collections.Generic;
  using Api.Result;
  using Data.InMemory;

  public class Questionnaire : IEntity
  {
    public string Identifier { get; set; }
    
    public string Name { get; set; }

    public IList<Slide> Slides { get; set; }

    public Questionnaire()
    {
      Slides = new List<Slide>();
    }
  }

  public class Slide
  {
    public IList<Question> Questions { get; set; }

    public Slide()
    {
      Questions = new List<Question>();
    }
  }
}