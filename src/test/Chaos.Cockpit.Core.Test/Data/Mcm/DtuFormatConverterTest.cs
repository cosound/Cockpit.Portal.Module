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
       var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><CoSound.Experiment.Demo.Content.R001 xml_tb_version=\"3.1\">" +
                 "<Experiment><GUID>6a7f67aef678fea7f6afae79888a9f89e7fae</GUID><Id>DR:Crowdsourcing</Id><Version>R001</Version>" +
                 "<Parts><Part><Tasks><Task><Trials>" +
                 "<Trial><GUID>7a90c205-8999-46df-b3a9-01ca451e7154</GUID></Trial>" +
                 "</Trials></Task></Tasks></Part></Parts>" +
                 "</Experiment></CoSound.Experiment.Demo.Content.R001>";

       var result = converter.Deserialize(xml);

       Assert.That(result.Id, Is.EqualTo("7a90c205-8999-46df-b3a9-01ca451e7154"));
       Assert.That(result.Name, Is.EqualTo("DR:Crowdsourcing"));
     }
  }
}