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

    public IList<SimpleValueValidation> SimpleValueValidators { get; set; }
  }

  public class SimpleValueValidation
  {
    public string Id { get; set; }
    public string Validation { get; set; }

    public void Validate(SimpleValue value)
    {
      if(value == null || value.Value == null) throw new ValidationException();
      if (!Regex.IsMatch(value.Value, Validation, RegexOptions.Singleline)) throw new ValidationException();
    }
  }

  public class ComplexValueValidator
  {
    public ComplexValueValidator()
    {
      SimpleValueValidations = new List<SimpleValueValidation>();
    }

    public string Id { get; set; }

    public IList<SimpleValueValidation> SimpleValueValidations { get; set; }

    public void Validate(ComplexValue value)
    {
      if (value == null) throw new ValidationException();

      SimpleValueValidations.First().Validate(value.SimpleValues.First());
    }
  }

  public class ComplexValue
  {
    public ComplexValue()
    {
      SimpleValues = new List<SimpleValue>();
    }

    public IList<SimpleValue> SimpleValues { get; set; }
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
