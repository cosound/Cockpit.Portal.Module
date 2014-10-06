namespace Chaos.Cockpit.Core
{
  using System.Collections.Generic;

  public class Experiment
  {
    public IList<Screen> Screens { get; set; }

    public Experiment()
    {
      Screens = new List<Screen>();
    }
  }
}