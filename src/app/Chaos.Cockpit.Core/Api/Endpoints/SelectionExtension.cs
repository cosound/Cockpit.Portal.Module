using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Exceptions;
using Chaos.Portal.Core.Extension;
using Chaos.Portal.v5.Extension.Result;
using Item = Chaos.Cockpit.Core.Core.Item;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  public class SelectionExtension : AExtension
  {
    public SelectionExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public SelectionResult Set(SelectionResult selection)
    {
      RequiresAuthentication();

      if (string.IsNullOrEmpty(selection.Identifier))
        selection.Identifier = Guid.NewGuid().ToString();

      var result = Context.SelectionGateway.Set(SelectionMapper.Map(selection));

      return SelectionMapper.Map(result);
    }

    public SelectionResult Get(string id)
    {
      RequiresAuthentication();

      var result = Context.SelectionGateway.Get(id);

      return SelectionMapper.Map(result);
    }

    public EndpointResult Delete(string id)
    {
      RequiresAuthentication();

      return new EndpointResult { WasSuccess = Context.SelectionGateway.Delete(id) };
    }

    public EndpointResult AddItems(string id, IEnumerable<string> items)
    {
      RequiresAuthentication();

      var result = Context.SelectionGateway.Get(id);

      foreach (var item in items)
        result.Items.Add(new Item { Identifier = item });

      Context.SelectionGateway.Set(result);

      return EndpointResult.Success();
    }

    public EndpointResult DeleteItems(string id, IEnumerable<string> items)
    {
      RequiresAuthentication();

      var result = Context.SelectionGateway.Get(id);

      foreach (var item in items)
      {
        var itemToDelete = result.Items.FirstOrDefault(i => i.Identifier == item);

        if(itemToDelete == null) continue;

        result.Items.Remove(itemToDelete);
      }

      Context.SelectionGateway.Set(result);

      return EndpointResult.Success();
    }
    
    private void RequiresAuthentication()
    {
      if (Request.IsAnonymousUser)
        throw new InsufficientPermissionsException("User is not authenticated");
    }
  }
}