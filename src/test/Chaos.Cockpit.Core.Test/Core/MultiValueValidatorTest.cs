using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Core
{
  [TestFixture]
  public class MultiValueValidatorTest
  {
    [Test, ExpectedException(typeof (ValidationException))]
    public void Validate_GivenNull_Throw()
    {
      var validator = new MultiValueValidator();

      validator.Validate(null);
    }

    [Test, ExpectedException(typeof (ValidationException))]
    public void Validate_GivenInvalidSimpleValue_Throw()
    {
      var validator = new MultiValueValidator();
      validator.SimpleValueValidators.Add(new SimpleValueValidator());
      var multiValue = new MultiValue();

      validator.Validate(multiValue);
    }

    [Test, ExpectedException(typeof (ValidationException))]
    public void Validate_GivenInvalidValue_Throw()
    {
      var validator = new MultiValueValidator();
      validator.SimpleValueValidators.Add(SimpleValueValidator.Create("key", "expected value"));
      var multiValue = new MultiValue();
      multiValue.SimpleValues.Add(new SimpleValue("key", "invalid value"));

      validator.Validate(multiValue);
    }

    [Test]
    public void Validate_GivenValidSimpleValue()
    {
      var validator = new MultiValueValidator();
      validator.SimpleValueValidators.Add(SimpleValueValidator.Create("key", "value"));
      var multiValue = new MultiValue();
      multiValue.SimpleValues.Add(new SimpleValue("key", "value"));

      validator.Validate(multiValue);
    }

    [Test, ExpectedException(typeof(ValidationException))]
    public void Validate_GivenInvalidComplexValue()
    {
      var validator = new MultiValueValidator();
      validator.ComplexValueValidators.Add(new ComplexValueValidator
        {
          SimpleValueValidators = new[] {SimpleValueValidator.Create("key", "expected value")}
        });
      var multiValue = new MultiValue();
      multiValue.ComplexValues.Add(new ComplexValue
        {
          SimpleValues = new[] {new SimpleValue("key", "invalid value")}
        });

      validator.Validate(multiValue);
    }
    
    [Test]
    public void Validate_GivenValidComplexValue()
    {
      var validator = new MultiValueValidator();
      validator.ComplexValueValidators.Add(new ComplexValueValidator
        {
          SimpleValueValidators = new[] {SimpleValueValidator.Create("key", "value")}
        });
      var multiValue = new MultiValue();
      multiValue.ComplexValues.Add(new ComplexValue
        {
          SimpleValues = new[] {new SimpleValue("key", "value")}
        });

      validator.Validate(multiValue);
    }
  }
}