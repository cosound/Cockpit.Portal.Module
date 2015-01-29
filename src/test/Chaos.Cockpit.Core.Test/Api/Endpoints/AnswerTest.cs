using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  using System.Collections.Generic;
  using Cockpit.Core.Api.Endpoints;
  using Cockpit.Core.Api.Result;
  using NUnit.Framework;

  [TestFixture]
  public class AnswerTest : TestBase
  {
    [Test]
    public void Set_GivenNewAnswer_SetAnswerOnQuestion()
    {
      var extension = new AnswerExtension(PortalApplication.Object);
      var answer = new AnswerDto("TestAnswerType");
      var question = new Question("TestQuestion");
      CockpitContext.QuestionnaireGateway.Set(new Questionnaire
        {
          Name = "Test",
          Slides = new List<Slide>
            {
              new Slide
                {
                  Questions = new List<Question>
                    {
                      question
                    }
                }
            }
        });

      extension.Set(question.Id, answer);

      var actual = CockpitContext.QuestionGateway.Get(question.Id);
      Assert.That(actual.UserAnswer.Id, Is.Not.Null);
    }
  }
}