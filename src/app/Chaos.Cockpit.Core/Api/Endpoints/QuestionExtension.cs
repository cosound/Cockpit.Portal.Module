﻿namespace Chaos.Cockpit.Core.Api.Endpoints
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

    public IPagedResult<QuestionDto> Get(string id, uint index = 0u)
    {
      var questionnaire = CockpitContext.QuestionnaireGateway.Get(id);
      var questionnaireDto = QuestionnaireBuilder.MakeDto(questionnaire);
      var questions = questionnaireDto.Slides[(int) index].Questions;

      return new PagedResult<QuestionDto>((uint) questionnaireDto.Slides.Count, index, questions);
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
          Data = question.Data,
          UserAnswer = question.UserAnswer == null
                         ? null
                         : AnswerDtoFactory.Map(question.UserAnswer)
        };
    }
  }
}