using System.Linq;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.InMemory;
using NUnit.Framework;
using Item = Chaos.Cockpit.Core.Api.Result.Item;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  [TestFixture]
  public class SelectionExtensionTest : TestBase
  {
    [SetUp]
    public void SetUp()
    {
      Context.SelectionGateway = new SelectionGateway();
    }

    [Test, ExpectedException(ExpectedExceptionName = "Chaos.Portal.Core.Exceptions.InsufficientPermissionsException")]
    public void Set_AnonymousUser_Throw()
    {
      PortalRequest.Setup(p => p.IsAnonymousUser).Returns(true);
      var extension = Make_SelectionExtension();

      extension.Set(null);
    }

    [Test]
    public void Set_NewSelection_ReturnTrue()
    {
      var extension = Make_SelectionExtension();

      var result = extension.Set(new SelectionResult());

      Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Set_NewSelection_CallGateway()
    {
      var extension = Make_SelectionExtension();
      var selection = new SelectionResult {Name = "Selection name", Items = new []{new Item{Identifier = "1"}}};

      var result = extension.Set(selection);

      Assert.That(result.Identifier, Is.Not.Null);
      Assert.That(result.Name, Is.EqualTo(result.Name));
      Assert.That(result.Items.First().Identifier, Is.EqualTo(result.Items.First().Identifier));
    }

    private SelectionExtension Make_SelectionExtension()
    {
      return (SelectionExtension) new SelectionExtension(PortalApplication.Object).WithPortalRequest(PortalRequest.Object);
    }
  }
}