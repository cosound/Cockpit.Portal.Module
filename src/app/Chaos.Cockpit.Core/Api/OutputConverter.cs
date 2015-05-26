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
      var output = new Result.OutputDto();
      var key = "";

      while (reader.Read())
      {
        if (reader.TokenType == JsonToken.PropertyName)
          key = reader.Value.ToString();

        if (reader.TokenType == JsonToken.Date || reader.TokenType == JsonToken.String)
          output.SingleValues.Add(ReadProperty(reader, key));

        if (reader.TokenType == JsonToken.StartObject)
        {
          var complex = new ComplexValueResult();
          string currentKey = null;

          while (reader.Read())
          {
            if (reader.TokenType == JsonToken.EndObject)
              break;

            if (reader.TokenType == JsonToken.PropertyName)
              currentKey = reader.Value.ToString();

            if (reader.TokenType == JsonToken.String)
              complex.SingleValues.Add(ReadProperty(reader, currentKey));
          }

          output.ComplexValues.Add(key, complex);
        }

        if (reader.TokenType == JsonToken.StartArray)
        {
          var multiValue = ReadArray(reader);

          output.MultiValues.Add(key, multiValue);
        }
      }

      return output;
    }

    private KeyValuePair<string, string> ReadProperty(JsonReader reader, string key)
    {
      if (reader.TokenType == JsonToken.String)
        return new KeyValuePair<string, string>(key, reader.Value.ToString());

      if (reader.TokenType == JsonToken.Date)
        return new KeyValuePair<string, string>(key, ((DateTime)reader.Value).ToString("yyyy-MM-dd'T'HH:mm:ss'.'fff'Z'"));

      throw new Exception("Unknown TokenType: " + reader.TokenType);
    }

    private IMultiValueResult ReadArray(JsonReader reader)
    {
      reader.Read();

      if (reader.TokenType == JsonToken.StartObject)
        return ReadComplexArray(reader);

      if (reader.TokenType == JsonToken.String)
        return ReadSingleArray(reader);

      return new MultiSingleValueResult();
    }

    private IMultiValueResult ReadComplexArray(JsonReader reader)
    {
      var complexArray = new MultiComplexValueResult();

      while (true)
      {
        if(reader.TokenType == JsonToken.EndArray)
          return complexArray;

        var complex = new ComplexValueResult();

        ReadObject(reader, complex);

        complexArray.ComplexValues.Add(complex);

        reader.Read();
      }
    }

    private IMultiValueResult ReadSingleArray(JsonReader reader)
    {
      var singleArray = new MultiSingleValueResult();

      while (true)
      {
        if(reader.TokenType == JsonToken.EndArray)
          return singleArray;

        singleArray.SingleValues.Add(reader.Value.ToString());

        reader.Read();
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
          value.SingleValues.Add(key, null);
        
        if (reader.TokenType == JsonToken.String)
          value.SingleValues.Add(key, reader.Value.ToString());
        
        if (reader.TokenType == JsonToken.Date)
        {
          var datetime = (DateTime) reader.Value;
          value.SingleValues.Add(key, datetime.ToString("yyyy-MM-dd'T'HH:mm:ss'.'fff'Z'"));
        }

        if (reader.TokenType == JsonToken.EndObject)
          break;
      }
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType.IsAssignableFrom(typeof(Result.OutputDto));
    }
  }
}