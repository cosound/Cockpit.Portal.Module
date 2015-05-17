using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Data.InMemory
{
  public class ExperimentGateway : EntityRepository<ClaimedList>, IExperimentGateway
  {
    public ClaimedList Save(ClaimedList entity)
    {
      return Set(entity);
    }

    public ClaimedList Get(string id)
    {
      return Retrieve(id);
    }
  }
}