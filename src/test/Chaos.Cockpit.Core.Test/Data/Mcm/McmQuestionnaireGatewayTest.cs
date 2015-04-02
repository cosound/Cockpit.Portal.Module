using System;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Validation;
using Chaos.Cockpit.Core.Data.Mcm;
using Chaos.Mcm.Data;
using Moq;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Data.Mcm
{
  [TestFixture]
  public class McmQuestionnaireGatewayTest
  {
    private Mock<IMcmRepository> mcm;

    [SetUp]
    public void SetUp()
    {
      mcm = new Mock<IMcmRepository>();
    }

    private McmQuestionnaireGateway Make_QuestionnaireGateway()
    {
      return new McmQuestionnaireGateway(mcm.Object);
    }

    [Test, ExpectedException(typeof (ArgumentException), ExpectedMessage = "No Questionaire found by that Id")]
    public void Get_IdMatchesWrongObjectType_Throw()
    {
      var obj = new Chaos.Mcm.Data.Dto.Object
        {
          Guid = new Guid("6a0fae3a-2ac0-477b-892a-b24433ff3bd2"),
          ObjectTypeID = 999 // wrong id
        };
      mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

      Make_QuestionnaireGateway().Get(obj.Guid);
    }

    [Test, ExpectedException(typeof (ArgumentException), ExpectedMessage = "No Questionaire found by that Id")]
    public void Get_IdNotFound_Throw()
    {
      Make_QuestionnaireGateway().Get(Guid.NewGuid());
    }

    [Test, ExpectedException(typeof (Exception), ExpectedMessage = "Questionaire found, but data is corrupt")]
    public void Get_ObjectFoundButNoMetadta_Throw()
    {
      var obj = new Chaos.Mcm.Data.Dto.Object
        {
          Guid = new Guid("6a0fae3a-2ac0-477b-892a-b24433ff3bd2"),
          ObjectTypeID = Context.Config.ExperimentObjectTypeId
        };
      mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

      Make_QuestionnaireGateway().Get(obj.Guid);
    }

    [Test]
    public void Get_IdMatchesQuestionaireObject_ReturnQuestionaire()
    {
      var obj = TestResources.Make_QuestionnaireObject();
      mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

      var result = Make_QuestionnaireGateway().Get(obj.Guid);

      Assert.That(result.Id, Is.EqualTo(obj.Guid.ToString()));
    }

    [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "No Questionaire found by that Id")]
    public void Set_IdMatchesWrongObjectType_Throw()
    {
      var obj = new Chaos.Mcm.Data.Dto.Object
      {
        Guid = new Guid("6a0fae3a-2ac0-477b-892a-b24433ff3bd2"),
        ObjectTypeID = 999 // wrong id
      };
      var questionnaire = TestResources.Make_Questionnaire();
      mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

      Make_QuestionnaireGateway().Set(questionnaire);
    }

    [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "No Questionaire found by that Id")]
    public void Set_IdNotFound_Throw()
    {
      var questionnaire = TestResources.Make_Questionnaire();

      Make_QuestionnaireGateway().Set(questionnaire);
    }

    [Test, ExpectedException(typeof(Exception), ExpectedMessage = "Questionaire found, but data is corrupt")]
    public void Set_ObjectFoundButNoMetadta_Throw()
    {
      var obj = new Chaos.Mcm.Data.Dto.Object
      {
        Guid = new Guid("6a0fae3a-2ac0-477b-892a-b24433ff3bd2"),
        ObjectTypeID = Context.Config.ExperimentObjectTypeId
      };
      var questionnaire = TestResources.Make_Questionnaire();
      mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

      Make_QuestionnaireGateway().Set(questionnaire);
    }

    [Test]
    public void Set_QuestionnaireFound_SetMetadata()
    {
      var obj = TestResources.Make_QuestionnaireObject();
      var questionnaire = TestResources.Make_Questionnaire();
      var question = new Question("Test")
      {
        Id = "6a0fae3a-2ac0-477b-892a-b24433ff3bd2:4",
        Output =
          new Output
          {
            SimpleValues = new[] { new SimpleValue("Text", "Mars"), }
          }
      };
      questionnaire.GetQuestion(question.Id).Output = question.Output;
      mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

      var result = Make_QuestionnaireGateway().Set(questionnaire);

      Assert.That(result.Id, Is.EqualTo(questionnaire.Id));
      mcm.Verify(m => m.MetadataSet(obj.Guid, It.IsAny<Guid>(), Context.Config.ExperimentMetadataSchemaId, null, 0, It.IsAny<XDocument>(), It.IsAny<Guid>()));
    }
  }
}