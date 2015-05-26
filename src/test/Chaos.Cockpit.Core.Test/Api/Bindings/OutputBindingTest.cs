namespace Chaos.Cockpit.Core.Test.Api.Bindings
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Cockpit.Core.Api.Binding;
  using Cockpit.Core.Api.Result;
  using NUnit.Framework;

  [TestFixture]
  public class OutputBindingTest
  {
    [Test, ExpectedException(typeof(ArgumentException))]
    public void Bind_GivenNoParameterWithTheCorrectName_Throw()
    {
      var binding = new OutputBinding();
      var parameters = new Dictionary<string, string>();
      var info = new BindingTest().GetType().GetMethod("Test").GetParameters().First();

      binding.Bind(parameters, info);
    }

    [Test]
    public void Bind_GivenAParameterContaining_Throw()
    {
      var binding = new OutputBinding();
      var parameters = new Dictionary<string, string> { { "output", "{ 'CheckSumAsHash': 'd3dc4e53a3ac7a5a106dfde2a5c71243', 'ClientUserGuid': '3a00d580-a0a0-4b84-bb8f-e4e819bbceeb', 'ClientBrowserUserAgent': 'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.90 Safari/537.36', 'ClientDeviceIPAddressAsHash': 'f6c28486c968d4582bd488ca2d67b50d', 'Response': { 'RadioButtonSelection': '0', 'DontknowSelection': '0', 'FreeText': 'Helle Helle', 'Grading': '30', 'Log': 'Any debug info you may need, e.g. streaming issues etc' }, 'Events': [ { 'DateTime': '2015-01-07T07:06.560Z', 'Type': 'TrialStart' }, { 'DateTime': '2015-01-07T07:08.560Z', 'Type': 'StimulusStart' }, { 'DateTime': '2015-01-07T08:09.560Z', 'Type': 'ReponseStart' }, { 'DateTime': '2015-01-07T08:12.560Z', 'Type': 'ResponseStop' }, { 'DateTime': '2015-01-07T07:56.560Z', 'Type': 'TrialStop' } ] }" } };
      var info = new BindingTest().GetType().GetMethod("Test").GetParameters().First();

      var result = (OutputDto) binding.Bind(parameters, info);
      
      Assert.That(result.ComplexValues.Count, Is.EqualTo(1));
      Assert.That(result.MultiValues.Count, Is.EqualTo(1));
      Assert.That(result.SingleValues.Count, Is.EqualTo(4));
      Assert.That(result.MultiValues.First().Key, Is.EqualTo("Events"));
      Assert.That(result.ComplexValues.First().Key, Is.EqualTo("Response"));
      Assert.That(result.ComplexValues.First().Value.SingleValues.First().Key, Is.EqualTo("RadioButtonSelection"));
      Assert.That(result.ComplexValues.First().Value.SingleValues.First().Value, Is.EqualTo("0"));
    }

    public class BindingTest
    {
      public void Test(OutputDto output)
      {
        
      }
    }
  }
}
