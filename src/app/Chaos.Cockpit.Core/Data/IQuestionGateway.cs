using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Data
{
  public interface IQuestionGateway
  {
    Question Save(Question entity);
    Question Get(string id);
  }
}
