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
    public SimpleValueValidator SimpleValueValidator { get; set; }

    public void Validate(MultiValue multiValue)
    {
      if(multiValue == null) throw new ValidationException();

      if (SimpleValueValidator != null)
      {
        var vals = multiValue.SimpleValues;

        if (!vals.Any()) throw new ValidationException(string.Format("Value ({0}) is missing", SimpleValueValidator.Id));

        foreach (var val in vals)
          SimpleValueValidator.Validate(val);
      }
      else if (ComplexValueValidator != null)
      {
        foreach (var val in multiValue.ComplexValues)
          ComplexValueValidator.Validate(val);
      }
    }
  }
}
