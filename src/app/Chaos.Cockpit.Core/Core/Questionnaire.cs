using System;
using System.Collections.Generic;
using System.Linq;

namespace Chaos.Cockpit.Core.Core
{
  public class Questionnaire : IKey
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public string TargetId { get; set; }
    public string TargetName { get; set; }
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

    public Question GetQuestion(string id)
    {
      foreach (var question in Slides.SelectMany(slide => slide.Questions).Where(question => question.Id == id))
        return question;

      throw new ArgumentException("No question with Id found.");
    }

    public Slide GetSlide(string id)
    {
      foreach (var slide in Slides.Where(slide => slide.Questions.Any(question => question.Id == id)))
        return slide;

      throw new ArgumentException("No question with Id found.");
    }
  }

  public class Slide
  {
    public string TaskId { get; set; }
    public IList<Question> Questions { get; set; }
    internal Questionnaire Parent { get; set; }
    public bool IsClosed { get; set; }

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