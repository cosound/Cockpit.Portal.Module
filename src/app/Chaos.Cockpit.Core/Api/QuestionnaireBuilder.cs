using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Api
{
  public static class QuestionnaireBuilder
  {
    public static QuestionnaireDto MakeDto(Core.Questionnaire questionnaire)
    {
      var dto = new QuestionnaireDto();
      dto.Identity = questionnaire.Id;
      dto.Name = questionnaire.Name;
      dto.Version = questionnaire.Version;
      dto.Css = questionnaire.Css;
      dto.LockQuestion = questionnaire.LockQuestion;
      dto.EnablePrevious = questionnaire.EnablePrevious;
      dto.FooterLabel = questionnaire.FooterLabel;
      dto.Slides = CreateSlides(questionnaire.Slides).ToList();
      dto.CurrentSlideIndex = questionnaire.NextSlide();

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
}