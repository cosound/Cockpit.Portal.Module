using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  public class AnswerExtension : AExtension
  {
    public AnswerExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public void Set(string questionId, AnswerResult answerResult)
    {
      var question = CockpitContext.QuestionGateway.Get(questionId);

      question.UserAnswer = AnswerResultMapper.Map(answerResult);

      CockpitContext.QuestionGateway.Save(question);
    }
  }
}