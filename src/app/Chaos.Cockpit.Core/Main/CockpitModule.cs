using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Core.Validation;
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
      
      portalApplication.AddBinding(typeof (AnswerDto), new JsonBinding<AnswerDto>());

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
          question.Slides[0].Questions[0].Output = new Output()
          {
            SimpleValues = new List<SimpleValue>()
            {
              new SimpleValue("k1", "v1"),
              new SimpleValue("k2", "v2")
            },
            ComplexValues = new List<ComplexValue>()
            {
              new ComplexValue
                {
                  Key = "k3",
                  SimpleValues = new List<SimpleValue>()
                    {
                      new SimpleValue("k3.1", "v3.1"),
                      new SimpleValue("k3.2", "v3.2")
                    }
                }
            },
            MultiValues = new List<MultiValue>()
              {
                new MultiValue
                  {
                    Key = "k4",
                    SimpleValues = new List<string>()
                      {
                        "v4.1","v4.2"
                      }
                  },
                  new MultiValue()
                    {
                      Key = "k5",
                      ComplexValues = new List<ComplexValue>()
                        {
                          new ComplexValue
                            {
                              Key = "k5.1",
                              SimpleValues = new List<SimpleValue>()
                                {
                                  new SimpleValue("k5.1.1", "v5.1.1"),
                                  new SimpleValue("k5.1.2", "v5.1.2")
                                }
                            }
                        }
                    }
              }
          };
          CockpitContext.QuestionnaireGateway.Set(question);
        }
      }
      catch (System.IO.DirectoryNotFoundException)
      {
      }
    }
  }
}