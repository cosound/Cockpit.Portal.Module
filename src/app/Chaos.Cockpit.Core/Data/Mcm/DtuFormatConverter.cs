using System;
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
      result.Version = experiemnt.Element("Version").Value;
      result.TargetId = experiemnt.Element("Target").Attribute("Id").Value;
      // todo better error messages

      result.TargetName = experiemnt.Element("Target").Attribute("Name").Value;

      var trials = experiemnt.Element("Trials").Elements("Trial");
      foreach (var trial in trials)
        DeserializeTrial(trial, result);

      return result;
    }

    private static void DeserializeTrial(XElement trial, Questionnaire result)
    {
      var slide = new Slide();
      slide.TaskId = trial.Attribute("TaskId").Value;
      slide.IsClosed = IsClosed(trial);
      result.AddSlide(slide);

      foreach (var questionElement in trial.Elements())
        DeserializeQuestion(questionElement, slide);
    }

    private static bool IsClosed(XElement trial)
    {
      var attribute = trial.Attribute("IsClosed");

      return attribute != null && bool.Parse(attribute.Value);
    }

    private static void DeserializeQuestion(XElement questionElement, Slide slide)
    {
      var type = questionElement.Name.LocalName;
      var question = new Question(type);
      question.Version = questionElement.Attribute("Version").Value;
      question.Input = questionElement.Element("Inputs").Elements();

      var validationElement = questionElement.Element("Outputs").Element("Validation");
      question.Validation.MultiValueValidator = FindMultiValueValidators(validationElement).ToList();
      question.Validation.SimpleValueValidator = FindSimpleValueValidator(validationElement).ToList();

      var output = new Output();
      var value = questionElement.Element("Outputs").Element("Value");

      if(value != null)
      {
        var valueElements = value.Elements();
        foreach (var valueElement in valueElements)
          DeserializeOutput(valueElement, output);

        if (valueElements.Any())
          question.Output = output;
      }

      slide.AddQuestion(question);
    }

    private static void DeserializeOutput(XElement valueElement, Output output)
    {
      if (valueElement.HasElements)
      {
        // simple or complex
        var isMultiValue = valueElement.Elements().GroupBy(item => item.Name.LocalName).Count() == 1;
        if (isMultiValue)
          output.MultiValues.Add(DeserializeMultiValue(valueElement));
        else
        {
          // is complex
          var s = "";
        }
      }
      else
        output.SimpleValues.Add(new SimpleValue(valueElement.Name.LocalName, valueElement.Value));
    }

    private static MultiValue DeserializeMultiValue(XElement valueElement)
    {
      var multiValue = new MultiValue();
      multiValue.Key = valueElement.Name.LocalName;

      foreach (var element in valueElement.Elements())
      {
        if (element.HasElements)
        {
          var complexValue = new ComplexValue();
          complexValue.Key = element.Name.LocalName;

          foreach (var field in element.Elements())
            complexValue.Add(new SimpleValue(field.Name.LocalName, field.Value));

          multiValue.ComplexValues.Add(complexValue);
        }
        else
        {
          multiValue.SimpleValues.Add(element.Value);
        }
      }
      return multiValue;
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

    public XDocument Serialize(Questionnaire questionnaire)
    {
      var xml = new XDocument();
      xml.Add(new XElement("Experiment"));
      xml.Root.Add(new XElement("Id", questionnaire.Id));
      xml.Root.Add(new XElement("Name", questionnaire.Name));
      xml.Root.Add(new XElement("Version", questionnaire.Version));
      xml.Root.Add(SerializeTarget(questionnaire));
      xml.Root.Add(SerializeTrials(questionnaire));

      return xml;
    }

    private static XElement SerializeTarget(Questionnaire questionnaire)
    {
      var target = new XElement("Target");
      target.Add(new XAttribute("Id", questionnaire.TargetId));
      target.Add(new XAttribute("Name", questionnaire.TargetName));
      return target;
    }

    private static XElement SerializeTrials(Questionnaire questionnaire)
    {
      var trials = new XElement("Trials");

      foreach (var slide in questionnaire.Slides)
        trials.Add(SerializeTrial(slide));

      return trials;
    }

    private static XElement SerializeTrial(Slide slide)
    {
      var trial = new XElement("Trial");
      trial.Add(new XAttribute("TaskId", slide.TaskId));
      trial.Add(new XAttribute("IsClosed", slide.IsClosed));

      foreach (var question in slide.Questions)
        trial.Add(SerializeQuestion(question));

      return trial;
    }

    private static XElement SerializeQuestion(Question question)
    {
      var component = new XElement(question.Type);
      component.Add(new XAttribute("Version", question.Version));

      var input = new XElement("Inputs");
      input.Add(question.Input);
      component.Add(input);

      var output = new XElement("Outputs");

      var validation = new XElement("Validation");
      validation.Add(SerializeSimpleValidators(question.Validation.SimpleValueValidator));
      validation.Add(SerializeMultiValidators(question.Validation.MultiValueValidator));

      var value = new XElement("Value");

      if (question.Output != null)
      {
        foreach (var multiValue in question.Output.MultiValues)
          value.Add(SerializeMultiValue(multiValue));

        foreach (var complexValue in question.Output.ComplexValues)
          value.Add(SerializeComplexValue(complexValue));

        foreach (var simpleValue in question.Output.SimpleValues)
          value.Add(SerializeSimpleValue(simpleValue));
      }

      output.Add(validation);
      output.Add(value);
      component.Add(output);

      return component;
    }

    private static XElement SerializeMultiValue(MultiValue multiValue)
    {
      var element = new XElement(multiValue.Key);

      foreach (var simpleValue in multiValue.SimpleValues)
        element.Add(new XElement("Item", simpleValue));

      foreach (var complexValue in multiValue.ComplexValues)
      {
        if (string.IsNullOrEmpty(complexValue.Key))
          complexValue.Key = "Item";

        var complexElement = new XElement(complexValue.Key);
        foreach (var simpleValue in complexValue.SimpleValues)
          complexElement.Add(new XElement(simpleValue.Key, simpleValue.Value));

        element.Add(complexElement);
      }
      return element;
    }

    private static XElement SerializeComplexValue(ComplexValue complexValue)
    {
      if (string.IsNullOrEmpty(complexValue.Key))
        throw new ArgumentException("Complex Type Key cannot be null.");

      var complexElement = new XElement(complexValue.Key);

      foreach (var simpleValue in complexValue.SimpleValues)
        complexElement.Add(new XElement(simpleValue.Key, simpleValue.Value));
      return complexElement;
    }

    private static XElement SerializeSimpleValue(SimpleValue simpleValue)
    {
      return new XElement(simpleValue.Key, simpleValue.Value);
    }

    private static IEnumerable<XElement> SerializeMultiValidators(IEnumerable<MultiValueValidator> multiValueValidators)
    {
      foreach (var validator in multiValueValidators)
      {
        var multi = new XElement("MultiValue");
        multi.Add(new XAttribute("Id", validator.Id));
        multi.Add(new XAttribute("Max", ConvertToString(validator.Max)));
        multi.Add(new XAttribute("Min", ConvertToString(validator.Min)));

        if (validator.ComplexValueValidator != null)
        {
          var complex = new XElement("ComplexValue");
          complex.Add(new XAttribute("Id", validator.ComplexValueValidator.Id));
          complex.Add(SerializeSimpleValidators(validator.ComplexValueValidator.SimpleValueValidators));
          multi.Add(complex);
        }

        if (validator.SimpleValueValidator != null)
          multi.Add(SerializeSimpleValidators(new[] {validator.SimpleValueValidator}));

        yield return multi;
      }
    }

    private static IEnumerable<XElement> SerializeSimpleValidators(
      IEnumerable<SimpleValueValidator> simpleValueValidators)
    {
      foreach (var validator in simpleValueValidators)
      {
        var simple = new XElement("SimpleValue");
        simple.Add(new XAttribute("Id", validator.Id));
        simple.Add(new XAttribute("Validation", validator.Validation));

        yield return simple;
      }
    }

    private static string ConvertToString(uint max)
    {
      return max == uint.MaxValue ? "Inf" : max.ToString();
    }
  }
}