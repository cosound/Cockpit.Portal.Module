using System;
using Chaos.Cockpit.Core.Main;
using Chaos.Portal.Core.Extension;
using Moq;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Main
{
  [TestFixture]
  public class CockpitModuleTest : TestBase
  {
    [Test]
    public void Load()
    {
      var module = new CockpitModule();

      module.Load(PortalApplication.Object);

      PortalApplication.Verify(m => m.MapRoute("/v6/Question", It.IsAny<Func<IExtension>>()));
      PortalApplication.Verify(m => m.MapRoute("/v6/Answer", It.IsAny<Func<IExtension>>()));
      PortalApplication.Verify(m => m.MapRoute("/v6/Selection", It.IsAny<Func<IExtension>>()));
    }
  }
}