using System.Collections.Generic;

namespace Chaos.Cockpit.Core.Core.Validation
{
  public class MultiValue
  {
    public MultiValue()
    {
      ComplexValues = new List<ComplexValue>();
      SimpleValues = new List<string>();
    }

    public string Key { get; set; }
    public IList<ComplexValue> ComplexValues { get; set; }
    public IList<string> SimpleValues { get; set; }
  }
}