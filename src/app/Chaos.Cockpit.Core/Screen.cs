namespace Chaos.Cockpit.Core
{
  using System.Collections.Generic;

  public class Screen
  {
    public IList<Question> Questions { get; set; }

    public Screen()
    {
      Questions = new List<Question>();
    }
  }
}