using System;
using System.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Mcm.Data;
using Object = Chaos.Mcm.Data.Dto.Object;

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
      var id = Guid.Parse(entity.Id);
      var obj = GetQuestionnaireObject(id);
      var metadata = obj.Metadatas.SingleOrDefault(m => m.MetadataSchemaGuid == Context.Config.ExperimentMetadataSchemaId);

      var xml = new DtuFormatConverter().Serialize(entity);

      Repository.MetadataSet(id, metadata.Guid, metadata.MetadataSchemaGuid, metadata.LanguageCode, 0, xml, metadata.EditingUserGuid);

      return Get(id);
    }

    public Questionnaire Get(Guid id)
    {
      var obj = GetQuestionnaireObject(id);
      var metadata = obj.Metadatas.Single(m => m.MetadataSchemaGuid == Context.Config.ExperimentMetadataSchemaId);

      return new DtuFormatConverter().Deserialize(metadata.MetadataXml);
    }

    private Object GetQuestionnaireObject(Guid id)
    {
      var obj = Repository.ObjectGet(id, true);

      if (obj == null || obj.ObjectTypeID != Context.Config.ExperimentObjectTypeId)
        throw new ArgumentException("No Questionaire found by that Id");

      if (obj.Metadatas.All(m => m.MetadataSchemaGuid != Context.Config.ExperimentMetadataSchemaId))
        throw new Exception("Questionaire found, but data is corrupt");

      return obj;
    }

    public Questionnaire GetByQuestionId(string identifier)
    {
      throw new System.NotImplementedException();
    }
  }
}