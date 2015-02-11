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
}
