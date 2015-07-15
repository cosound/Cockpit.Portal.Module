using System;
using System.Linq;
using CHAOS.Serialization.Standard;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Dto;
using Chaos.Portal.Core.Exceptions;

namespace Chaos.Cockpit.Core.Data.Mcm
{
  public class McmSelectionGateway : ISelectionGateway
  {
    public IMcmRepository McmRepository { get; set; }

    public McmSelectionGateway(IMcmRepository mcmRepository)
    {
      McmRepository = mcmRepository;
    }

    public Selection Set(Selection selection)
    {
      Guid metadataGuid;

      if (string.IsNullOrEmpty(selection.Id))
      {
        selection.Id = Guid.NewGuid().ToString();
        McmRepository.ObjectCreate(Guid.Parse(selection.Id), Context.Config.SelectionObjectTypeId, Context.Config.SelectionFolderId);

        metadataGuid = Guid.NewGuid();
      }
      else
      {
        if (selection.Items == null)
        {
          var fromDb = Get(selection.Id);
          selection.Items = fromDb.Items;
        }

        metadataGuid = GetSelectionMetadata(selection.Id).Guid;
      }

      var xml = SerializerFactory.XMLSerializer.Serialize(selection);

      // todo fix revisionid 0
      McmRepository.MetadataSet(Guid.Parse(selection.Id), metadataGuid, Context.Config.SelectionMetadataSchemaId, null, 0, xml, Guid.Empty);

      return selection;
    }

    public Selection Get(string id)
    {
      var metadata = GetSelectionMetadata(id);

      var name = metadata.MetadataXml.Root.Element("Name").Value;
      var items = metadata.MetadataXml.Descendants("Item").Select(item => new Item{Id = item.Element("Id").Value});

      return new Selection
        {
          Id = id,
          Name = name,
          Items = items.ToList()
        };
    }

    private Metadata GetSelectionMetadata(string id)
    {
      var obj = McmRepository.ObjectGet(Guid.Parse(id), true);

      if (obj == null)
        throw new DataNotFoundException("No data found with Id: " + id, "Requested Selection was not found");

      var metadata =
        obj.Metadatas.SingleOrDefault(item => item.MetadataSchemaGuid == Context.Config.SelectionMetadataSchemaId);

      if (metadata == null)
        throw new ServerException("Metadata missing on selection object", "System error");

      return metadata;
    }

    public bool Delete(string id)
    {
      return McmRepository.ObjectDelete(Guid.Parse(id)) == 1;
    }
  }
}