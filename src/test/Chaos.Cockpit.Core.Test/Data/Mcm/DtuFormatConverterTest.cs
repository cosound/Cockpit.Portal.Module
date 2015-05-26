using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Api;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Validation;
using Chaos.Cockpit.Core.Data.Mcm;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Test.Data.Mcm
{
  [TestFixture]
  public class DtuFormatConverterTest
  {
    [Test]
    public void Deserialize_GivenExperimentWithOneTask_ParseRequiredFields()
    {
      var converter = new DtuFormatConverter();
      var xml = XDocument.Load("Resources\\experiment.xml");

      var result = converter.Deserialize(xml);

      Assert.That(result.Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7"));
      Assert.That(result.Name, Is.EqualTo("DTU:Test:001"));
      Assert.That(result.Version, Is.EqualTo("1"));
      Assert.That(result.TargetId, Is.EqualTo("fba94e48-3c6d-4a2a-aef9-23108ee89ac0"));
      Assert.That(result.TargetName, Is.EqualTo("Jens Jensen"));
      Assert.That(result.Slides.Count, Is.EqualTo(8));
      Assert.That(result.Slides[0].TaskId, Is.EqualTo("0"));
      Assert.That(result.Slides[0].IsClosed, Is.True);
      Assert.That(result.Slides[0].Questions.Count, Is.EqualTo(1));
      Assert.That(result.Slides[0].Questions[0].Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7:0"));
      Assert.That(result.Slides[0].Questions[0].Version, Is.EqualTo("1"));
      Assert.That(result.Slides[0].Questions[0].Type, Is.EqualTo("Monitor"));
      Assert.That(result.Slides[0].Questions[0].Input, Is.Not.Empty);
      Assert.That(result.Slides[0].Questions[0].Output.MultiValues, Is.Not.Empty);
      var events = result.Slides[0].Questions[0].Validation.MultiValueValidator[0];
      Assert.That(events.Id, Is.EqualTo("Events"));
      Assert.That(events.Min, Is.EqualTo(0));
      Assert.That(events.Max, Is.EqualTo(uint.MaxValue));
      Assert.That(events.ComplexValueValidator.Id, Is.EqualTo("Event"));
      Assert.That(events.ComplexValueValidator.SimpleValueValidators[0].Id, Is.EqualTo("DateTime"));
      Assert.That(events.ComplexValueValidator.SimpleValueValidators[0].Validation,
                  Is.EqualTo(@"(\d{4}-\d{2}-\d{2})T(\d{2}:\d{2}:\d{2}.\d{3})Z"));
      Assert.That(result.Slides[1].IsClosed, Is.False);
      Assert.That(result.Slides[1].Questions[0].Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7:1"));
      Assert.That(result.Slides[1].Questions[0].Type, Is.EqualTo("Freetext"));
      Assert.That(result.Slides[1].Questions[0].Validation.SimpleValueValidator, Is.Not.Empty);
      Assert.That(result.Slides[1].Questions[1].Type, Is.EqualTo("RadioButtonGroup"));
      Assert.That(result.Slides[2].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
      Assert.That(result.Slides[2].Questions[0].Validation.MultiValueValidator[0].SimpleValueValidator.Id,
                  Is.EqualTo("Id"));
      Assert.That(result.Slides[2].Questions[0].Validation.MultiValueValidator[0].SimpleValueValidator.Validation,
                  Is.EqualTo(".*"));
      Assert.That(result.Slides[3].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
      Assert.That(result.Slides[4].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
      Assert.That(result.Slides[5].Questions[0].Type, Is.EqualTo("OneDScale"));
      Assert.That(result.Slides[6].Questions[0].Type, Is.EqualTo("TwoDKScaleDD"));
      Assert.That(result.Slides[7].Questions[0].Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7:8"));
      Assert.That(result.Slides[7].Questions[0].Type, Is.EqualTo("Monitor"));
    }

    [Test]
    public void Deserialize_GivenQuestionWithComplexValue_ValueAndValidationShouldBeParsed()
    {
      var converter = new DtuFormatConverter();
      var xml = XDocument.Load("Resources\\experiment5.xml");

      var result = converter.Deserialize(xml);
      
      Assert.That(result.Slides[0].Questions[0].Validation.ComplexValueValidator.Count, Is.EqualTo(1));
      Assert.That(result.Slides[0].Questions[0].Output.ComplexValues.Count, Is.EqualTo(1));

      var q = QuestionnaireBuilder.MakeDto(result);
      Assert.That(q.Slides[0].Questions[0].Values.Any(item => item.Key == "Response"), Is.True);
    }

    [Test]
    public void Serialize_GivenQuestionWithComplexValue_SerializeComplexTypesToo()
    {
      var converter = new DtuFormatConverter();
      var xml = XDocument.Load("Resources\\experiment5.xml");

      var result = converter.Deserialize(xml);
      var result2 = converter.Serialize(result);

      Assert.That(result2.Descendants("DRQuestionCore").First().Element("Outputs").Element("Validation").Element("ComplexValue"), Is.Not.Null);
      Assert.That(result2.Descendants("DRQuestionCore").First().Element("Outputs").Element("Value").Element("Response"), Is.Not.Null);
      Assert.That(result2.Descendants("DRQuestionCore").First().Element("Outputs").Element("Value").Element("Response").Element("FreeText"), Is.Not.Null);
      Assert.That(result2.Descendants("DRQuestionCore").First().Element("Outputs").Element("Value").Element("Response").Element("Grading"), Is.Not.Null);
    }

    [Test]
    public void Validate()
    {
      var question = new Question("Monitor");
      question.Validation.MultiValueValidator = new List<MultiValueValidator>()
        {
          new MultiValueValidator
            {
              Id = "Events",
              Min = 0,
              Max = 10,
              ComplexValueValidator = new ComplexValueValidator
                {
                  Id = "Event",
                  SimpleValueValidators = new List<SimpleValueValidator>()
                    {
                      new SimpleValueValidator
                        {
                          Id = "DateTime",
                          Validation = @"(\d{4}-\d{2}-\d{2})T(\d{2}:\d{2}:\d{2}.\d{3})Z"
                        },
                      new SimpleValueValidator
                        {
                          Id = "Type",
                          Validation = @".*"
                        }
                    }
                }
            }
        };
      question.Output = new Output
        {
          MultiValues = new List<MultiValue>()
            {
              new MultiValue
                {
                  Key = "Events",
                  ComplexValues = new List<ComplexValue>()
                    {
                      new ComplexValue
                        {
                          Key = null,
                          SimpleValues = new List<SimpleValue>()
                            {
                              new SimpleValue("DateTime", "2015-02-12T16:00:00.000Z"),
                              new SimpleValue("Type", null)
                            }
                        }
                    }
                }
            }
        };
    }

    [Test]
    public void Serialize_GivenQuestionair_ReturnXml()
    {
      var converter = new DtuFormatConverter();
      var xml = XDocument.Load("Resources\\experiment.xml");
      var expected = converter.Deserialize(xml);

      var actual = converter.Serialize(expected);

      var root = actual.Root;
      Assert.That(root.Name.LocalName, Is.EqualTo("Experiment"));
      Assert.That(root.Element("Id").Value, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7"));
      Assert.That(root.Element("Name").Value, Is.EqualTo("DTU:Test:001"));
      Assert.That(root.Element("Version").Value, Is.EqualTo("1"));
      Assert.That(root.Element("Target").Attribute("Id").Value, Is.EqualTo("fba94e48-3c6d-4a2a-aef9-23108ee89ac0"));
      Assert.That(root.Element("Target").Attribute("Name").Value, Is.EqualTo("Jens Jensen"));
      Assert.That(root.Element("Trials").HasElements, Is.True);
      Func<int, XElement> trial = (index) => root.Element("Trials").Descendants("Trial").ToList()[index];
      Assert.That(trial(0).Attribute("TaskId").Value, Is.EqualTo("0"));
      Assert.That(trial(0).Attribute("IsClosed").Value, Is.EqualTo("true"));
      Assert.That(trial(1).Attribute("IsClosed").Value, Is.EqualTo("false"));
      Func<int, int, XElement> question =
        (trialIndex, questionIndex) => trial(trialIndex).Elements().ToList()[questionIndex];
      Assert.That(question(0, 0).Attribute("Version").Value, Is.EqualTo("1"));
      Assert.That(question(0, 0).Element("Inputs").HasElements, Is.True);
      Func<int, int, string, XElement> validation =
        (trialIndex, questionIndex, id) =>
        question(trialIndex, questionIndex)
          .Element("Outputs")
          .Element("Validation")
          .Elements()
          .Single(ele => ele.Attribute("Id").Value == id);
      Assert.That(validation(0, 0, "Events").Attribute("Max").Value, Is.EqualTo("Inf"));
      Assert.That(validation(0, 0, "Events").Attribute("Min").Value, Is.EqualTo("0"));
      Assert.That(validation(0, 0, "Events").Element("ComplexValue").Attribute("Id").Value, Is.EqualTo("Event"));
      Assert.That(validation(0, 0, "Events").Element("ComplexValue").Elements().First().Attribute("Id").Value,
                  Is.EqualTo("DateTime"));
      Assert.That(validation(1, 0, "Text").Attribute("Validation").Value, Is.EqualTo(".*"));
      Func<int, int, string, XElement> value =
        (trialIndex, questionIndex, id) =>
        question(trialIndex, questionIndex).Element("Outputs").Element("Value").Element(id);
      Assert.That(value(0, 0, "Events").Elements("Event").First().Element("Id").Value, Is.EqualTo(".\\"));
    }

    [Test]
    public void SerializeAfterDeserialize_XmlShouldStillBeValid()
    {
      var converter = new DtuFormatConverter();
      var questionnaire = TestResources.Make_Questionnaire();
      questionnaire.GetQuestion("6a0fae3a-2ac0-477b-892a-b24433ff3bd2:4").Output = new Output
        {
          SimpleValues = new[] {new SimpleValue("Text", "Mars")}
        };

      var serialized = converter.Serialize(questionnaire);
      var deserialized = converter.Deserialize(serialized);

      Assert.That(serialized.ToString().Contains("Mars"), Is.True);
      Assert.That(deserialized.GetQuestion("6a0fae3a-2ac0-477b-892a-b24433ff3bd2:4").Output.SimpleValues.First().Key,
                  Is.EqualTo("Text"));
    }

    [Test]
    public void ComplexMultiValue_ValidateCorrectly()
    {
      var questionnaire = TestResources.Make_Questionnaire();

      var question = questionnaire.GetQuestion("6a0fae3a-2ac0-477b-892a-b24433ff3bd2:0");

      question.Output = new Output
        {
          MultiValues = new[]
            {
              new MultiValue
                {
                  Key = "Events",
                  ComplexValues = new[]
                    {
                      new ComplexValue
                        {
                          SimpleValues = new[]
                            {
                              new SimpleValue("Id", " "),
                              new SimpleValue("Type", "Trial Start"),
                              new SimpleValue("Method", " "),
                              new SimpleValue("Data", " "),
                              new SimpleValue("DateTime", "2015-04-03T08:37:27.805Z"),
                            }
                        }
                    }
                }
            }
        };
    }

    [Test]
    public void Serialize_MultiValueComplexValueDoesntHaveAKey_UseItemAsKey()
    {
      var questionnaire = TestResources.Make_Questionnaire();

      var question = questionnaire.GetQuestion("6a0fae3a-2ac0-477b-892a-b24433ff3bd2:0");

      question.Output = new Output
        {
          MultiValues = new[]
            {
              new MultiValue
                {
                  Key = "Events",
                  ComplexValues = new[]
                    {
                      new ComplexValue
                        {
                          SimpleValues = new[]
                            {
                              new SimpleValue("Id", " "),
                              new SimpleValue("Type", "Trial Start"),
                              new SimpleValue("Method", " "),
                              new SimpleValue("Data", " "),
                              new SimpleValue("DateTime", "2015-04-03T08:37:27.805Z"),
                            }
                        }
                    }
                }
            }
        };

      var result = new DtuFormatConverter().Serialize(questionnaire);

      Assert.That(
        result.Descendants("Outputs").First().Descendants("Events").First().Elements().All(item => item.Name == "Item"),
        Is.True);
    }
  }
}