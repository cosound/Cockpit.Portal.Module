using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Core
{
  [TestFixture]
  public class SimpleValueValidationTest
  {
    private SimpleValueValidation _validator;

    [SetUp]
    public void SetUp()
    {
      _validator = new SimpleValueValidation();
    }

     [Test, ExpectedException(typeof(ValidationException))]
     public void Validate_GivenNull_Throw()
     {
       _validator.Validate(null);
     }

    [Test, ExpectedException(typeof(ValidationException))]
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