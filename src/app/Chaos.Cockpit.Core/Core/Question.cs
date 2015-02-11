using System.Xml.Linq;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core.Validation;

namespace Chaos.Cockpit.Core.Core
{
  using System.Collections.Generic;

  public class Question : IKey
  {
    public string Id { get; set; }
    public Answer UserAnswer { get; set; }
    public string Type { get; set; }
    public IEnumerable<XElement> Input { get; set; }
    public Output Output { get; set; }
    public OutputValidator Validation { get; set; }

    public Question(string type)
    {
      Type = type;
      Input = new List<XElement>();
      Output = new Output();
      Validation = new OutputValidator();
    }
  }

  public class OutputValidator
  {
    public IList<MultiValueValidator> MultiValueValidator { get; set; }
    public IList<ComplexValueValidator> ComplexValueValidator { get; set; }
    public IList<SimpleValueValidator> SimpleValueValidator { get; set; }

    public OutputValidator()
    {
      MultiValueValidator = new List<MultiValueValidator>();
      ComplexValueValidator = new List<ComplexValueValidator>();
      SimpleValueValidator = new List<SimpleValueValidator>();
    }
  }

  public class Output
  {
    public IList<ComplexValue> ComplexValues { get; set; }
    public IList<MultiValue> MultiValues { get; set; }
    public IList<SimpleValue> SimpleValues { get; set; }

    public Output()
    {
      ComplexValues = new List<ComplexValue>();
      MultiValues = new List<MultiValue>();
      SimpleValues = new List<SimpleValue>();
    }
  }
}
