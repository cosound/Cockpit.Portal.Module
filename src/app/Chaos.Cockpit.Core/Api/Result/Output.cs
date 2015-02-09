using System.Collections.Generic;

namespace Chaos.Cockpit.Core.Api.Result
{
  public class Output
  {
    public Output()
    {
      Values = new List<MultiValueResult>();
    }

    public List<MultiValueResult> Values { get; set; }
  }

  public class MultiValueResult
  {
    public MultiValueResult()
    {
      ComplexValues = new List<ComplexValueResult>();
      SimpleValues = new List<SimpleValueResult>();
    }

    public string Key { get; set; }
    public IList<ComplexValueResult> ComplexValues { get; set; }
    public IList<SimpleValueResult> SimpleValues { get; set; }
  }

  public class ComplexValueResult
  {
    public ComplexValueResult()
    {
      SimpleValues = new List<SimpleValueResult>();
    }

    public string Key { get; set; }
    public IList<SimpleValueResult> SimpleValues { get; set; }
  }

  public class SimpleValueResult
  {
    public SimpleValueResult(string key, string value)
    {
      Key = key;
      Value = value;
    }

    public string Key { get; set; }
    public string Value { get; set; }
  }

  public interface IValue
  {
    string Key { get; set; }
  }
}