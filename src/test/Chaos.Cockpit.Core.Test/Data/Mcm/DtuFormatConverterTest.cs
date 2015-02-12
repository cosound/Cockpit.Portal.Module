﻿using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Validation;
using Chaos.Cockpit.Core.Data.Mcm;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Data.Mcm
{
  [TestFixture]
  public class DtuFormatConverterTest
  {
     [Test]
     public void Deserialize_GivenExperimentWithOneTask_ParseRequiredFields()
     {
       var converter = new DtuFormatConverter();
       var xml = XDocument.Load("Ressources\\experiment.xml");

       var result = converter.Deserialize(xml);

       Assert.That(result.Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7"));
       Assert.That(result.Name, Is.EqualTo("DTU:Test:001"));
       Assert.That(result.Slides.Count, Is.EqualTo(8));
       Assert.That(result.Slides[0].Questions.Count, Is.EqualTo(1));
       Assert.That(result.Slides[0].Questions[0].Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7:0"));
       Assert.That(result.Slides[0].Questions[0].Type, Is.EqualTo("Monitor"));
       Assert.That(result.Slides[0].Questions[0].Input, Is.Not.Empty);
       var events = result.Slides[0].Questions[0].Validation.MultiValueValidator[0];
       Assert.That(events.Id, Is.EqualTo("Events"));
       Assert.That(events.Min, Is.EqualTo(0));
       Assert.That(events.Max, Is.EqualTo(uint.MaxValue));
       Assert.That(events.ComplexValueValidator.Id, Is.EqualTo("Event"));
       Assert.That(events.ComplexValueValidator.SimpleValueValidators[0].Id, Is.EqualTo("DateTime"));
       Assert.That(events.ComplexValueValidator.SimpleValueValidators[0].Validation, Is.EqualTo(@"(\d{4}-\d{2}-\d{2})T(\d{2}:\d{2}:\d{2}.\d{3})Z"));
       Assert.That(result.Slides[1].Questions[0].Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7:1"));
       Assert.That(result.Slides[1].Questions[0].Type, Is.EqualTo("Freetext"));
       Assert.That(result.Slides[1].Questions[1].Type, Is.EqualTo("RadioButtonGroup"));
       Assert.That(result.Slides[2].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
       Assert.That(result.Slides[2].Questions[0].Validation.MultiValueValidator[0].SimpleValueValidator.Id, Is.EqualTo("Id"));
       Assert.That(result.Slides[2].Questions[0].Validation.MultiValueValidator[0].SimpleValueValidator.Validation, Is.EqualTo(".+"));
       Assert.That(result.Slides[3].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
       Assert.That(result.Slides[4].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
       Assert.That(result.Slides[5].Questions[0].Type, Is.EqualTo("OneDScale"));
       Assert.That(result.Slides[6].Questions[0].Type, Is.EqualTo("TwoDKScaleDD"));
       Assert.That(result.Slides[7].Questions[0].Id, Is.EqualTo("ea3976c7-2d4a-4ef0-84a1-1d9e66f4a0e7:8"));
       Assert.That(result.Slides[7].Questions[0].Type, Is.EqualTo("Monitor"));
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
               Min = 0, Max = 10,
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

  }
}