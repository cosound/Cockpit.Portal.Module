namespace Chaos.Cockpit.Core.Api.Result
{
  using System.Collections.Generic;
  using CHAOS.Serialization;

  public class SlideDto
  {
    [Serialize("Questions")]
    public IList<QuestionDto> Questions { get; set; }

    public SlideDto()
    {
      Questions = new List<QuestionDto>();
    }
  }
}