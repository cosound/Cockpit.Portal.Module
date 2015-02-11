using System.Collections.Generic;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core.Validation
{
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
}