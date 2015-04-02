using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.Mcm;
using Chaos.Mcm.Data.Dto;
using Object = Chaos.Mcm.Data.Dto.Object;

static internal class TestResources
{
  public static Object Make_QuestionnaireObject()
  {
    return new Object
      {
        Guid = new Guid("6a0fae3a-2ac0-477b-892a-b24433ff3bd2"),
        ObjectTypeID = Context.Config.ExperimentObjectTypeId,
        Metadatas = new List<Metadata>()
          {
            new Metadata
              {
                MetadataSchemaGuid = Context.Config.ExperimentMetadataSchemaId,
                MetadataXml = XDocument.Load("Resources\\experiment2.xml")
              }
          }
      };
  }
  
  public static Questionnaire Make_Questionnaire()
  {
    return new DtuFormatConverter().Deserialize(XDocument.Load("Resources\\experiment2.xml"));
  }
}