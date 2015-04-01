using System;
using Chaos.Cockpit.Core.Data;

namespace Chaos.Cockpit.Core.Core
{
  public static class CockpitContext
  {
    static CockpitContext()
    {
      Config = new CockpitConfig
        {
          ExperimentObjectTypeId = 775,
          ExperimentMetadataSchemaId = Guid.Parse("ffffffff-ffff-ffff-ffff-fff775000001")
        };
    }

    public static IQuestionnaireGateway QuestionnaireGateway;
    public static IQuestionGateway QuestionGateway;
    public static CockpitConfig Config { get; set; }
  }

  public class CockpitConfig
  {
    public uint ExperimentObjectTypeId { get; set; }

    public Guid ExperimentMetadataSchemaId { get; set; }
  }
}