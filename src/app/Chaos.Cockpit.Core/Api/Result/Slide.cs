namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;

  public class Slide
  {
    [Serialize("Questions")]
    public IList<Question> Questions { get; set; }

    public Slide()
    {
      Questions = new List<Question>();
    }
  }
}