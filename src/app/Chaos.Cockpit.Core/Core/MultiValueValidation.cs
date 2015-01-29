using System.Text.RegularExpressions;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core
{
  public class MultiValueValidation
  {
    public string Id { get; set; }
    public uint Min { get; set; }
    public uint Max { get; set; }

    public SimpleValue Sv { get; set; }

    public class SimpleValue
    {
      public string Id { get; set; }
      public string Validation { get; set; }

      public void Validate(object o)
      {
        if(o == null) throw new ValidationException();
        if (!Regex.IsMatch(o.ToString(), Validation, RegexOptions.Singleline)) throw new ValidationException();
      }
    }
  }
}
