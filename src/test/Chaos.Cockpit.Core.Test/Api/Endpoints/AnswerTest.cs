using System.Collections.Generic;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  [TestFixture]
  public class AnswerTest : TestBase
  {
    [Test]
    public void Set_GivenNewAnswer_SetAnswerOnQuestion()
    {
      var extension = new AnswerExtension(PortalApplication.Object);
      var answer = new OutputDto
        {
          ComplexValues = new Dictionary<string, ComplexValueResult>()
            {
              {
                "k1", new ComplexValueResult
                  {
                    SingleValues = new Dictionary<string, string>()
                      {
                        {"k2", "v2"}
                      }
                  }
              },
              {
                "k3", new ComplexValueResult
                  {
                    SingleValues = new Dictionary<string, string>()
                      {
                        {"k4", "v4"},
                        {"k5", "v5"}
                      }
                  }
              }
            },
          SingleValues = new Dictionary<string, string>()
            {
              {"k6", "v6"}
            },
          MultiValues = new Dictionary<string, IMultiValueResult>()
            {
              {
                "k7",
                new MultiSingleValueResult
                  {
                    SingleValues = new List<string>()
                      {
                        "v7.1",
                        "v7.2"
                      }
                  }
              },
              {
                "k8",
                new MultiComplexValueResult
                  {
                    ComplexValues = new List<ComplexValueResult>()
                      {
                        new ComplexValueResult
                          {
                            SingleValues = new Dictionary<string, string>()
                              {
                                {"k8.1","v8.1"},
                                {"k8.2","v8.2"}
                              }
                          }
                      }
                  }
              }
            }
        };
      var question = new Question("TestQuestion");
      CockpitContext.QuestionnaireGateway.Set(new Questionnaire
        {
          Name = "Test",
          Slides = new List<Slide>
            {
              new Slide
                {
                  Questions = new List<Question>
                    {
                      question
                    }
                }
            }
        });

      extension.Set(question.Id, answer);

      var actual = CockpitContext.QuestionGateway.Get(question.Id);
      Assert.That(actual.Output.ComplexValues[0].Key, Is.EqualTo("k1"));
      Assert.That(actual.Output.ComplexValues[0].SimpleValues[0].Key, Is.EqualTo("k2"));
      Assert.That(actual.Output.ComplexValues[0].SimpleValues[0].Value, Is.EqualTo("v2"));
      Assert.That(actual.Output.ComplexValues[1].Key, Is.EqualTo("k3"));
      Assert.That(actual.Output.ComplexValues[1].SimpleValues[0].Key, Is.EqualTo("k4"));
      Assert.That(actual.Output.ComplexValues[1].SimpleValues[0].Value, Is.EqualTo("v4"));
      Assert.That(actual.Output.ComplexValues[1].SimpleValues[1].Key, Is.EqualTo("k5"));
      Assert.That(actual.Output.ComplexValues[1].SimpleValues[1].Value, Is.EqualTo("v5"));
      Assert.That(actual.Output.SimpleValues[0].Key, Is.EqualTo("k6"));
      Assert.That(actual.Output.SimpleValues[0].Value, Is.EqualTo("v6"));
      Assert.That(actual.Output.MultiValues[0].Key, Is.EqualTo("k7"));
      Assert.That(actual.Output.MultiValues[0].SimpleValues[0], Is.EqualTo("v7.1"));
      Assert.That(actual.Output.MultiValues[0].SimpleValues[1], Is.EqualTo("v7.2"));
      Assert.That(actual.Output.MultiValues[1].Key, Is.EqualTo("k8"));
      Assert.That(actual.Output.MultiValues[1].ComplexValues[0].SimpleValues[0].Key, Is.EqualTo("k8.1"));
      Assert.That(actual.Output.MultiValues[1].ComplexValues[0].SimpleValues[1].Key, Is.EqualTo("k8.2"));
    }
  }
}