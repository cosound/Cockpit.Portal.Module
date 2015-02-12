using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Core.Validation;

namespace Chaos.Cockpit.Core.Api
{
  public class OutputMapper
  {
    public static Output Map(OutputDto arg)
    {
      var output = new Output();

      MapComplex(arg, output);
      MapSimple(arg, output);
      MapMulti(arg, output);

      return output;
    }

    private static void MapMulti(OutputDto answer, Output output)
    {
      foreach (var multiValueResult in answer.MultiValues)
      {
        var multipleValue = new MultiValue();
        var single = multiValueResult.Value as MultiSingleValueResult;
        var complex = multiValueResult.Value as MultiComplexValueResult;

        if (single != null)
          foreach (var value in single.SingleValues)
            multipleValue.SimpleValues.Add(value);

        if (complex != null)
          foreach (var value in complex.ComplexValues)
          {
            var complexValue = new ComplexValue();
            foreach (var singleValue in value.SingleValues)
              complexValue.SimpleValues.Add(new SimpleValue(singleValue.Key, singleValue.Value));

            multipleValue.ComplexValues.Add(complexValue);
          }

        multipleValue.Key = multiValueResult.Key;
        output.MultiValues.Add(multipleValue);
      }
    }

    private static void MapSimple(OutputDto answer, Output output)
    {
      foreach (var singleResult in answer.SingleValues)
        output.SimpleValues.Add(new SimpleValue(singleResult.Key, singleResult.Value));
    }

    private static void MapComplex(OutputDto answer, Output output)
    {
      foreach (var complexResult in answer.ComplexValues)
      {
        var item = new ComplexValue();
        item.Key = complexResult.Key;

        foreach (var singleResult in complexResult.Value.SingleValues)
          item.Add(new SimpleValue(singleResult.Key, singleResult.Value));

        output.ComplexValues.Add(item);
      }
    }

    public static OutputDto Map(Output arg)
    {
      var output = new OutputDto();

      foreach (var value in arg.SimpleValues)
        output.SingleValues.Add(value.Key, value.Value);

      foreach (var value in arg.ComplexValues)
      {
        var vals = new ComplexValueResult();

        foreach (var simpleValue in value.SimpleValues)
          vals.SingleValues.Add(simpleValue.Key, simpleValue.Value);

        output.ComplexValues.Add(value.Key, vals);
      }

      foreach (var value in arg.MultiValues)
      {
        if (value.SimpleValues.Count > 0)
        {
          var multi = new MultiSingleValueResult();
        
          foreach (var simpleValue in value.SimpleValues)
            multi.SingleValues.Add(simpleValue);

          output.MultiValues.Add(value.Key, multi);
        }

        if (value.ComplexValues.Count > 0)
        {
          var multi = new MultiComplexValueResult();

          foreach (var complexValue in value.ComplexValues)
          {
            var complexValueResult = new ComplexValueResult();

            foreach (var simpleValue in complexValue.SimpleValues)
              complexValueResult.SingleValues.Add(simpleValue.Key, simpleValue.Value);

            multi.ComplexValues.Add(complexValueResult);
          }

          output.MultiValues.Add(value.Key, multi);
        }
      }

      return output;
    }
  }
}