namespace Chaos.Cockpit.Core.Api.Endpoints
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Portal.Core;
  using Portal.Core.Extension;
  using Result;

  public class Questionnaire : AExtension
  {
    public Questionnaire(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public QuestionnaireResult Get(string id)
    {
      var questionnaire = CockpitContext.QuestionnaireGateway.Get(id);

      return QuestionnaireBuilder.MakeDto(questionnaire);
    }
  }

  public static class QuestionnaireBuilder
  {
    public static QuestionnaireResult MakeDto(Core.Questionnaire questionnaire)
    {
      var dto = new QuestionnaireResult();
      dto.Identity = questionnaire.Identifier;
      dto.Name = questionnaire.Name;
      dto.Slides = CreateSlides(questionnaire.Slides).ToList();

      return dto;
    }

    private static IEnumerable<SlideResult> CreateSlides(IEnumerable<Slide> slides)
    {
      foreach (var slide in slides)
      {
        var s = new SlideResult();

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
    public static QuestionResult MakeDto(Core.Question question)
    {
      var booleanQ = question as BooleanQuestion;
      var abQ = question as AbQuestion;

      if (booleanQ != null)
         return new BooleanQuestionResult
           {
             Identifier = booleanQ.Identifier,
             Value = booleanQ.Value
           };

      if (abQ != null)
        return new AbQuestionResult
           {
             Identifier = abQ.Identifier,
             Text = abQ.Text,
             Url1 = abQ.Url1,
             Url2 = abQ.Url2
           };

      throw new Exception("Unsupported Question");
    }
  }
}
