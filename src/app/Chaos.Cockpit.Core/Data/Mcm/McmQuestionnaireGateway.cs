using System;
using System.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Mcm.Data;

namespace Chaos.Cockpit.Core.Data.Mcm
{
  public class McmQuestionnaireGateway : IQuestionnaireGateway
  {
    public IMcmRepository Repository { get; set; }

    public McmQuestionnaireGateway(IMcmRepository repository)
    {
      Repository = repository;
    }

    public Questionnaire Set(Questionnaire entity)
    {
      throw new System.NotImplementedException();
    }

    public Questionnaire Get(Guid id)
    {
      var obj = Repository.ObjectGet(id, true);
      
      if(obj == null || obj.ObjectTypeID != CockpitContext.Config.ExperimentObjectTypeId)
        throw new ArgumentException("No Questionaire found by that Id");

      var metadata = obj.Metadatas.SingleOrDefault(m => m.MetadataSchemaGuid == CockpitContext.Config.ExperimentMetadataSchemaId);
      
      if(metadata == null)
        throw new Exception("Questionaire found, but data is corrupt");

      return new DtuFormatConverter().Deserialize(metadata.MetadataXml);
    }

    public Questionnaire GetByQuestionId(string identifier)
    {
      throw new System.NotImplementedException();
    }
  }
}