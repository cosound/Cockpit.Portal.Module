using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Api.Result
{
  public class Output
  {
    public Output()
    {
      ComplexValues = new Dictionary<string, ComplexValueResult>();
      MultiValues = new Dictionary<string, IMultiValueResult>();
      SingleValues = new Dictionary<string, string>();
    }

    [JsonIgnore]
    public IDictionary<string, ComplexValueResult> ComplexValues { get; set; }

    [JsonIgnore]
    public IDictionary<string, IMultiValueResult> MultiValues { get; set; }

    [JsonIgnore]
    public IDictionary<string, string> SingleValues { get; set; }

    
  }

  public interface IMultiValueResult
  {
    IEnumerable<object> Values { get; }
  }

  public class MultiComplexValueResult : IMultiValueResult
  {
    public MultiComplexValueResult()
    {
      ComplexValues = new List<ComplexValueResult>();
    }

    [JsonIgnore]
    public IList<ComplexValueResult> ComplexValues { get; set; }

    public IEnumerable<object> Values
    {
      get
      {
        foreach (var complexValueResult in ComplexValues)
        {
          if (complexValueResult.SingleValues.Count != 0)
            yield return complexValueResult.SingleValues;
          
          if (complexValueResult.ComplexValues.Count != 0)
            yield return complexValueResult.ComplexValues;

          if (complexValueResult.MultiValues.Count != 0)
            yield return complexValueResult.MultiValues;
        }
      }
    }
  }

  public class MultiSingleValueResult : IMultiValueResult
  {
    public MultiSingleValueResult()
    {
      SingleValues = new List<string>();
    }

    [JsonIgnore]
    public IList<string> SingleValues { get; set; }

    public IEnumerable<object> Values
    {
      get
      {
        foreach (var singleValue in SingleValues)
        {
          yield return singleValue;
        }
      }
    }
  }

  public class ComplexValueResult
  {
    public ComplexValueResult()
    {
      ComplexValues = new Dictionary<string, ComplexValueResult>();
      MultiValues = new Dictionary<string, MultiComplexValueResult>();
      SingleValues = new Dictionary<string, string>();
    }

    [JsonIgnore]
    public IDictionary<string, ComplexValueResult> ComplexValues { get; set; }

    [JsonIgnore]
    public IDictionary<string, MultiComplexValueResult> MultiValues { get; set; }

    [JsonIgnore]
    public IDictionary<string, string> SingleValues { get; set; }

    public IDictionary<string, object> Values
    {
      get
      {
        var dict = new Dictionary<string, object>();

        foreach (var singleValue in SingleValues)
        {
          dict.Add(singleValue.Key, singleValue.Value);
        }

        foreach (var complexValueResult in ComplexValues)
        {
          dict.Add(complexValueResult.Key, complexValueResult.Value);
        }

        foreach (var multiValueResult in MultiValues)
        {
          dict.Add(multiValueResult.Key, multiValueResult.Value);
        }

        return dict;
      }
    }
  }
}