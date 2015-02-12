using System.Collections.Generic;
using Chaos.Cockpit.Core.Core.Validation;

namespace Chaos.Cockpit.Core.Data.Mcm
{
  using System.Linq;
  using System.Xml.Linq;
  using Core;

  public class DtuFormatConverter
  {
    public Questionnaire Deserialize(XDocument xml)
    {
      var experiemnt = xml.Element("Experiment");
      
      var result = new Questionnaire();
      result.Id = experiemnt.Element("Id").Value;
      result.Name = experiemnt.Element("Name").Value;

      var trials = xml.Descendants("Trial");
      foreach (var trial in trials)
      {
        var slide = new Slide();
        result.AddSlide(slide);

        foreach (var questionElement in trial.Elements())
        {
          var type = questionElement.Name.LocalName;
          var question = new Question(type);

          question.Input = questionElement.Element("Inputs").Elements();

          var validationElement = questionElement.Element("Outputs").Element("Validation");
          question.Validation.MultiValueValidator = FindMultiValueValidators(validationElement).ToList();

          slide.AddQuestion(question);
        }
      }

      return result;
    }

    private static IEnumerable<MultiValueValidator> FindMultiValueValidators(XContainer parent)
    {
      foreach (var multiElement in parent.Elements("MultiValue"))
      {
        var multi = new MultiValueValidator();
        multi.Id = multiElement.Attribute("Id").Value;
        multi.Min = StringToUint(multiElement.Attribute("Min").Value);
        multi.Max = StringToUint(multiElement.Attribute("Max").Value);

        multi.ComplexValueValidator = FindComplexValueValidators(multiElement).SingleOrDefault();
        multi.SimpleValueValidator = FindSimpleValueValidator(multiElement).SingleOrDefault();

        yield return multi;
      }
    }

    private static IEnumerable<ComplexValueValidator> FindComplexValueValidators(XContainer parent)
    {
      foreach (var complexElement in parent.Elements("ComplexValue"))
      {
        var complex = new ComplexValueValidator();
        complex.Id = complexElement.Attribute("Id").Value;

        complex.SimpleValueValidators = FindSimpleValueValidator(complexElement).ToList();

        yield return complex;
      }
    }

    private static IEnumerable<SimpleValueValidator> FindSimpleValueValidator(XContainer parent)
    {
      foreach (var simpleElement in parent.Elements("SimpleValue"))
      {
        var simple = new SimpleValueValidator();
        simple.Id = simpleElement.Attribute("Id").Value;
        simple.Validation = simpleElement.Attribute("Validation").Value;

        yield return simple;
      }
    }

    private static uint StringToUint(string value)
    {
      if ("Inf".Equals(value))
        return uint.MaxValue;

      return uint.Parse(value);
    }
  }
}