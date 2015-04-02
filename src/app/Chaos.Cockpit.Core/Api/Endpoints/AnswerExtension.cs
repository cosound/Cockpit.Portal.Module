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

    public EndpointResult Set(string questionId, OutputDto output)
    {
      var question = Context.QuestionGateway.Get(questionId);
      
      question.Output = OutputMapper.Map(output);

      Context.QuestionGateway.Save(question);

      return EndpointResult.Success();
    }
  }
}