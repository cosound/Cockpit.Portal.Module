using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core
{
  public class MultiValueValidator
  {
    public MultiValueValidator()
    {
      ComplexValueValidators = new List<ComplexValueValidator>();
      SimpleValueValidators = new List<SimpleValueValidator>();
    }

    public string Id { get; set; }
    public uint Min { get; set; }
    public uint Max { get; set; }

    public IList<ComplexValueValidator> ComplexValueValidators { get; set; }
    public IList<SimpleValueValidator> SimpleValueValidators { get; set; }

    public void Validate(MultiValue multiValue)
    {
      if(multiValue == null) throw new ValidationException();

      foreach (var validator in SimpleValueValidators)
      {
        var vals = multiValue.SimpleValues.Where(item => item.Key == validator.Id);

        if (!vals.Any()) throw new ValidationException(string.Format("Value ({0}) is missing", validator.Id));

        foreach (var val in vals)
          validator.Validate(val);
      }

      foreach (var validator in ComplexValueValidators)
      {
        var vals = multiValue.ComplexValues.Where(item => item.Key == validator.Id);

        if (!vals.Any()) throw new ValidationException(string.Format("Value ({0}) is missing", validator.Id));

        foreach (var val in vals)
          validator.Validate(val);
      }
    }
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

    public static SimpleValueValidator Create(string id, string validation)
    {
      return new SimpleValueValidator
        {
          Id = id,
          Validation = validation
        };
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
