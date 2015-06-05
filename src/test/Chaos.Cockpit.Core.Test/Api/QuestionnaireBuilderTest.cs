using System.Collections.Generic;
using Chaos.Cockpit.Core.Api;
using Chaos.Cockpit.Core.Core;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api
{
  [TestFixture]
  public class QuestionnaireBuilderTest
  {
    [Test]
    public void MakeDto_GivenQuestionnaireWithSlides_CurrentSlideIndexIsOne()
    {
      var q = new Questionnaire
        {
          Slides = new List<Slide>
            {
              new Slide { IsCompleted = true },
              new Slide { IsCompleted = false }
            }
        };

      var result = QuestionnaireBuilder.MakeDto(q);

      Assert.That(result.CurrentSlideIndex, Is.EqualTo(1));
    }
  }
}