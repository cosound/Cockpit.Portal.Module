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

      return Data[id];
    }

    protected override Questionnaire Copy(Questionnaire entity)
    {
      var copy = new Questionnaire();
      copy.Identity = entity.Identity;
      copy.Name = entity.Name;
      copy.Slides = entity.Slides;

      return copy;
    }
  }
}