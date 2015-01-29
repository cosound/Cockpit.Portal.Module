using System;
using System.Linq;
using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Data.InMemory
{
  public class QuestionGateway : EntityRepository<Question>
  {
    public Question Save(Question entity)
    {
      var questionnaire = CockpitContext.QuestionnaireGateway.GetByQuestionId(entity.Id);
      var question = entity;

      foreach (var slide in questionnaire.Slides)
        for (var i = 0; i < slide.Questions.Count; i++)
          if (slide.Questions[i].Id == entity.Id)
            slide.Questions[i] = question;

      CockpitContext.QuestionnaireGateway.Set(questionnaire);

      return question;
    }

    public Question Get(string id)
    {
      return GetQuestionFromQuestionnaire(id);
    }

    private Question GetQuestionFromQuestionnaire(string id)
    {
      var questionnaire = CockpitContext.QuestionnaireGateway.GetByQuestionId(id);

      foreach (var slide in questionnaire.Slides)
        foreach (var question in slide.Questions.Where(t => t.Id == id))
          return question;

      throw new Exception("No data by the given Id");
    }

  }
}