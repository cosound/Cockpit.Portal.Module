using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core
{
  public class MultiValueValidation
  {
    public string Id { get; set; }
    public uint Min { get; set; }
    public uint Max { get; set; }

    public IList<ComplexValueValidator> ComplexValueValidators { get; set; }
    public IList<SimpleValueValidator> SimpleValueValidators { get; set; }
  }

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
  }

  public class ComplexValueValidator
  {
    public ComplexValueValidator()
    {
      SimpleValueValidators = new List<SimpleValueValidator>();
    }

    public string Id { get; set; }

    public IList<SimpleValueValidator> SimpleValueValidators { get; set; }

    public void Validate(ComplexValue value)
    {
      if (value == null) throw new ValidationException();

      foreach (var validator in SimpleValueValidators)
        validator.Validate(value.GetValue(validator.Id));
    }
  }

  public class ComplexValue
  {
    public ComplexValue()
    {
      SimpleValues = new List<SimpleValue>();
    }

    public IList<SimpleValue> SimpleValues { get; set; }

    public SimpleValue GetValue(string key)
    {
      var val = SimpleValues.FirstOrDefault(item => item.Key == key);

      if (val == null) throw new ValidationException(string.Format("Value ({0}) is missing", key));

      return val;
    }
  }

  public class SimpleValue
  {
    public SimpleValue(string key, string value)
    {
      Key = key;
      Value = value;
    }

    public string Key { get; set; }
    public string Value { get; set; }
  }
}
