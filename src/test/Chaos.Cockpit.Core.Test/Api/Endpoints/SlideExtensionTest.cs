using System;
using System.Collections.Generic;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
  [TestFixture]
  public class SlideExtensionTest : TestBase
  {
    [Test]
    public void Close_GivenQuestionaireExistsAndHasSlide_CloseTheSlide()
    {
      var ext = new SlideExtension(PortalApplication.Object);
      var id = Guid.Parse("00000000-0000-0000-0000-000000000001");
      Context.QuestionnaireGateway.Set(new Questionnaire
      {
        Id = id.ToString(),
        Name = "Test",
        Slides = new List<Slide>
            {
              new Slide
                {
                  IsClosed = false
                }
            }
      });

      ext.Close(id, 0);

      var actual = Context.QuestionnaireGateway.Get(id);
      Assert.That(actual.Slides[0].IsClosed, Is.True);
    }
  }
}
