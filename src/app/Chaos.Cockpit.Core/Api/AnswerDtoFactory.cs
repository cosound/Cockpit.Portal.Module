namespace Chaos.Cockpit.Core.Api
{
  using System;
  using Core;
  using Result;

  public static class AnswerDtoFactory
  {
    public static AnswerDto Map(Answer answer)
    {
      if ("BooleanAnswer, 1.0" == answer.Type)
      {
        var dto = AnswerDto.CreateBooleanAnswer();
        dto.Identifier = answer.Identifier;
        dto.Data = answer.Data;

        return dto;
      }

      if ("MultipleChoiceAnswer, 1.0" == answer.Type)
      {
        var dto = AnswerDto.CreateMultipleChoiceAnswer();
        dto.Identifier = answer.Identifier;
        dto.Data = answer.Data;

        return dto;
      }

      throw new Exception("Unsupported Answer Type");
    }

    public static Answer Map(AnswerDto answer)
    {
      if ("BooleanAnswer, 1.0" == answer.Type)
      {
        var booleanAnswer = Answer.CreateBooleanAnswer();
        booleanAnswer.Identifier = answer.Identifier;
        booleanAnswer.Data = answer.Data;

        return booleanAnswer;
      }

      if ("MultipleChoiceAnswer, 1.0" == answer.Type)
      {
        var multipleAnswer = Answer.CreateMultipleChoiceAnswer();
        multipleAnswer.Identifier = answer.Identifier;
        multipleAnswer.Data = answer.Data;

        return multipleAnswer;
      }

      throw new Exception("Unsupported Answer Dto Type");
     }
  }
}