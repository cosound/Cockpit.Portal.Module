namespace Chaos.Cockpit.Core.Main
{
  using System.Collections.Generic;
  using Api.Result;
  using Core;
  using Portal.Core;
  using Portal.Core.Module;
  using Questionnaire = Api.Endpoints.Questionnaire;

  public class CockpitModule : IModuleConfig
  {
    public void Load(IPortalApplication portalApplication)
    {
      CockpitContext.QuestionnaireGateway = new Data.InMemory.QuestionnaireGateway();

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
                      new BooleanQuestion
                        {
                          Identifier = "1234", Value = "Do you have any friends?"
                        },
                      new BooleanQuestion
                        {
                          Identifier = "2345", Value = "What about imaginary ones?"
                        },
                      new AbQuestion
                        {
                          Identifier = "3456", Text = "Which is more peppy", Url1 = "http://example.com/1.wav", Url2 = "http://example.com/2.wav"
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new BooleanQuestion
                        {
                          Identifier = "4567", Value = "What if this was the last question?"
                        }
                    }
                }
            }
        });
      CockpitContext.QuestionnaireGateway.Save(new Core.Questionnaire
        {
          Identifier = "ab12",
          Name = "Sample QuestionnaireResult 2",
          Slides = new List<Slide>()
            {
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new BooleanQuestion
                        {
                          Identifier = "9876", Value = "Do you have any friends?"
                        },
                      new BooleanQuestion
                        {
                          Identifier = "8765", Value = "What about imaginary ones?"
                        },
                      new AbQuestion
                        {
                          Identifier = "7654", Text = "Which is more peppy", Url1 = "http://example.com/1.wav", Url2 = "http://example.com/2.wav"
                        }
                    }
                },
              new Slide
                {
                  Questions = new List<Question>()
                    {
                      new BooleanQuestion
                        {
                          Identifier = "6543", Value = "What if this was the last question?"
                        }
                    }
                }
            }
        });

      portalApplication.MapRoute("/v6/QuestionnaireResult", () => new Questionnaire(portalApplication));
    }
  }
}
