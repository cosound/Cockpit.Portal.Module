using System.Text.RegularExpressions;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core.Validation
{
  public class SimpleValueValidator
  {
    public string Id { get; set; }
    public string Validation { get; set; }

    public void Validate(SimpleValue value)
    {
      if(value == null || value.Value == null) throw new ValidationException();
      if(IsInvalid(value)) throw new ValidationException();
    }

    private bool IsInvalid(SimpleValue value)
    {
      return !Regex.IsMatch(value.Value, Validation, RegexOptions.Singleline);
    }

    public static SimpleValueValidator Create(string id, string validation)
    {
      return new SimpleValueValidator
        {
          Id = id,
          Validation = validation
        };
    }
  }
}