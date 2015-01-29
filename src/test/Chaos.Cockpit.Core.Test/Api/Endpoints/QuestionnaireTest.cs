using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  [TestFixture]
  public class QuestionnaireTest : TestBase
  {
    [Test]
    public void Get_GivenOnlyId_ReturnFirstSlide()
    {
      var extension = new QuestionExtension(PortalApplication.Object);
      var questionnaire = CockpitContext.QuestionnaireGateway.Set(new Questionnaire
          {
            Slides = new[] {new Slide {Questions = new[] {new Question("TestQuestion")}}}
          });

      var page = extension.Get(questionnaire.Id);

      Assert.That(page.Results, Is.Not.Empty);
    }
  }
}