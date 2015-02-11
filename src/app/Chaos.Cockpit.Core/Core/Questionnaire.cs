using System.Collections.Generic;

namespace Chaos.Cockpit.Core.Core
{
  public class Questionnaire : IKey
  {
    public string Id { get; set; }
    
    public string Name { get; set; }

    public IList<Slide> Slides { get; set; }

    public Questionnaire()
    {
      Slides = new List<Slide>();
    }

    public void AddSlide(Slide slide)
    {
      Slides.Add(slide);
    }
  }

  public class Slide
  {
    public IList<Question> Questions { get; set; }

    public Slide()
    {
      Questions = new List<Question>();
    }

    public void AddQuestion(Question question)
    {
      Questions.Add(question);
    }
  }
}