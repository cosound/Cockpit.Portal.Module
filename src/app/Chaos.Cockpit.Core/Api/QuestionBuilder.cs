using Chaos.Cockpit.Core.Api.Result;

namespace Chaos.Cockpit.Core.Api
{
  public static class QuestionBuilder
  {
    public static QuestionDto MakeDto(Core.Question question)
    {
      return new QuestionDto(question.Type)
        {
          Identifier = question.Id,
          UserAnswer = question.UserAnswer == null
                         ? null
                         : AnswerDtoFactory.Map(question.UserAnswer),
          Input = question.Input,
          Output = question.Output == null ? null : OutputMapper.Map(question.Output)
        };
    }
  }
}