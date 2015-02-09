using System;
using System.Linq;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Api;
using Chaos.Cockpit.Core.Api.Result;
using NUnit.Framework;
using Newtonsoft.Json;
using Output = Chaos.Cockpit.Core.Api.Result.Output;

namespace Chaos.Cockpit.Core.Test.Api
{
  [TestFixture]
  public class OutputConverterTest
  {
    [Test]
    public void ReadJson_DeserializeObjectsArraysAndProperties()
    {


      var xml = XDocument.Parse("<xmls><xml>123</xml><xml>1234</xml></xmls>");
      System.Console.WriteLine(JsonConvert.SerializeObject(xml));

      var json = "{\"MyValue\":\"Some value\", " +
                 "\"Events\":[{" +
                    "\"Id\":\".\\\\\", " +
                    "\"Type\":\"Start<!---->\"," +
                    "\"Data\":null},{" +
                    "\"Id\":\"id 2\"}]," +
                 "\"Contexts\":[{" +
                 "\"Type\":\"IPaddress\"}]," +
                 "\"SimpleArray\":[" +
                 "\"type\"]}";

      var output = JsonConvert.DeserializeObject<Output>(json, new OutputConverter());

      Func<string, MultiValueResult> multiValue = key => output.Values.First(item => item.Key == key);
      Func<string, int, int, SimpleValueResult> queryMultiValue = (key, i,j) => multiValue(key).ComplexValues.ToList()[i].SimpleValues.ToList()[j];
      Assert.That(output.Values.First().SimpleValues.First().Value, Is.EqualTo("Some value"));
      Assert.That(queryMultiValue("Events", 0, 0).Key, Is.EqualTo("Id"));
      Assert.That(queryMultiValue("Events", 0, 0).Value, Is.EqualTo(".\\"));
      Assert.That(queryMultiValue("Events", 0, 1).Key, Is.EqualTo("Type"));
      Assert.That(queryMultiValue("Events", 0, 1).Value, Is.EqualTo("Start<!---->"));
      Assert.That(queryMultiValue("Events", 0, 2).Key, Is.EqualTo("Data"));
      Assert.That(queryMultiValue("Events", 0, 2).Value, Is.Null);
      Assert.That(queryMultiValue("Events", 1, 0).Key, Is.EqualTo("Id"));
      Assert.That(queryMultiValue("Events", 1, 0).Value, Is.EqualTo("id 2"));
      Assert.That(queryMultiValue("Contexts", 0, 0).Key, Is.EqualTo("Type"));
      Assert.That(queryMultiValue("Contexts", 0, 0).Value, Is.EqualTo("IPaddress"));
      Assert.That(multiValue("SimpleArray").SimpleValues.First().Value, Is.EqualTo("type"));
    }
  }
}
