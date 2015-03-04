using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Data
{
  public interface IQuestionnaireGateway
  {
    Questionnaire Set(Questionnaire entity);
    Questionnaire Get(string id);
  }
}