using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.InMemory;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  using System.Collections.Generic;

  [TestFixture]
  public class AnswerTest : TestBase
  {
    [SetUp]
    public void SetUp()
    {
      base.SetUp();

      CockpitContext.QuestionGateway = new QuestionGateway();
      CockpitContext.QuestionnaireGateway = new QuestionnaireGateway();
    }

    [Test]
    public void Set_GivenNewAnswer_SetAnswerOnQuestion()
    {
      var extension = new AnswerExtension(PortalApplication.Object);
      var answer = new DummyAnswerResultResult {Identifier = "id"};
      var question = new Question {Identifier = "id"};
      CockpitContext.QuestionnaireGateway.Save(new Core.Questionnaire
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

      extension.Set(question.Identifier, answer);

      var actual = CockpitContext.QuestionGateway.Get(question.Identifier);
      Assert.That(actual.UserAnswer.Identifier, Is.EqualTo(answer.Identifier));
    }
  }

  public class DummyAnswerResultResult : Cockpit.Core.Api.Result.AnswerResult
  {
  }
}