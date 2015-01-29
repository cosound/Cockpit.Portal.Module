namespace Chaos.Cockpit.Core.Main
{
  using System.Collections.Generic;
  using Api.Binding;
  using Api.Result;
  using Core;
  using Newtonsoft.Json.Linq;
  using Portal.Core;
  using Portal.Core.Module;

  public class CockpitModule : IModuleConfig
  {
    public void Load(IPortalApplication portalApplication)
    {
      CockpitContext.QuestionnaireGateway = new Data.InMemory.QuestionnaireGateway();
      CockpitContext.QuestionGateway = new Data.InMemory.QuestionGateway();

      var question = new Core.Questionnaire
        {
          Id = "a12f", Name = "Sample QuestionnaireResult", Slides = new List<Slide>()
            {
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new Question("introductions_r001")
                        {
                          Id = "1234", Data = new Dictionary<string, object>
                            {
                              {"labelHeader", "Survey about life, the universe, and everything"}, {"textContent", "Please answer these 42 quick questions"}, {"imageObject", "http://upload.wikimedia.org/wikipedia/commons/5/56/Answer_to_Life.png"}
                            }
                        },
                      new Question("Monitor:Event:StartAtDateTime")
                        {
                          Id = "12345"
                        },
                      new Question("Monitor:Event:EndAtDateTime")
                        {
                          Id = "123456"
                        },
                      new Question("Monitor:Event:EndAtDateTime")
                        {
                          Id = "123456"
                        },
                      new Question("RadioButtonGroup")
                        {
                          Id = "RadioButtonGroup0", Data = new Dictionary<string, object>
                            {
                              {"Url", "http://example.com/1.wav"}, {"Label", "How much do you like this survey?"}, {"Items", new[] {new JValue("a lot"), new JValue("kinda"), new JValue("not really")}}
                            }
                        },
                      new Question("ContinousScale")
                        {
                          Id = "ContinousScale0", Data = new Dictionary<string, object>
                            {
                              {"Url", "http://example.com/1.wav"}, {"Label", "How much do you like this survey?"}, {"Items", new[] {"a lot", "kinda", "not really"}}
                            }
                        },
                      new Question("DropDown")
                        {
                          Id = "DropDown0", Data = new Dictionary<string, object>
                            {
                              {"Url", "http://example.com/1.wav"}, {"Label", "Where do you think fish can survive"}, {"Items", new[] {"Water", "Land", "Space", "Fish are imaginary"}}, {"MinNoOfSelections", "1"}, {"MaxNoOfSelections", "3"}
                            }
                        },
                      new Question("Response:Freetext")
                        {
                          Id = "1234567", Data = new Dictionary<string, object>
                            {
                              {"Value", "Very interesting text block"},
                            }
                        },
                      new Question("BooleanQuestion, 1.0")
                        {
                          Id = "2345", Data = new Dictionary<string, object> {{"Text", "Do you have any friends?"}},
                        },
                      new Question("AbQuestion, 1.0")
                        {
                          Id = "3456", Data = new Dictionary<string, object>
                            {
                              {"Text", "Which is more peppy"}, {"Url1", "http://example.com/1.wav"}, {"Url2", "http://example.com/2.wav"}
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
                          Id = "4567", Data = new Dictionary<string, object> {{"Text", "What if this was the last question?"}},
                        }
                    }
                }
            }
        };
      CockpitContext.QuestionnaireGateway.Set(question);

      portalApplication.AddBinding(typeof(AnswerDto), new JsonBinding<AnswerDto>());

      portalApplication.MapRoute("/v6/Question", () => new Api.Endpoints.QuestionExtension(portalApplication));
      portalApplication.MapRoute("/v6/Answer", () => new Api.Endpoints.AnswerExtension(portalApplication));
    }
  }
}
