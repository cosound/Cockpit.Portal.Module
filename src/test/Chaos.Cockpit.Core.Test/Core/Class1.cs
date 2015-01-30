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
      var json = "{\"MyValue\":\"Some value\", \"Events\":[{\"Id\":\".\\\\\"}]}";

      var output = JsonConvert.DeserializeObject<Output>(json, new OutputConverter());

      Assert.That(output.Values.First(item => item.Key == "MyValue").Value, Is.EqualTo("Some value"));
    }
  }

  public class Output
  {
    public Output()
    {
      Values = new List<SimpleValue>();
    }

    public List<SimpleValue> Values { get; set; }
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

      var key = "";
      var fields = new Dictionary<string, string>();
      while (reader.Read())
      {
        if (reader.TokenType == JsonToken.PropertyName)
          key = reader.Value.ToString();

        if(reader.TokenType == JsonToken.String)
          output.Values.Add(new SimpleValue(key, reader.Value.ToString()));

        if (reader.TokenType == JsonToken.StartArray)
        {
          
        }

      }

      System.Console.WriteLine(key);

      return output;
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType.IsAssignableFrom(typeof(Output));
    }
  }
}
