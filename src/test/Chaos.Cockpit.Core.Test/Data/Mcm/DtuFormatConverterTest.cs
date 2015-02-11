using System.Xml.Linq;

namespace Chaos.Cockpit.Core.Test.Data.Mcm
{
  using Cockpit.Core.Data.Mcm;
  using NUnit.Framework;

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
       Assert.That(result.Slides[0].Questions[0].Type, Is.EqualTo("Monitor"));
       Assert.That(result.Slides[0].Questions[0].Input, Is.Not.Empty);
       var events = result.Slides[0].Questions[0].Validation.MultiValueValidator[0];
       Assert.That(events.Id, Is.EqualTo("Events"));
       Assert.That(events.Min, Is.EqualTo(0));
       Assert.That(events.Max, Is.EqualTo(uint.MaxValue));
       Assert.That(events.ComplexValueValidators[0].Id, Is.EqualTo("Event"));
       Assert.That(events.ComplexValueValidators[0].SimpleValueValidators[0].Id, Is.EqualTo("DateTime"));
       Assert.That(events.ComplexValueValidators[0].SimpleValueValidators[0].Validation, Is.EqualTo(@"(\d{4}-\d{2}-\d{2})T(\d{2}:\d{2}:\d{2}.\d{3})Z"));
       Assert.That(result.Slides[1].Questions[0].Type, Is.EqualTo("Freetext"));
       Assert.That(result.Slides[1].Questions[1].Type, Is.EqualTo("RadioButtonGroup"));
       Assert.That(result.Slides[2].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
       Assert.That(result.Slides[2].Questions[0].Validation.MultiValueValidator[0].SimpleValueValidators[0].Id, Is.EqualTo("Id"));
       Assert.That(result.Slides[2].Questions[0].Validation.MultiValueValidator[0].SimpleValueValidators[0].Validation, Is.EqualTo(".+"));
       Assert.That(result.Slides[3].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
       Assert.That(result.Slides[4].Questions[0].Type, Is.EqualTo("CheckBoxGroup"));
       Assert.That(result.Slides[5].Questions[0].Type, Is.EqualTo("OneDScale"));
       Assert.That(result.Slides[6].Questions[0].Type, Is.EqualTo("TwoDKScaleDD"));
       Assert.That(result.Slides[7].Questions[0].Type, Is.EqualTo("Monitor"));
     }
  }
}