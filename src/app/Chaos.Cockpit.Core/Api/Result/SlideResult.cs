namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;

  public class SlideResult
  {
    [Serialize("Questions")]
    public IList<QuestionResult> Questions { get; set; }

    public SlideResult()
    {
      Questions = new List<QuestionResult>();
    }
  }
}