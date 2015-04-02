using System;
using System.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Validation;
using Chaos.Cockpit.Core.Data;
using Chaos.Cockpit.Core.Data.Mcm;
using Chaos.Mcm.Data;
using Moq;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Data.Mcm
{
  [TestFixture]
  public class McmQuestionGatewayTest
  {
    private Mock<IMcmRepository> Mcm;

    [SetUp]
    public void SetUp()
    {
      Mcm = new Mock<IMcmRepository>();
      Context.QuestionnaireGateway = new McmQuestionnaireGateway(Mcm.Object);
    }

    [Test, ExpectedException(typeof (ArgumentException))]
    public void Get_QuestionaireDoesntExist_Throw()
    {
      var gateway = new McmQuestionGateway(Mcm.Object);

      gateway.Get("6a0fae3a-2ac0-477b-892a-b24433ff3bd2:0");
    }

    [Test, ExpectedException(typeof (ArgumentException), ExpectedMessage = "No question with Id found.")]
    public void Get_QuestionDoesntExist_Throw()
    {
      var gateway = new McmQuestionGateway(Mcm.Object);
      var questionaire = TestResources.Make_QuestionnaireObject();
      Mcm.Setup(m => m.ObjectGet(It.IsAny<Guid>(), true, false, false, false, false)).Returns(questionaire);

      gateway.Get("6a0fae3a-2ac0-477b-892a-b24433ff3bd2:999");
    }

    [Test]
    public void Get_QuestionIdThatExist_Return()
    {
      var gateway = new McmQuestionGateway(Mcm.Object);
      var questionaire = TestResources.Make_QuestionnaireObject();
      var questionId = "6a0fae3a-2ac0-477b-892a-b24433ff3bd2:0";
      Mcm.Setup(m => m.ObjectGet(It.IsAny<Guid>(), true, false, false, false, false)).Returns(questionaire);

      var result = gateway.Get(questionId);

      Assert.That(result.Id, Is.EqualTo(questionId));
      Assert.That(result.Type, Is.EqualTo("Monitor"));
    }

    [Test, ExpectedException(typeof (ArgumentException))]
    public void Save_QuestionaireDoesntExist_Throw()
    {
      var gateway = new McmQuestionGateway(Mcm.Object);
      var question = new Question("Test") {Id = "10000000-0000-0000-0000-000000000001:0"};

      gateway.Save(question);
    }

    [Test, ExpectedException(typeof (ArgumentException), ExpectedMessage = "No question with Id found.")]
    public void Save_QuestionDoesntExist_Throw()
    {
      var gateway = new McmQuestionGateway(Mcm.Object);
      var questionaire = TestResources.Make_QuestionnaireObject();
      var question = new Question("Test") {Id = "10000000-0000-0000-0000-000000000001:999"};
      Mcm.Setup(m => m.ObjectGet(It.IsAny<Guid>(), true, false, false, false, false)).Returns(questionaire);

      gateway.Save(question);
    }

    [Test]
    public void Save_QuestionThatExist_UpdateDatabaseAndReturn()
    {
      var questionireGateway = new Mock<IQuestionnaireGateway>();
      Context.QuestionnaireGateway = questionireGateway.Object;
      var gateway = new McmQuestionGateway(Mcm.Object);
      var questionnaire = TestResources.Make_Questionnaire();
      var question = new Question("Test")
        {
          Id = "6a0fae3a-2ac0-477b-892a-b24433ff3bd2:4",
          Output =
            new Output
              {
                SimpleValues  = new []
                  {
                    new SimpleValue("Text", "Mars")
                  }
              }
        };
      questionireGateway.Setup(m => m.Get(It.IsAny<Guid>())).Returns(questionnaire);

      var result = gateway.Save(question);

      Assert.That(result.Output.SimpleValues.First().Key, Is.EqualTo("Text"));
      Assert.That(result.Output.SimpleValues.First().Value, Is.EqualTo("Mars"));
      questionireGateway.Verify(m => m.Set(It.IsAny<Questionnaire>()));
    }
  }
}