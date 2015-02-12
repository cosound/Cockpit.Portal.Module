using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
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

    public EndpointResult Set(string questionId, OutputDto answer)
    {
      var question = CockpitContext.QuestionGateway.Get(questionId);

      question.Output = OutputMapper.Map(answer);

      CockpitContext.QuestionGateway.Save(question);

      return EndpointResult.Success();
    }

    
  }
}