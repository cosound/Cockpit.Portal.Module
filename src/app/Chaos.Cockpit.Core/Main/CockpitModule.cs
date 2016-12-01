using System.Xml.Linq;
using Chaos.Cockpit.Core.Api.Binding;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.Mcm;
using Chaos.Mcm;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Module;

namespace Chaos.Cockpit.Core.Main
{
  public class CockpitModule : IModuleConfig
  {
    public void Load(IPortalApplication portalApplication)
    {
      Context.QuestionnaireGateway = new Data.InMemory.QuestionnaireGateway();
      Context.QuestionGateway = new Data.InMemory.QuestionGateway();
      Context.SelectionGateway = new Data.InMemory.SelectionGateway();
      Context.ExperimentGateway = new Data.InMemory.ExperimentGateway();

      LoadExperiments();

      portalApplication.OnModuleLoaded += (sender, args) =>
        {
          var mcm = args.Module as IMcmModule;

          if (mcm == null) return;

          Context.QuestionnaireGateway = new McmQuestionnaireGateway(mcm.McmRepository);
          Context.QuestionGateway = new McmQuestionGateway(mcm.McmRepository);
          Context.ExperimentGateway = new McmExperimentGateway(mcm.McmRepository);

	        portalApplication.MapRoute("/v6/Upload", () => new Api.Endpoints.UploadExtension(portalApplication, mcm.Configuration.Aws));
        };

      portalApplication.AddBinding(typeof(AnswerDto), new JsonBinding<AnswerDto>());
      portalApplication.AddBinding(typeof(SelectionResult), new JsonBinding<SelectionResult>());
      portalApplication.AddBinding(typeof(OutputDto), new OutputBinding());

      portalApplication.MapRoute("/v6/AudioInformation", () => new Api.Endpoints.AudioInformation(portalApplication, new Http()));
      portalApplication.MapRoute("/v6/Experiment", () => new Api.Endpoints.ExperimentExtension(portalApplication));
      portalApplication.MapRoute("/v6/Selection", () => new Api.Endpoints.SelectionExtension(portalApplication));
      portalApplication.MapRoute("/v6/Question", () => new Api.Endpoints.QuestionExtension(portalApplication));
      portalApplication.MapRoute("/v6/Answer", () => new Api.Endpoints.AnswerExtension(portalApplication));
      portalApplication.MapRoute("/v6/Search", () => new Api.Endpoints.SearchExtension(portalApplication));
      portalApplication.MapRoute("/v6/Slide", () => new Api.Endpoints.SlideExtension(portalApplication));
    }

    private static void LoadExperiments()
    {
      try
      {
        foreach (var file in System.IO.Directory.GetFiles(@"C:\inetpub\wwwroot\api.dev.cosound.dk\experiments", "*.xml"))
        {
          var xml = XDocument.Load(file);
          var question = new DtuFormatConverter().Deserialize(xml);
          
          Context.QuestionnaireGateway.Set(question);
        }
      }
      catch (System.IO.DirectoryNotFoundException)
      {
      }
    }
  }
}