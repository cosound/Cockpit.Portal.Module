namespace Chaos.Cockpit.Core.Api.Endpoints
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Portal.Core;
  using Portal.Core.Extension;
  using Result;

  public class QuestionnaireExtension : AExtension
  {
    public QuestionnaireExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public QuestionnaireDto Get(string id)
    {
      var questionnaire = CockpitContext.QuestionnaireGateway.Get(id);

      return QuestionnaireBuilder.MakeDto(questionnaire);
    }
  }

  public static class QuestionnaireBuilder
  {
    public static QuestionnaireDto MakeDto(Core.Questionnaire questionnaire)
    {
      var dto = new QuestionnaireDto();
      dto.Identity = questionnaire.Identifier;
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
      if (question.Type == "BooleanQuestion, 1.0")
      {
        var dto = QuestionDto.CreateBooleanAnswer();
        dto.Identifier = question.Identifier;
        dto.Data = question.Data;
        dto.UserAnswer = question.UserAnswer == null
                           ? null
                           : AnswerDtoFactory.Map(question.UserAnswer);
        
        return dto;
      }

      if (question.Type == "AbQuestion, 1.0")
      {
        var dto = QuestionDto.CreateBooleanAnswer();
        dto.Identifier = question.Identifier;
        dto.Data = question.Data;
        dto.UserAnswer = question.UserAnswer == null
                           ? null
                           : AnswerDtoFactory.Map(question.UserAnswer);
        
        return dto;
      }

      throw new Exception(question.GetType().FullName);
    }
  }
}
