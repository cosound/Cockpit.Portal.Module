using System.Collections.Generic;
using System.Linq;

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
      slide.Parent = this;

      Slides.Add(slide);
    }

    internal void UpdateIds()
    {
      var count = 0;

      foreach (var question in Slides.SelectMany(slide => slide.Questions))
        question.Id = string.Format("{0}:{1}", Id, count++);
    }
  }

  public class Slide
  {
    public IList<Question> Questions { get; set; }

    internal Questionnaire Parent { get; set; }

    public Slide()
    {
      Questions = new List<Question>();
    }

    public void AddQuestion(Question question)
    {
      Questions.Add(question);

      Parent.UpdateIds();
    }
  }
}