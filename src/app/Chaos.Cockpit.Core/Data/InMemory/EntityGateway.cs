namespace Chaos.Cockpit.Core.Data.InMemory
{
  using System;
  using System.Collections.Generic;

  public abstract class EntityGateway<T> where T : IEntiy
  {
    protected readonly IDictionary<string, T> Data = new Dictionary<string, T>();

    public T Save(T questionnaire)
    {
      var copy = Copy(questionnaire);

      var isNewEntity = string.IsNullOrEmpty(copy.Identity);
      if (isNewEntity)
        copy.Identity = Guid.NewGuid().ToString();

      if (Data.ContainsKey(copy.Identity))
        Data[copy.Identity] = copy;
      else
        Data.Add(copy.Identity, copy);

      return copy;
    }

    protected abstract T Copy(T entity);
  }
}