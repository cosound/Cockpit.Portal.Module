using System.Xml.Linq;
using Chaos.Cockpit.Core.Data.Mcm;
using Chaos.Mcm;

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

      portalApplication.OnModuleLoaded += (sender, args) =>
        {
          var mcm = args.Module as IMcmModule;

          if (mcm == null) return;

          CockpitContext.QuestionnaireGateway = new McmQuestionnaireGateway(mcm.McmRepository);
        };

      portalApplication.AddBinding(typeof (AnswerDto), new JsonBinding<AnswerDto>());
      portalApplication.AddBinding(typeof(OutputDto), new OutputBinding());

      portalApplication.MapRoute("/v6/Question", () => new Api.Endpoints.QuestionExtension(portalApplication));
      portalApplication.MapRoute("/v6/Answer", () => new Api.Endpoints.AnswerExtension(portalApplication));
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