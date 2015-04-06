using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Data.InMemory
{
  public class SelectionGateway : EntityRepository<Selection>, ISelectionGateway
  {
    public Selection Get(string id)
    {
      return Retrieve(id);
    }
  }
}