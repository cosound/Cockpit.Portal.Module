using System;
using Chaos.Cockpit.Core.Core;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;
using Chaos.Portal.v5.Extension.Result;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  public class SlideExtension : AExtension
  {
    public SlideExtension(IPortalApplication portalApplication) : base(portalApplication) {}

    public EndpointResult Completed(Guid questionaireId, int slideIndex)
    {
      var questionaire = Context.QuestionnaireGateway.Get(questionaireId);
      var slide = questionaire.Slides[slideIndex];
      
      slide.Validate();
      slide.IsCompleted = true;

      Context.QuestionnaireGateway.Set(questionaire);

      return EndpointResult.Success();
    }
  }
}
