using System;
using System.Linq;
using Chaos.Cockpit.Core.Api;
using Chaos.Cockpit.Core.Api.Result;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Test.Api
{
  [TestFixture]
  public class OutputConverterTest
  {
    [Test]
    public void ReadJson_DeserializeObjectsArraysAndProperties()
    {
      var json = "{\"MyValue\":\"Some value\", " +
                 "\"Events\":[{" +
                    "\"Id\":\".\\\\\", " +
                    "\"Type\":\"Start<!---->\"," +
                    "\"Data\":null},{" +
                    "\"Id\":\"id 2\"}]," +
                 "\"Contexts\":[{" +
                 "\"Type\":\"IPaddress\"}]," +
                 "\"SimpleArray\":[" +
                 "\"type\",\"type2\"]}";
      
      var output = JsonConvert.DeserializeObject<OutputDto>(json, new OutputConverter());

      Func<string, int, string> singleArray = (key, i) => output.MultiValues.First(item => item.Key == key).Value.Values.Cast<string>().ToList()[i];
      Func<string, int, ComplexValueResult> complexArray = (key, i) => (output.MultiValues.First(item => item.Key == key).Value as MultiComplexValueResult).ComplexValues.ToList()[i];
      Assert.That(complexArray("Events", 0).SingleValues["Id"], Is.EqualTo(".\\"));
      Assert.That(complexArray("Events", 0).SingleValues["Type"], Is.EqualTo("Start<!---->"));
      Assert.That(complexArray("Events", 0).SingleValues["Data"], Is.Null);
      Assert.That(complexArray("Events", 1).SingleValues["Id"], Is.EqualTo("id 2"));
      Assert.That(complexArray("Contexts", 0).SingleValues["Type"], Is.EqualTo("IPaddress"));
      Assert.That(singleArray("SimpleArray", 0), Is.EqualTo("type"));
    }
  }
}