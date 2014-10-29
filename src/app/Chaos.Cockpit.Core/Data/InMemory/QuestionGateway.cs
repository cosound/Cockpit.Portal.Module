using System.Linq;

namespace Chaos.Cockpit.Core.Data.InMemory
{
  using System;
  using Core;

  public class QuestionGateway : IEntityGateway<Question>
  {
    public Question Save(Question entity)
    {
      var questionnaire = CockpitContext.QuestionnaireGateway.GetByQuestionId(entity.Identifier);
      var question = Copy(entity);

      foreach (var slide in questionnaire.Slides)
        for (var i = 0; i < slide.Questions.Count; i++)
          if (slide.Questions[i].Identifier == entity.Identifier)
            slide.Questions[i] = question;

      CockpitContext.QuestionnaireGateway.Save(questionnaire);

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
        foreach (var question in slide.Questions.Where(t => t.Identifier == id))
          return Copy(question);

      throw new Exception("No data by the given Id");
    }

    protected Question Copy(Question entity)
    {
      var copy = new Question(entity.Type)
        {
          Identifier = entity.Identifier,
          UserAnswer = entity.UserAnswer == null
                         ? null
                         : Copy(entity.UserAnswer)
        };

      return copy;
    }

    private Answer Copy(Answer answer)
    {
      var copy = new Answer(answer.Type);
      copy.Identifier = answer.Identifier;
      copy.Data = answer.Data;

      return copy;
    }
  }
}