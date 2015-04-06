using System;
using System.Linq;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
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

    [Test, ExpectedException(ExpectedExceptionName = "Chaos.Portal.Core.Exceptions.InsufficientPermissionsException")]
    public void Get_AnonymousUser_Throw()
    {
      PortalRequest.Setup(p => p.IsAnonymousUser).Returns(true);
      var extension = Make_SelectionExtension();

      extension.Get(null);
    }

    [Test, ExpectedException(ExpectedExceptionName = "Chaos.Cockpit.Core.Core.Exceptions.DataNotFoundException")]
    public void Get_SelectionDoesntExist_Throw()
    {
      var extension = Make_SelectionExtension();

      extension.Get("Missing");
    }

    [Test]
    public void Get_SelectionExist_ReturnSelectionResult()
    {
      var selection = new Selection{Id = "id", Name = "name"};
      Context.SelectionGateway.Set(selection);
      var extension = Make_SelectionExtension();

      var result = extension.Get("id");

      Assert.That(result.Identifier, Is.EqualTo(selection.Id));
      Assert.That(result.Name, Is.EqualTo(selection.Name));
    }

    [Test, ExpectedException(ExpectedExceptionName = "Chaos.Portal.Core.Exceptions.InsufficientPermissionsException")]
    public void Delete_AnonymousUser_Throw()
    {
      PortalRequest.Setup(p => p.IsAnonymousUser).Returns(true);
      var extension = Make_SelectionExtension();

      extension.Delete(null);
    }

    [Test]
    public void Delete_SelectionExist_DeleteOnGateway()
    {
      var selection = new Selection { Id = "id", Name = "name" };
      Context.SelectionGateway.Set(selection);
      var extension = Make_SelectionExtension();

      var result = extension.Delete("id");

      Assert.That(result.WasSuccess, Is.True);
      try
      {
        Context.SelectionGateway.Get("id");
        Assert.Fail();
      }
      catch (DataNotFoundException)
      {
      }
    }

    [Test]
    public void Delete_SelectionDoesntExist_ReturnFailed()
    {
      var extension = Make_SelectionExtension();

      var result = extension.Delete("missing");

      Assert.That(result.WasSuccess, Is.False);
    }

    private SelectionExtension Make_SelectionExtension()
    {
      return (SelectionExtension) new SelectionExtension(PortalApplication.Object).WithPortalRequest(PortalRequest.Object);
    }
  }
}