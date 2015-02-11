using System.Collections.Generic;

namespace Chaos.Cockpit.Core.Core.Validation
{
  public class MultiValue
  {
    public MultiValue()
    {
      ComplexValues = new List<ComplexValue>();
      SimpleValues = new List<SimpleValue>();
    }

    public string Key { get; set; }
    public IList<ComplexValue> ComplexValues { get; set; }
    public IList<SimpleValue> SimpleValues { get; set; }
  }
}