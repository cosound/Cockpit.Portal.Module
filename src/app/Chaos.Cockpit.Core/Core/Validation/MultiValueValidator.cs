using System.Linq;
using Chaos.Cockpit.Core.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core.Validation
{
  public class MultiValueValidator
  {
    public string Id { get; set; }
    public uint Min { get; set; }
    public uint Max { get; set; }

    public ComplexValueValidator ComplexValueValidator { get; set; }
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

      if (ComplexValueValidator != null)
      {
        var vals = multiValue.ComplexValues.Where(item => item.Key == ComplexValueValidator.Id);

        if (!vals.Any()) throw new ValidationException(string.Format("Value ({0}) is missing", ComplexValueValidator.Id));

        foreach (var val in vals)
          ComplexValueValidator.Validate(val);
      }
    }
  }
}
