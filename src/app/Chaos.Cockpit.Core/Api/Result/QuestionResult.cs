using CHAOS.Serialization;

namespace Chaos.Cockpit.Core.Api.Result
{
  public class QuestionResult
  {
    [Serialize("Id")]
    public string Identifier { get; set; }

    [Serialize]
    public Input Inputs { get; set; }

    [Serialize]
    public Output Outputs { get; set; }

    public class Input
    {

    }
  }
}
