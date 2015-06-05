using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  [TestFixture]
  public class QuestionnaireTest : TestBase
  {
    [Test]
    public void Get_GivenOnlyId_ReturnFirstSlide()
    {
      var extension = Make_QuestionExtension();
      var questionnaire = Context.QuestionnaireGateway.Set(new Questionnaire
          {
            Slides = new[] {new Slide {Questions = new[] {new Question("TestQuestion")}}}
          });

      var page = extension.Get(questionnaire.Id);

      Assert.That(page.Results, Is.Not.Empty);
    }

    [Test]
    public void Get_GivenKnownUserAndSlideIsClosed_ReturnFirstSlide()
    {
      PortalRequest.Setup(p => p.IsAnonymousUser).Returns(false);
      var extension = Make_QuestionExtension();
      var questionnaire = Context.QuestionnaireGateway.Set(new Questionnaire
      {
        Slides = new[] { new Slide { IsCompleted = true, Questions = new[] { new Question("TestQuestion") } } }
      });

      var page = extension.Get(questionnaire.Id);

      Assert.That(page.Results, Is.Not.Empty);
    }
    
    [Test, ExpectedException(typeof(SlideLockedException))]
    public void Get_GivenAnonymousUserAndSlideIsClosed_Throw()
    {
      PortalRequest.Setup(p => p.IsAnonymousUser).Returns(true);
      var extension = Make_QuestionExtension();
      var questionnaire = Context.QuestionnaireGateway.Set(new Questionnaire
          {
            LockQuestion = true,
            Slides = new[] {new Slide { IsCompleted = true, Questions = new[] {new Question("TestQuestion")}}}
          });

      extension.Get(questionnaire.Id);
    }

    private QuestionExtension Make_QuestionExtension()
    {
      return (QuestionExtension) new QuestionExtension(PortalApplication.Object).WithPortalRequest(PortalRequest.Object);
    }
  }
}