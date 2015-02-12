using System.Text.RegularExpressions;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core.Validation
{
  public class SimpleValueValidator
  {
    public string Id { get; set; }
    public string Validation { get; set; }

    public void Validate(string value)
    {
      if(value == null) throw new ValidationException();
      if(IsInvalid(value)) throw new ValidationException();
    }
    
    public void Validate(SimpleValue value)
    {
      if(value == null || value.Value == null || value.Key == null) throw new ValidationException();
      if(IsInvalid(value.Value)) throw new ValidationException();
    }

    private bool IsInvalid(string value)
    {
      return !Regex.IsMatch(value, Validation, RegexOptions.Singleline);
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