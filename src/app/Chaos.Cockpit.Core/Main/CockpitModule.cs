using System.Xml.Linq;
using Chaos.Cockpit.Core.Core.Validation;
using Chaos.Cockpit.Core.Data.Mcm;

namespace Chaos.Cockpit.Core.Main
{
  using System.Collections.Generic;
  using Api.Binding;
  using Api.Result;
  using Core;
  using Portal.Core;
  using Portal.Core.Module;

  public class CockpitModule : IModuleConfig
  {
    public void Load(IPortalApplication portalApplication)
    {
      CockpitContext.QuestionnaireGateway = new Data.InMemory.QuestionnaireGateway();
      CockpitContext.QuestionGateway = new Data.InMemory.QuestionGateway();

      var xml = XDocument.Load(@"C:\inetpub\wwwroot\api.dev.cosound.dk\Modules\experiment.xml");
      var question = new DtuFormatConverter().Deserialize(xml);

      CockpitContext.QuestionnaireGateway.Set(question);
      portalApplication.AddBinding(typeof (AnswerDto),
                                   new JsonBinding<AnswerDto>());

      portalApplication.MapRoute("/v6/Question",
                                 () => new Api.Endpoints.QuestionExtension(portalApplication));
      portalApplication.MapRoute("/v6/Answer",
                                 () => new Api.Endpoints.AnswerExtension(portalApplication));
    }
  }
}