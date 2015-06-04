using System;
using System.Linq;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  [TestFixture]
  public class ExperimentExtensionTest : TestBase
  {
    [Test]
    public void Next_()
    {
      var ext = new ExperimentExtension(PortalApplication.Object);
      Context.ExperimentGateway.Save(Make_ClaimedList());

      var result = ext.Next(Guid.Parse("00000000-0000-0000-0000-000000000001"));

      Assert.That(result.Id, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
      Assert.That(result.ClaimedOnDate, Is.Not.Empty);
    }

    [Test]
    public void Next_GivenCalledOnce_ReturnFirstItem()
    {
      var claimedList = Make_ClaimedList();

      var next = claimedList.Next();

      Assert.That(next.Id, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
      Assert.That(next.ClaimedOnDate, Is.Not.Empty);
    }

    [Test]
    public void Next_GivenCalledTwice_ReturnSecondItem()
    {
      var claimedList = Make_ClaimedList();

      claimedList.Next();
      var next = claimedList.Next();

      Assert.That(next.Id, Is.EqualTo("00000000-0000-0000-0000-000000000002"));
      Assert.That(next.ClaimedOnDate, Is.Not.Empty);
    }

    [Test]
    public void ToXml_GivenCalledOnce_FirstItemHasClaimedDateSet()
    {
      var claimedList = Make_ClaimedList();
      claimedList.Next();

      var xml = claimedList.ToXml();

      Assert.That(xml.Root.Elements().First().Attribute("ClaimedOnDate").Value, Is.Not.Empty);
      Assert.That(xml.Root.Elements().Last().Attribute("ClaimedOnDate").Value, Is.Empty);
    }

    private static ClaimedList Make_ClaimedList()
    {
      var xml =
        XDocument.Parse(
          "<Experiments><Item Id='00000000-0000-0000-0000-000000000001' ClaimedOnDate='' /><Item Id='00000000-0000-0000-0000-000000000002' ClaimedOnDate='' /></Experiments>");
      var claimedList = new ClaimedList("00000000-0000-0000-0000-000000000001", xml);
      return claimedList;
    }

    [Test, ExpectedException(typeof(DataNotFoundException))]
    public void Get_GivenMissingId_Throw()
    {
      var ext = new ExperimentExtension(PortalApplication.Object);

      ext.Get(Guid.Empty);
    }

    [Test]
    public void Get_GivenExistingId_Return()
    {
      var ext = new ExperimentExtension(PortalApplication.Object);
      var questionaire = new Questionnaire
        {
          Version = "1",
          Name = "name",
          Css = "url",
          LockQuestion = true,
          EnablePrevious = false,
          FooterLabel = "footer"
        };
      Context.QuestionnaireGateway.Set(questionaire);

      var result = ext.Get(Guid.Parse(questionaire.Id));

      Assert.That(result.Name, Is.EqualTo(questionaire.Name));
      Assert.That(result.Version, Is.EqualTo(questionaire.Version));
      Assert.That(result.Css, Is.EqualTo(questionaire.Css));
      Assert.That(result.LockQuestion, Is.EqualTo(questionaire.LockQuestion));
      Assert.That(result.EnablePrevious, Is.EqualTo(questionaire.EnablePrevious));
      Assert.That(result.FooterLabel, Is.EqualTo(questionaire.FooterLabel));
    }
  }
}
