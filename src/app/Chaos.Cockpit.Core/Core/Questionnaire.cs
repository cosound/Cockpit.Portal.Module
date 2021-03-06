﻿using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core
{
  public class Questionnaire : IKey
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public string Css { get; set; }
    public bool LockQuestion { get; set; }
    public bool EnablePrevious { get; set; }
    public string FooterLabel { get; set; }
    public string RedirectOnCloseUrl { get; set; }
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

    public uint NextSlide()
    {
      if (!Slides.Any())
        throw new DataNotFoundException();

      for(var i = 0; i < Slides.Count; i++)
      {
        var slide = Slides[i];

        if (!slide.IsCompleted)
          return (uint) i;
      }

      return 0;
    }
  }

  public class Slide
  {
    public string TaskId { get; set; }
    public IList<Question> Questions { get; set; }
    internal Questionnaire Parent { get; set; }
    public bool IsCompleted { get; set; }

    public Slide()
    {
      Questions = new List<Question>();
    }

    public void AddQuestion(Question question)
    {
      Questions.Add(question);

      Parent.UpdateIds();
    }

    public void Validate()
    {
      foreach (var question in Questions)
        question.Validate();
    }
  }
}