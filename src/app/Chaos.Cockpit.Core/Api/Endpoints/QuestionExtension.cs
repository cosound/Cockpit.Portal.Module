using System;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
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

      if (questionnaire.LockQuestion && Request.IsAnonymousUser && questionnaire.Slides[index].IsCompleted)
        throw new SlideLockedException("Slide has been locked by calling Slide/Complete while LockQuestion is specified on the experiment", "The requested slide is not available for viewing");

      var questionnaireDto = QuestionnaireBuilder.MakeDto(questionnaire);
      var questions = questionnaireDto.Slides[index].Questions;

      return new PagedResult<QuestionDto>((uint) questionnaireDto.Slides.Count, (uint) index, questions);
    }
  }
}