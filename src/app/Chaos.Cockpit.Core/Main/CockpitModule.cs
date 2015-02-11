using System.Xml.Linq;
using Chaos.Cockpit.Core.Data.Mcm;

namespace Chaos.Cockpit.Core.Main
{
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

      LoadExperiments();
      
      portalApplication.AddBinding(typeof (AnswerDto),
                                   new JsonBinding<AnswerDto>());

      portalApplication.MapRoute("/v6/Question",
                                 () => new Api.Endpoints.QuestionExtension(portalApplication));
      portalApplication.MapRoute("/v6/Answer",
                                 () => new Api.Endpoints.AnswerExtension(portalApplication));
    }

    private static void LoadExperiments()
    {
      try
      {
        foreach (var file in System.IO.Directory.GetFiles(@"C:\inetpub\wwwroot\api.dev.cosound.dk\experiments", "*.xml"))
        {
          var xml = XDocument.Load(file);
          var question = new DtuFormatConverter().Deserialize(xml);
          CockpitContext.QuestionnaireGateway.Set(question);
        }
      }
      catch (System.IO.DirectoryNotFoundException)
      {
      }
    }
  }
}