using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  using Portal.v5.Extension.Result;

  public class AnswerExtension : AExtension
  {
    public AnswerExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public EndpointResult Set(string questionId, AnswerDto answer)
    {
      var question = CockpitContext.QuestionGateway.Get(questionId);

      question.UserAnswer = AnswerDtoFactory.Map(answer);

      CockpitContext.QuestionGateway.Save(question);

      return EndpointResult.Success();
    }
  }
}