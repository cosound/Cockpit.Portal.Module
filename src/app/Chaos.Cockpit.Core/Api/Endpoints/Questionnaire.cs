namespace Chaos.Cockpit.Core.Api.Endpoints
{
  using Core;
  using Portal.Core;
  using Portal.Core.Extension;

  public class Questionnaire : AExtension
  {
    public Questionnaire(IPortalApplication portalApplication) : base(portalApplication)
    {
    }

    public Result.Questionnaire Get(string id)
    {
      var questionnaire = CockpitContext.QuestionnaireGateway.Get(id);

      return QuestionnaireBuilder.MakeDto(questionnaire);
    }
  }

  public static class QuestionnaireBuilder
  {
    public static Result.Questionnaire MakeDto(Core.Questionnaire questionnaire)
    {
      var dto = new Result.Questionnaire();

      dto.Identity = questionnaire.Identity;
      dto.Name = questionnaire.Name;
      dto.Slides = questionnaire.Slides;

      return dto;
    }
  }
}
