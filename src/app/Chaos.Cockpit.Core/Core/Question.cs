﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Core.Validation;

namespace Chaos.Cockpit.Core.Core
{
  public class Question : IKey
  {
    private Output _output;
    
    public string Id { get; set; }
    public Answer UserAnswer { get; set; }
    public string Type { get; set; }
    public IEnumerable<XElement> Input { get; set; }
    public OutputValidator Validation { get; set; }
    
    public Output Output
    {
      set
      {
        if (value != null)
        {
          foreach (var validator in Validation.ComplexValueValidator)
          {
            var complex = value.ComplexValues.Single(item => item.Key == validator.Id);

            validator.Validate(complex);
          }

          foreach (var validator in Validation.MultiValueValidator)
          {
            var multi = value.MultiValues.Single(item => item.Key == validator.Id);

            validator.Validate(multi);
          }

          foreach (var validator in Validation.SimpleValueValidator)
          {
            var simple = value.SimpleValues.Single(item => item.Key == validator.Id);

            validator.Validate(simple);
          }
        }

        _output = value;
      }
      get { return _output; }
    }

    public Question(string type)
    {
      Type = type;
      Input = new List<XElement>();
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
