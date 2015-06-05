using System;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;
using Chaos.Portal.v5.Extension.Result;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  public class AnswerExtension : AExtension
  {
    public AnswerExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public EndpointResult Set(string questionId, OutputDto output)
    {
      var id = new QuestionId(questionId);
      var questionnaire = Context.QuestionnaireGateway.Get(id.QuestionaireId);

      if (questionnaire.LockQuestion && Request.IsAnonymousUser && questionnaire.GetSlide(questionId).IsCompleted)
        throw new SlideLockedException("Slide has been locked by calling Slide/Complete while LockQuestion is specified on the experiment", "The requested slide is not available for editing");

      var question = questionnaire.GetQuestion(questionId);
      question.Output = OutputMapper.Map(output);

      Context.QuestionGateway.Save(question);

      return EndpointResult.Success();
    }

    private class QuestionId
    {
      public QuestionId(string id)
      {
        var idSplit = id.Split(':');
        QuestionaireId = Guid.Parse(idSplit[0]);
        Index = int.Parse(idSplit[1]);
      }

      public Guid QuestionaireId { get; private set; }
      public int Index { get; private set; }
    }
  }
}