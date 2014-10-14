namespace Chaos.Cockpit.Core.Test.Main
{
  using System;
  using Cockpit.Core.Main;
  using Moq;
  using NUnit.Framework;
  using Portal.Core;
  using Portal.Core.Extension;

  [TestFixture]
  public class CockpitModuleTest
  {
    [Test]
    public void Load()
    {
      var module = new CockpitModule();
      var portalApplication = new Mock<IPortalApplication>();

      module.Load(portalApplication.Object);

      portalApplication.Verify(m => m.MapRoute("/v6/Questionnaire", It.IsAny<Func<IExtension>>()));
    }
  }
}