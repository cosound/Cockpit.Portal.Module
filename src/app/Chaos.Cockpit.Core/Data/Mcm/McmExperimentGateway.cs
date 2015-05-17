using System;
using System.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Dto;
using Chaos.Portal.Core.Exceptions;

namespace Chaos.Cockpit.Core.Data.Mcm
{
  public class McmExperimentGateway : IExperimentGateway
  {
    private readonly IMcmRepository _repo;

    public McmExperimentGateway(IMcmRepository repo)
    {
      _repo = repo;
    }

    public ClaimedList Save(ClaimedList entity)
    {
      var objectGuid = Guid.Parse(entity.Id);
      var metadata = Metadata(objectGuid);

      _repo.MetadataSet(objectGuid, metadata.Guid, metadata.MetadataSchemaGuid, metadata.LanguageCode, 0, entity.ToXml(), metadata.EditingUserGuid);

      return entity;
    }

    private Metadata Metadata(Guid objectGuid)
    {
      var result = _repo.ObjectGet(objectGuid, includeMetadata: true);

      var metadata = result.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == Guid.Parse("00000000-0000-0000-0000-000000000111"));

      if (metadata == null)
        throw new ServerException("Id does not match a valid List object",
                                  "Server error, please contact the Administrator if it appears again.");
      return metadata;
    }

    public ClaimedList Get(string id)
    {
      var objectGuid = Guid.Parse(id);
      var metadata = Metadata(objectGuid);

      return new ClaimedList(id, metadata.MetadataXml);
    }
  }
}