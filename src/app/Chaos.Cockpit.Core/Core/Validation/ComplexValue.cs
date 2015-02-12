using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core.Validation
{
  public class ComplexValue
  {
    public ComplexValue()
    {
      SimpleValues = new List<SimpleValue>();
    }

    public string Key { get; set; }
    public IList<SimpleValue> SimpleValues { get; set; }

    public SimpleValue GetValue(string key)
    {
      var val = SimpleValues.FirstOrDefault(item => item.Key == key);

      if (val == null) throw new ValidationException(string.Format("Value ({0}) is missing", key));

      return val;
    }

    public void Add(SimpleValue value)
    {
      SimpleValues.Add(value);
    }
  }
}