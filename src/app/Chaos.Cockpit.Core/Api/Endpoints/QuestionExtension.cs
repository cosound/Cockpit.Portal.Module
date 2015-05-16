﻿using System;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Portal.Core;
  using Portal.Core.Data.Model;
  using Portal.Core.Extension;
  using Result;

  public class QuestionExtension : AExtension
  {
    public QuestionExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public IPagedResult<QuestionDto> Get(string id, int index = 0)
    {
      var questionnaire = Context.QuestionnaireGateway.Get(Guid.Parse(id));

      if(Request.IsAnonymousUser && questionnaire.Slides[index].IsClosed) 
        throw new SlideClosedException("Slide has been closed by calling Slide/Close", "The requested slide is not available for viewing");

      var questionnaireDto = QuestionnaireBuilder.MakeDto(questionnaire);
      var questions = questionnaireDto.Slides[index].Questions;

      return new PagedResult<QuestionDto>((uint) questionnaireDto.Slides.Count, (uint) index, questions);
    }
  }

  public static class QuestionnaireBuilder
  {
    public static QuestionnaireDto MakeDto(Core.Questionnaire questionnaire)
    {
      var dto = new QuestionnaireDto();
      dto.Identity = questionnaire.Id;
      dto.Name = questionnaire.Name;
      dto.Slides = CreateSlides(questionnaire.Slides).ToList();

      return dto;
    }

    private static IEnumerable<SlideDto> CreateSlides(IEnumerable<Slide> slides)
    {
      foreach (var slide in slides)
      {
        var s = new SlideDto();

        foreach (var question in slide.Questions)
        {
          s.Questions.Add(QuestionBuilder.MakeDto(question));
        }

        yield return s;
      }
    }
  }

  public static class QuestionBuilder
  {
    public static QuestionDto MakeDto(Core.Question question)
    {
      return new QuestionDto(question.Type)
        {
          Identifier = question.Id,
          UserAnswer = question.UserAnswer == null
                         ? null
                         : AnswerDtoFactory.Map(question.UserAnswer),
          Input = question.Input,
          Output = question.Output == null ? null : OutputMapper.Map(question.Output)
        };
    }
  }
}