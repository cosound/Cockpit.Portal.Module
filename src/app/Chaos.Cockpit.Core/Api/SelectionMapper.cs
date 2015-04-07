using System.Linq;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Api
{
  public static class SelectionMapper
  {
    public static SelectionResult Map(Selection selection)
    {
      return new SelectionResult
        {
          Identifier = selection.Id,
          Name = selection.Name,
          Items = selection.Items.Select(item => new Chaos.Cockpit.Core.Api.Result.Item { Identifier = item.Id }).ToList()
        };
    }

    public static Selection Map(SelectionResult selection)
    {
      return new Selection
        {
          Id = selection.Identifier,
          Name = selection.Name,
          Items = selection.Items.Select(item => new Chaos.Cockpit.Core.Core.Item { Id = item.Identifier }).ToList()
        };
    }
  }
}