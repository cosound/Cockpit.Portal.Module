using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Core;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Test.Core
{
  [TestFixture]
  public class Class1
  {
    [Test]
    public void create()
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
                 "\"type\"]}";

      var output = JsonConvert.DeserializeObject<Output>(json, new OutputConverter());

      Assert.That(output.Values.First(item => item.Key == "MyValue").Value, Is.EqualTo("Some value"));
      Func<string, MultiValue> multiValue = key => output.Values2.First(item => item.Key == key);
      Func<string, int, int, SimpleValue> queryMultiValue = (key, i,j) => multiValue(key).ComplexValues.ToList()[i].SimpleValues.ToList()[j];
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

  public class Output
  {
    public Output()
    {
      Values = new List<SimpleValue>();
      Values2 = new List<MultiValue>();
    }

    public List<SimpleValue> Values { get; set; }
    public List<MultiValue> Values2 { get; set; }
  }

  public class OutputConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var output = new Output();
      
      while (reader.Read())
      {
        var key = "";

        if (reader.TokenType == JsonToken.PropertyName)
        {
          key = reader.Value.ToString();
          reader.Read();
        }

        if(reader.TokenType == JsonToken.String)
          output.Values.Add(new SimpleValue(key, reader.Value.ToString()));

        if (reader.TokenType == JsonToken.StartArray)
        {
          var multiValue = new MultiValue{Key = key};

          ReadArray(reader, multiValue);

          output.Values2.Add(multiValue);
        }

        System.Console.WriteLine(reader.TokenType);
      }

      return output;
    }

    private void ReadArray(JsonReader reader, MultiValue multiValue)
    {
      while (reader.Read())
      {
        if (reader.TokenType == JsonToken.StartObject)
        {
          var complex = new ComplexValue();

          ReadObject(reader, complex);

          multiValue.ComplexValues.Add(complex);
        }

        if(reader.TokenType == JsonToken.String)
          multiValue.SimpleValues.Add(new SimpleValue(null, reader.Value.ToString()));

        if(reader.TokenType == JsonToken.EndArray)
          break;
      }
    }

    private void ReadObject(JsonReader reader, ComplexValue value)
    {
      while (reader.Read())
      {
        var key = "";

        if (reader.TokenType == JsonToken.PropertyName)
        {
          key = reader.Value.ToString();
        
          reader.Read();
        }

        if (reader.TokenType == JsonToken.Null)
          value.SimpleValues.Add(new SimpleValue(key, null));
        
        if (reader.TokenType == JsonToken.String)
          value.SimpleValues.Add(new SimpleValue(key, reader.Value.ToString()));

        if (reader.TokenType == JsonToken.EndObject)
          break;
      }
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType.IsAssignableFrom(typeof(Output));
    }
  }
}
