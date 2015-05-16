using System.Linq;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
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
      var claimedList = Make_ClaimedList();

      var next = claimedList.Next();

      Assert.That(next.Id, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
      Assert.That(next.ClaimedOnDate, Is.Not.Null);
      Assert.That(next.ClaimedOnDate, Is.Not.Empty);
    }

    [Test]
    public void Next_GivenCalledOnce_ReturnFirstItem()
    {
      var claimedList = Make_ClaimedList();

      var next = claimedList.Next();

      Assert.That(next.Id, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
      Assert.That(next.ClaimedOnDate, Is.Not.Null);
      Assert.That(next.ClaimedOnDate, Is.Not.Empty);
    }

    [Test]
    public void Next_GivenCalledTwice_ReturnSecondItem()
    {
      var claimedList = Make_ClaimedList();

      claimedList.Next();
      var next = claimedList.Next();

      Assert.That(next.Id, Is.EqualTo("00000000-0000-0000-0000-000000000002"));
      Assert.That(next.ClaimedOnDate, Is.Not.Null);
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
      var claimedList = new ClaimedList(xml);
      return claimedList;
    }
  }
}
