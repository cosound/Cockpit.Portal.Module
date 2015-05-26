using System;
using System.Collections.Generic;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using Chaos.Cockpit.Core.Core.Validation;
using Chaos.Cockpit.Core.Data.Mcm;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  [TestFixture]
  public class AnswerTest : TestBase
  {
    [Test]
    public void Set_GivenNewAnswer_SetAnswerOnQuestion()
    {
      var extension = Make_AnswerExtension();
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
      var question = new Question("TestQuestion")
        {
          Id = "00000000-0000-0000-0000-000000000001:0",
          Version = "",
          Validation = new OutputValidator
            {
              ComplexValueValidator = new []
                {
                  new ComplexValueValidator
                    {
                      Id = "k1",
                      SimpleValueValidators = new []
                        {
                          new SimpleValueValidator
                            {
                              Id = "k2",
                              Validation = ".*"
                            }
                        }
                    }
                }
            }
        };
      Context.QuestionnaireGateway.Set(new Questionnaire
        {
          Id = "00000000-0000-0000-0000-000000000001",
          Name = "Test",
          TargetId = "",
          TargetName = "1",
          Slides = new List<Slide>
            {
              new Slide
                {
                  TaskId = "1",
                  Questions = new List<Question>
                    {
                      question
                    }
                }
            }
        });

      extension.Set(question.Id, answer);

      var actual = Context.QuestionGateway.Get(question.Id);
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

    [Test, ExpectedException(typeof(SlideClosedException))]
    public void Set_GivenAnonymousUserAndSlideIsClosed_Throw()
    {
      PortalRequest.Setup(p => p.IsAnonymousUser).Returns(true);
      var extension = Make_AnswerExtension();
      var question = new Question("TestQuestion")
        {
          Id = "00000000-0000-0000-0000-000000000001:0"
        };
      Context.QuestionnaireGateway.Set(new Questionnaire
      {
        Id = "00000000-0000-0000-0000-000000000001",
        Name = "Test",
        Slides = new List<Slide>
            {
              new Slide
                {
                  IsClosed = true,
                  Questions = new List<Question>
                    {
                      question
                    }
                }
            }
      });

      extension.Set("00000000-0000-0000-0000-000000000001:0", null);
    }

    private AnswerExtension Make_AnswerExtension()
    {
      return (AnswerExtension) new AnswerExtension(PortalApplication.Object).WithPortalRequest(PortalRequest.Object);
    }
  }
}