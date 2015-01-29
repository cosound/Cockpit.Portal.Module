using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Core
{
  [TestFixture]
  public class ComplexValueValidatorTest
  {
    [Test, ExpectedException(typeof(ValidationException))]
    public void Validate_GivenNull_Throw()
    {
      var validator = new ComplexValueValidator();

      validator.Validate(null);
    }
    
    [Test, ExpectedException(typeof(ValidationException))]
    public void Validate_GivenComplexValueContainingAnInvalidValue_Throw()
    {
      var validator = new ComplexValueValidator();
      validator.SimpleValueValidations.Add(new SimpleValueValidation());
      var complex = new ComplexValue();
      complex.SimpleValues.Add(new SimpleValue("key", null));

      validator.Validate(complex);
    }
  }
}