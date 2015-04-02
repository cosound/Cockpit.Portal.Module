using System;
using Chaos.Cockpit.Core.Core;
using Chaos.Mcm.Data;

namespace Chaos.Cockpit.Core.Data.Mcm
{
  public class McmQuestionGateway : IQuestionGateway
  {
    public IMcmRepository McmRepository { get; set; }

    public McmQuestionGateway(IMcmRepository mcmRepository)
    {
      McmRepository = mcmRepository;
    }

    public Question Save(Question entity)
    {
      var questionaireId = Guid.Parse(entity.Id.Split(':')[0]);

      var questionaire = Context.QuestionnaireGateway.Get(questionaireId);
      var question = questionaire.GetQuestion(entity.Id);
      question.Output = entity.Output;

      Context.QuestionnaireGateway.Set(questionaire);

      return question;
    }

    public Question Get(string id)
    {
      var questionaireId = Guid.Parse(id.Split(':')[0]);

      var questionaire = Context.QuestionnaireGateway.Get(questionaireId);

      return questionaire.GetQuestion(id);
    }
  }
}