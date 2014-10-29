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

      CockpitContext.QuestionnaireGateway.Save(new Core.Questionnaire
        {
          Identifier = "a12f",
          Name = "Sample QuestionnaireResult",
          Slides = new List<Slide>()
            {
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("BooleanQuestion, 1.0")
                        {
                          Identifier = "1234", 
                          Data = new Dictionary<string, string>{{"Text" , "Do you have any friends?"}}
                        },
                      new Question("BooleanQuestion, 1.0")
                        {
                          Identifier = "2345", 
                          Data = new Dictionary<string, string>{{"Text" , "Do you have any friends?"}},
                        },
                      new Question("AbQuestion, 1.0")
                        {
                          Identifier = "3456", 
                          Data = new Dictionary<string, string>
                            {
                              {"Text" , "Which is more peppy"},
                              {"Url1" , "http://example.com/1.wav"},
                              {"Url2" , "http://example.com/2.wav"}
                            }
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("BooleanQuestion, 1.0")
                        {
                          Identifier = "4567", 
                          Data = new Dictionary<string, string>{{"Text" , "What if this was the last question?"}},
                        }
                    }
                }
            }
        });

      CockpitContext.QuestionnaireGateway.Save(new Core.Questionnaire
      {
        Identifier = "a122",
        Name = "Sample QuestionnaireResult 2",
        Slides = new List<Slide>()
            {
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("BooleanQuestion, 1.0")
                        {
                          Identifier = "9876", 
                          Data = new Dictionary<string, string>{{"Text" , "Do you have any friends?"}}
                        },
                      new Question("BooleanQuestion, 1.0")
                        {
                          Identifier = "9876", 
                          Data = new Dictionary<string, string>{{"Text" , "Do you have any friends?"}},
                        },
                      new Question("AbQuestion, 1.0")
                        {
                          Identifier = "7654", 
                          Data = new Dictionary<string, string>
                            {
                              {"Text" , "Which is more peppy"},
                              {"Url1" , "http://example.com/1.wav"},
                              {"Url2" , "http://example.com/2.wav"}
                            }
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("BooleanQuestion, 1.0")
                        {
                          Identifier = "6543", 
                          Data = new Dictionary<string, string>{{"Text" , "What if this was the last question?"}},
                        }
                    }
                }
            }
      });

      portalApplication.AddBinding(typeof(AnswerDto), new JsonBinding<AnswerDto>());

      portalApplication.MapRoute("/v6/Questionnaire", () => new Api.Endpoints.QuestionnaireExtension(portalApplication));
      portalApplication.MapRoute("/v6/Answer", () => new Api.Endpoints.AnswerExtension(portalApplication));
    }
  }
}
