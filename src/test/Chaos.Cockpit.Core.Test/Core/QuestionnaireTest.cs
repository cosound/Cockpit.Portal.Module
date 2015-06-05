using System.Collections.Generic;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Exceptions;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Core
{
  [TestFixture]
  public class QuestionnaireTest
  {
    [Test, ExpectedException(typeof(DataNotFoundException))]
    public void NextSlide_GivenNoSlides_Throw()
    {
      var q = new Questionnaire();

      q.NextSlide();
    }

    [Test]
    public void NextSlide_GivenOneOpenSlide_ReturnSlideIndex()
    {
      var q = new Questionnaire
        {
          Slides = new List<Slide>
            {
              new Slide()
            }
        };

      var index = q.NextSlide();

      Assert.That(index, Is.EqualTo(0));
    }
    
    [Test]
    public void NextSlide_GivenTwoQuestion_ReturnSlideIndex()
    {
      var q = new Questionnaire
        {
          Slides = new List<Slide>
            {
              new Slide { IsCompleted = true },
              new Slide { IsCompleted = false }
            }
        };

      var index = q.NextSlide();

      Assert.That(index, Is.EqualTo(1));
    }
    
    [Test]
    public void NextSlide_GivenFiveQuestion_ReturnSlideIndex()
    {
      var q = new Questionnaire
        {
          Slides = new List<Slide>
            {
              new Slide { IsCompleted = true },
              new Slide { IsCompleted = true },
              new Slide { IsCompleted = true },
              new Slide { IsCompleted = false },
              new Slide { IsCompleted = false }
            }
        };

      var index = q.NextSlide();

      Assert.That(index, Is.EqualTo(3));
    }
  }
}