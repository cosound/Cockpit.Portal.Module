using System;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Exceptions;
using Chaos.Portal.Core.Extension;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  public class SelectionExtension : AExtension
  {
    public SelectionExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }


    public SelectionResult Set(SelectionResult selection)
    {
      if(Request.IsAnonymousUser)
        throw new InsufficientPermissionsException("User is not authenticated");

      if (string.IsNullOrEmpty(selection.Identifier))
        selection.Identifier = Guid.NewGuid().ToString();

      var result = Context.SelectionGateway.Set(SelectionMapper.Map(selection));

      return SelectionMapper.Map(result);
    }
  }
}