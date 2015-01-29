using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Core
{
  [TestFixture]
  public class SimpleValueTest
  {
    private MultiValueValidation.SimpleValue _simpleValueValidation;

    [SetUp]
    public void SetUp()
    {
      _simpleValueValidation = new MultiValueValidation.SimpleValue();
    }

     [Test, ExpectedException(typeof(ValidationException))]
     public void Validate_GivenNull_Throw()
     {
       _simpleValueValidation.Validate(null);
     }

    [Test, ExpectedException(typeof(ValidationException))]
     public void Validate_GivenInvalidValue_Throw()
     {
       _simpleValueValidation.Validation = "Expected value";

       _simpleValueValidation.Validate("Wrong value");
     }

     [TestCase("", "")]
     [TestCase("", ".*")]
     [TestCase("some value", "^.*$")]
     public void Validate_GivenValidValue(string value, string validation)
     {
       _simpleValueValidation.Validation = validation;

       _simpleValueValidation.Validate(value);
     }
  }
}