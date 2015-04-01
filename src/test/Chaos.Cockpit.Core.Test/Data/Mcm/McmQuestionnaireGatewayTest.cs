using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.Mcm;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Dto;
using Moq;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Data.Mcm
{
  [TestFixture]
  public class McmQuestionnaireGatewayTest 
  {
     [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "No Questionaire found by that Id")]
     public void Get_IdMatchesWrongObjectType_Throw()
     {
       var mcm = new Mock<IMcmRepository>();
       var gateway = new McmQuestionnaireGateway(mcm.Object);
       var obj = new Chaos.Mcm.Data.Dto.Object
         {
           Guid = new Guid("10000000-0000-0000-0000-000000000001"),
           ObjectTypeID = 999 // wrong id
         };
       mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

       gateway.Get(obj.Guid);
     }

     [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "No Questionaire found by that Id")]
     public void Get_IdNotFound_Throw()
     {
       var gateway = new McmQuestionnaireGateway(new Mock<IMcmRepository>().Object);

       gateway.Get(Guid.NewGuid());
     }

     [Test, ExpectedException(typeof(Exception), ExpectedMessage = "Questionaire found, but data is corrupt")]
     public void Get_ObjectFoundButNoMetadta_Throw()
     {
       var mcm = new Mock<IMcmRepository>();
       var gateway = new McmQuestionnaireGateway(mcm.Object);
       var obj = new Chaos.Mcm.Data.Dto.Object
       {
         Guid = new Guid("10000000-0000-0000-0000-000000000001"),
         ObjectTypeID = CockpitContext.Config.ExperimentObjectTypeId
       };
       mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

       gateway.Get(obj.Guid);
     }

     [Test]
     public void Get_IdMatchesQuestionaireObject_ReturnQuestionaire()
     {
       var mcm = new Mock<IMcmRepository>();
       var gateway = new McmQuestionnaireGateway(mcm.Object);
       var obj = new Chaos.Mcm.Data.Dto.Object
       {
         Guid = new Guid("10000000-0000-0000-0000-000000000001"),
         ObjectTypeID = CockpitContext.Config.ExperimentObjectTypeId,
         Metadatas = new List<Metadata>()
           {
             new Metadata
               {
                 MetadataSchemaGuid = CockpitContext.Config.ExperimentMetadataSchemaId,
                 MetadataXml = XDocument.Load("Ressources\\experiment2.xml")
               }
           }
       };
       mcm.Setup(m => m.ObjectGet(obj.Guid, true, false, false, false, false)).Returns(obj);

       var result = gateway.Get(obj.Guid);

       Assert.That(result.Id, Is.EqualTo(obj.Guid.ToString()));
     }
  }
}