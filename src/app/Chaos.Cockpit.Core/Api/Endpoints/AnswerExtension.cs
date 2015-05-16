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
      var idSplit = questionId.Split(':');
      var questionaireId = Guid.Parse(idSplit[0]);
      var index = int.Parse(idSplit[1]);
      var questionaire = Context.QuestionnaireGateway.Get(questionaireId);

      if (Request.IsAnonymousUser && questionaire.Slides[index].IsClosed)
        throw new SlideClosedException("Slide has been closed by calling Slide/Close", "The requested slide is not available for editing");

      var question = questionaire.GetQuestion(questionId);
      question.Output = OutputMapper.Map(output);

      Context.QuestionGateway.Save(question);

      return EndpointResult.Success();
    }
  }
}