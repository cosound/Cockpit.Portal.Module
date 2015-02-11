using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core.Validation
{
  public class MultiValueValidator
  {
    public MultiValueValidator()
    {
      ComplexValueValidators = new List<ComplexValueValidator>();
    }

    public string Id { get; set; }
    public uint Min { get; set; }
    public uint Max { get; set; }

    public IList<ComplexValueValidator> ComplexValueValidators { get; set; }
    public SimpleValueValidator SimpleValueValidators { get; set; }

    public void Validate(MultiValue multiValue)
    {
      if(multiValue == null) throw new ValidationException();

      if (SimpleValueValidators != null)
      {
        var vals = multiValue.SimpleValues.Where(item => item.Key == SimpleValueValidators.Id);

        if (!vals.Any()) throw new ValidationException(string.Format("Value ({0}) is missing", SimpleValueValidators.Id));

        foreach (var val in vals)
          SimpleValueValidators.Validate(val);
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
}
