namespace Chaos.Cockpit.Core.Test.Api.Bindings
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Cockpit.Core.Api.Binding;
  using Cockpit.Core.Api.Result;
  using NUnit.Framework;

  [TestFixture]
  public class AnswerResultBindingTest
  {
    [Test, ExpectedException(typeof(ArgumentException))]
    public void Bind_GivenNoParameterWithTheCorrectName_Throw()
    {
      var binding = new JsonBinding<AnswerDto>();
      var parameters = new Dictionary<string, string>();
      var info = new BindingTest().GetType().GetMethod("Test").GetParameters().First();

      binding.Bind(parameters, info);
    }

    [Test]
    public void Bind_GivenAParameterContaining_Throw()
    {
      var binding = new JsonBinding<AnswerDto>();
      var parameters = new Dictionary<string, string> { { "answer", "{'Id':'1234', 'Data':{'Val1':'1','Val2':'2'}}" } };
      var info = new BindingTest().GetType().GetMethod("Test").GetParameters().First();

      var result = (AnswerDto) binding.Bind(parameters, info);

      Assert.That(result.Identifier, Is.EqualTo("1234"));
      Assert.That(result.Data["Val1"], Is.EqualTo("1"));
      Assert.That(result.Data["Val2"], Is.EqualTo("2"));
    }

    public class BindingTest
    {
      public void Test(AnswerDto answer)
      {
        
      }
    }
  }
}
