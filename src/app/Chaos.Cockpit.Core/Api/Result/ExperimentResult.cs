using CHAOS.Serialization;
using Chaos.Portal.Core.Data.Model;

namespace Chaos.Cockpit.Core.Api.Result
{
  public class ExperimentResult : AResult
  {
    [Serialize]
    public string Id { get; set; }

    [Serialize]
    public string ClaimedOnDate { get; set; }
  }
}