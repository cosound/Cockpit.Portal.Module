using Chaos.Cockpit.Core.Core.Exceptions;
using Chaos.Cockpit.Core.Core.Validation;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Core.Validation
{
  [TestFixture]
  public class SimpleValueValidatorTest
  {
    private SimpleValueValidator _validator;

    [SetUp]
    public void SetUp()
    {
      _validator = new SimpleValueValidator();
      _validator.Id = "key";
    }

    [Test, ExpectedException(typeof (ValidationException))]
    public void Validate_GivenNull_Throw()
    {
      _validator.Validate((SimpleValue) null);
    }

    [Test, ExpectedException(typeof (ValidationException))]
    public void Validate_GivenInvalidValue_Throw()
    {
      _validator.Validation = "Expected value";

      _validator.Validate(new SimpleValue("key", "Wrong value"));
    }
    
    [TestCase("", "")]
    [TestCase("", ".*")]
    [TestCase("some value", "^.*$")]
    public void Validate_GivenValidValue(string value, string validation)
    {
      _validator.Validation = validation;

      _validator.Validate(new SimpleValue("key", value));
    }
  }
}