using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Api
{
  public static class AnswerResultMapper
  {
    public static AnswerResult Map(Answer answer)
     {
       return new AnswerResult
         {
           Identifier = answer.Identifier
         };
     }

    public static Answer Map(AnswerResult answer)
     {
       return new Answer
         {
           Identifier = answer.Identifier
         };
     }
  }
}