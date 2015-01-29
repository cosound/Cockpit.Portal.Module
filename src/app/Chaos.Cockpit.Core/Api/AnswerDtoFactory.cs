namespace Chaos.Cockpit.Core.Api
{
  using Core;
  using Result;

  public static class AnswerDtoFactory
  {
    public static AnswerDto Map(Answer answer)
    {
      return new AnswerDto(answer.Type)
        {
          Identifier = answer.Id,
          Data = answer.Data
        };
    }

    public static Answer Map(AnswerDto answer)
    {
      return new Answer(answer.Type)
      {
        Id = answer.Identifier,
        Data = answer.Data
      };
     }
  }
}