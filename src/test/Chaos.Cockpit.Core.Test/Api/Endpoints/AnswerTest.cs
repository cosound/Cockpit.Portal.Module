namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  using System.Collections.Generic;
  using Cockpit.Core.Api.Endpoints;
  using Cockpit.Core.Api.Result;
  using Cockpit.Core.Data.InMemory;
  using Core;
  using NUnit.Framework;

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
      var answer = AnswerDto.CreateBooleanAnswer();
      var question = Question.CreateBooleanQuestion();
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
      Assert.That(actual.UserAnswer.Identifier, Is.Not.Null);
    }
  }
}