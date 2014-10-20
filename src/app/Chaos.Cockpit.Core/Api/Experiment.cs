namespace Chaos.Cockpit.Core.Api
{
  using System.Collections.Generic;
  using Result;

  public class Experiment
  {
    public IList<SlideResult> Screens { get; set; }

    public Experiment()
    {
      Screens = new List<SlideResult>();
    }
  }
}