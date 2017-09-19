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
                    "\"DateTime\":\"2015-02-12T16:00:00.000Z\"," +
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
      Assert.That(complexArray("Events", 0).SingleValues["DateTime"], Is.EqualTo("2015-02-12T16:00:00.000Z"));
      Assert.That(complexArray("Events", 0).SingleValues["Data"], Is.Null);
      Assert.That(complexArray("Events", 1).SingleValues["Id"], Is.EqualTo("id 2"));
      Assert.That(complexArray("Contexts", 0).SingleValues["Type"], Is.EqualTo("IPaddress"));
      Assert.That(singleArray("SimpleArray", 0), Is.EqualTo("type"));
    }
    
    [Test]
    public void ReadJson_DeserializeObjectsArraysAndProperties2()
    {
      var json = "{"+
        "\"Events\": ["+
      "{"+
        "\"Id\": \"None\","+
          "\"Type\": \"Search\","+
          "\"Method\": null,"+
          "\"Data\": \"def\","+
          "\"DateTime\": \"2017-09-19T09:31:35.802Z\""+
      "},"+
      "{"+
        "\"Id\": \"MyItem1\","+
          "\"Type\": \"Result Selected\","+
          "\"Method\": \"None\","+
          "\"Data\": \"None\","+
          "\"DateTime\": \"2017-09-19T09:31:38.240Z\""+
      "},"+
      "{"+
        "\"Id\": \"MyItem1\","+
          "\"Type\": \"Player\","+
          "\"Method\": \"TooglePlay\","+
          "\"Data\": \"{\\\"IsPlaying\\\":true,\\\"Position\\\":0}\","+
          "\"DateTime\": \"2017-09-19T09:31:38.745Z\""+
      "},"+
      "{"+
        "\"Id\": \"MyItem1\","+
          "\"Type\": \"Player\","+
          "\"Method\": \"TooglePlay\","+
          "\"Data\": \"{\\\"IsPlaying\\\":false,\\\"Position\\\":2465}\","+
          "\"DateTime\": \"2017-09-19T09:31:41.524Z\""+
      "},"+
      "{"+
        "\"Id\": \"MyItem1\","+
          "\"Type\": \"Answer\","+
          "\"Method\": \"Selection\","+
          "\"Data\": \"{\\\"Rating\\\":\\\"0\\\"}\","+
          "\"DateTime\": \"2017-09-19T09:31:43.239Z\""+
      "},"+
      "{"+
        "\"Id\": \"1\","+
          "\"Type\": \"Segment Selected\","+
          "\"Method\": null,"+
          "\"Data\": \"{\\\"SelectionId\\\":\\\"MyItem1\\\"}\","+
          "\"DateTime\": \"2017-09-19T09:31:48.424Z\""+
      "},"+
      "{"+
        "\"Id\": \"MyItem1\","+
          "\"Type\": \"Answer\","+
          "\"Method\": \"Segment\","+
          "\"Data\": \"{\\\"SegmentId\\\":\\\"1\\\",\\\"Rating\\\":\\\"1\\\"}\","+
          "\"DateTime\": \"2017-09-19T09:31:57.286Z\""+
      "}"+
      "],"+
      "\"Selections\": ["+
      "{"+
        "\"Id\": \"MyItem1\","+
          "\"Rating\": \"0\","+
          "\"SegmentRatings\": \"[]\""+
      "}"+
      "]"+
    "}";
      
      var output = JsonConvert.DeserializeObject<OutputDto>(json, new OutputConverter());
    }
  }
}