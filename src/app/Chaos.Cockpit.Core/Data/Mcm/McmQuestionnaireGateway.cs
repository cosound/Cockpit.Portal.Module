using Chaos.Cockpit.Core.Core;
using Chaos.Mcm.Data;

namespace Chaos.Cockpit.Core.Data.Mcm
{
  public class McmQuestionnaireGateway : IQuestionnaireGateway
  {
    public IMcmRepository Repository { get; set; }

    public McmQuestionnaireGateway(IMcmRepository repository)
    {
      Repository = repository;
    }

    public Questionnaire Set(Questionnaire entity)
    {
      throw new System.NotImplementedException();
    }

    public Questionnaire Get(string id)
    {
      throw new System.NotImplementedException();
    }

    public Questionnaire GetByQuestionId(string identifier)
    {
      throw new System.NotImplementedException();
    }
  }
}