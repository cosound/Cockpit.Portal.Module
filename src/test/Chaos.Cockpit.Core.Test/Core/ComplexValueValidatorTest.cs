using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Core
{
  [TestFixture]
  public class ComplexValueValidatorTest
  {
    private ComplexValueValidator _validator;

    [SetUp]
    public void SetUp()
    {
      _validator = new ComplexValueValidator();
    }

    [Test, ExpectedException(typeof(ValidationException))]
    public void Validate_GivenNull_Throw()
    {
      _validator.Validate(null);
    }

    [Test, ExpectedException(typeof(ValidationException))]
    public void Validate_GivenComplexValueContainingAnInvalidValue_Throw()
    {
      _validator.SimpleValueValidators.Add(new SimpleValueValidator());
      var complex = new ComplexValue();
      complex.SimpleValues.Add(new SimpleValue("key", null));

      _validator.Validate(complex);
    }

    [Test, ExpectedException(typeof(ValidationException))]
    public void Validate_GivenValueWithUnknownKey_Throw()
    {
      _validator.SimpleValueValidators.Add(new SimpleValueValidator{Id = "key", Validation = "^.*$"});
      var complex = new ComplexValue();
      complex.SimpleValues.Add(new SimpleValue("unknown key", "valid value"));

      _validator.Validate(complex);
    }
    
    [Test]
    public void Validate_GivenComplexValueContainingAnValidValue()
    {
      _validator.SimpleValueValidators.Add(new SimpleValueValidator{Id = "key", Validation = "^.*$"});
      var complex = new ComplexValue();
      complex.SimpleValues.Add(new SimpleValue("key", "valid value"));

      _validator.Validate(complex);
    }
    
    [Test]
    public void Validate_GivenTwoValues_ValidateWithCorrectSpecification()
    {
      _validator.SimpleValueValidators.Add(new SimpleValueValidator{Id = "key1", Validation = "^.*$"});
      _validator.SimpleValueValidators.Add(new SimpleValueValidator{Id = "key2", Validation = "^.*$"});
      var complex = new ComplexValue();
      complex.SimpleValues.Add(new SimpleValue("key1", "valid value"));
      complex.SimpleValues.Add(new SimpleValue("key2", "valid value"));

      _validator.Validate(complex);
    }
  }
}