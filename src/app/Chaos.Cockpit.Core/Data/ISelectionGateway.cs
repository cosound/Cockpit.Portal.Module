using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Data
{
  public interface ISelectionGateway
  {
    Selection Set(Selection selection);
    Selection Get(string id);
    bool Delete(string id);
  }
}