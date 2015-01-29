using System;
using System.Linq;
using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Data.InMemory
{
  public class QuestionnaireGateway : EntityRepository<Questionnaire>
  {
    public Questionnaire Get(string id)
    {
      return Retrieve(id);
    }

    public Questionnaire GetByQuestionId(string identifier)
    {
      foreach (var questionnaire in Retrieve().Where(questionnaire => questionnaire.Slides.Any(slide => slide.Questions.Any(question => question.Id == identifier))))
        return questionnaire;

      throw new Exception("No data by the given Id");
    }
  }
}