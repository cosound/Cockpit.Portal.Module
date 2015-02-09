using System;
using System.Collections.Generic;
using Chaos.Cockpit.Core.Api.Result;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Api
{
  public class OutputConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var output = new Result.Output();
      
      while (reader.Read())
      {
        var key = "";

        if (reader.TokenType == JsonToken.PropertyName)
        {
          key = reader.Value.ToString();
          reader.Read();
        }

        if (reader.TokenType == JsonToken.String)
        {
          var mv = new MultiValueResult
            {
              SimpleValues = new List<SimpleValueResult>()
                {
                  new SimpleValueResult(key, reader.Value.ToString())
                }
            };

          output.Values.Add(mv);
        }

        if (reader.TokenType == JsonToken.StartArray)
        {
          var multiValue = new MultiValueResult { Key = key };

          ReadArray(reader, multiValue);

          output.Values.Add(multiValue);
        }
      }

      return output;
    }

    private void ReadArray(JsonReader reader, MultiValueResult multiValue)
    {
      while (reader.Read())
      {
        if (reader.TokenType == JsonToken.StartObject)
        {
          var complex = new ComplexValueResult();

          ReadObject(reader, complex);

          multiValue.ComplexValues.Add(complex);
        }

        if(reader.TokenType == JsonToken.String)
          multiValue.SimpleValues.Add(new SimpleValueResult(null, reader.Value.ToString()));

        if(reader.TokenType == JsonToken.EndArray)
          break;
      }
    }

    private void ReadObject(JsonReader reader, ComplexValueResult value)
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
          value.SimpleValues.Add(new SimpleValueResult(key, null));
        
        if (reader.TokenType == JsonToken.String)
          value.SimpleValues.Add(new SimpleValueResult(key, reader.Value.ToString()));

        if (reader.TokenType == JsonToken.EndObject)
          break;
      }
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType.IsAssignableFrom(typeof(Result.Output));
    }
  }
}