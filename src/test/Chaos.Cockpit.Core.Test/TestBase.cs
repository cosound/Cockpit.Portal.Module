using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.InMemory;
using Chaos.Portal.Core;
using Moq;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test
{
  public class TestBase
  {
    protected Mock<IPortalApplication> PortalApplication;

    [SetUp]
    public void SetUp()
    {
      PortalApplication = new Mock<IPortalApplication>();

      Context.QuestionGateway = new QuestionGateway();
      Context.QuestionnaireGateway = new QuestionnaireGateway();
    }
  }
}