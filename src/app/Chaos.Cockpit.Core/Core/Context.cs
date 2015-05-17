using System;
using Chaos.Cockpit.Core.Data;

namespace Chaos.Cockpit.Core.Core
{
  public static class Context
  {
    static Context()
    {
      Config = new Config
        {
          ExperimentObjectTypeId = 775,
          ExperimentMetadataSchemaId = Guid.Parse("ffffffff-ffff-ffff-ffff-fff775000001"),
          SelectionMetadataSchemaId = Guid.Parse("1fffffff-ffff-ffff-ffff-fff775000001"),
          ClaimListMetadataSchemaGuid = Guid.Parse("00000000-0000-0000-0000-000000000111")
        };
    }

    public static IQuestionnaireGateway QuestionnaireGateway;
    public static IQuestionGateway QuestionGateway;
    public static ISelectionGateway SelectionGateway;
    public static IExperimentGateway ExperimentGateway;
    public static Config Config { get; set; }
  }

  public interface IExperimentGateway
  {
    ClaimedList Save(ClaimedList entity);
    ClaimedList Get(string id);
  }

  public class Config
  {
    public uint ExperimentObjectTypeId { get; set; }

    public Guid ExperimentMetadataSchemaId { get; set; }

    public Guid SelectionMetadataSchemaId { get; set; }

    public uint SelectionObjectTypeId { get; set; }

    public uint SelectionFolderId { get; set; }

    public Guid ClaimListMetadataSchemaGuid { get; set; }
  }
}