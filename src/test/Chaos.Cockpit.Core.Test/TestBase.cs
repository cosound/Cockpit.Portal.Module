using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.InMemory;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Request;
using Moq;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test
{
  public class TestBase
  {
    protected Mock<IPortalApplication> PortalApplication;
    protected Mock<IPortalRequest> PortalRequest;

    [SetUp]
    public void SetUp()
    {
      PortalApplication = new Mock<IPortalApplication>();
      PortalRequest = new Mock<IPortalRequest>();

      Context.QuestionGateway = new QuestionGateway();
      Context.QuestionnaireGateway = new QuestionnaireGateway();
    }
  }
}