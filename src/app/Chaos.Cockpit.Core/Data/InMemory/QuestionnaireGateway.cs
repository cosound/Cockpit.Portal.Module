using System.Linq;

namespace Chaos.Cockpit.Core.Data.InMemory
{
  using System;
  using Core;

  public class QuestionnaireGateway : EntityGateway<Questionnaire>
  {
    public Questionnaire Get(string id)
    {
      if(!Data.ContainsKey(id))
        throw new Exception("No data by the given Id");

      return Copy(Data[id]);
    }

    protected override Questionnaire Copy(Questionnaire entity)
    {
      var copy = new Questionnaire();
      copy.Identifier = entity.Identifier;
      copy.Name = entity.Name;
      copy.Slides = entity.Slides;

      return copy;
    }

    public Questionnaire GetByQuestionId(string identifier)
    {
      foreach (var questionnaire in Data.Values.Where(questionnaire => questionnaire.Slides.Any(slide => slide.Questions.Any(question => question.Identifier == identifier))))
        return Copy(questionnaire);

      throw new Exception("No data by the given Id");
    }
  }
}