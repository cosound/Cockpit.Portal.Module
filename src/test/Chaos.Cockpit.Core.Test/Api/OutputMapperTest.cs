using System;
using System.Collections.Generic;
using Chaos.Cockpit.Core.Api;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Validation;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api
{
  [TestFixture]
  public class OutputMapperTest
  {
    [Test]
    public void Map()
    {
      var output = new Output()
        {
          SimpleValues = new List<SimpleValue>()
            {
              new SimpleValue("k1", "v1"),
              new SimpleValue("k2", "v2")
            },
          ComplexValues = new List<ComplexValue>()
            {
              new ComplexValue
                {
                  Key = "k3",
                  SimpleValues = new List<SimpleValue>()
                    {
                      new SimpleValue("k3.1", "v3.1"),
                      new SimpleValue("k3.2", "v3.2")
                    }
                }
            },
            MultiValues = new List<MultiValue>()
              {
                new MultiValue
                  {
                    Key = "k4",
                    SimpleValues = new List<string>()
                      {
                        "v4.1","v4.2"
                      }
                  },
                  new MultiValue()
                    {
                      Key = "k5",
                      ComplexValues = new List<ComplexValue>()
                        {
                          new ComplexValue
                            {
                              Key = "k5.1",
                              SimpleValues = new List<SimpleValue>()
                                {
                                  new SimpleValue("k5.1.1", "v5.1.1"),
                                  new SimpleValue("k5.1.2", "v5.1.2")
                                }
                            }
                        }
                    }
              }
        };

      var result = OutputMapper.Map(output);

      Assert.That(result.SingleValues["k1"], Is.EqualTo("v1"));
      Assert.That(result.SingleValues["k2"], Is.EqualTo("v2"));
      Assert.That(result.ComplexValues["k3"].SingleValues["k3.1"], Is.EqualTo("v3.1"));
      Assert.That(result.ComplexValues["k3"].SingleValues["k3.2"], Is.EqualTo("v3.2"));
      Func<string, int, string> multiSingle = (key, i) => (result.MultiValues[key] as MultiSingleValueResult).SingleValues[i];
      Assert.That(multiSingle("k4", 0), Is.EqualTo("v4.1"));
      Assert.That(multiSingle("k4", 1), Is.EqualTo("v4.2"));
      Func<string, int, string, string> multiComplex = (key, i, field) => (result.MultiValues[key] as MultiComplexValueResult).ComplexValues[i].SingleValues[field];
      Assert.That(multiComplex("k5", 0, "k5.1.1"), Is.EqualTo("v5.1.1"));
      Assert.That(multiComplex("k5", 0, "k5.1.2"), Is.EqualTo("v5.1.2"));
    }
  }
}