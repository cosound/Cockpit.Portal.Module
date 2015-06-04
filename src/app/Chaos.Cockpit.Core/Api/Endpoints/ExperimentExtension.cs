using System;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  public class ExperimentExtension : AExtension
  {
    public ExperimentExtension(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public ExperimentResult Next(Guid listId)
    {
      var list = Context.ExperimentGateway.Get(listId.ToString());

      var result = list.Next();

      Context.ExperimentGateway.Save(list);

      return result;
    }

    public QuestionnaireDto Get(Guid id)
    {
      var result = Context.QuestionnaireGateway.Get(id);

      return QuestionnaireBuilder.MakeDto(result);
    }
  }
}