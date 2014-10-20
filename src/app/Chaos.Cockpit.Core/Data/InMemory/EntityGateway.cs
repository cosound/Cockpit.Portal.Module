namespace Chaos.Cockpit.Core.Data.InMemory
{
  using System;
  using System.Collections.Generic;

  public abstract class EntityGateway<T> : IEntityGateway<T> where T : IEntity
  {
    protected readonly IDictionary<string, T> Data = new Dictionary<string, T>();

    public T Save(T entity)
    {
      var copy = Copy(entity);

      var isNewEntity = string.IsNullOrEmpty(copy.Identifier);
      if (isNewEntity)
        copy.Identifier = Guid.NewGuid().ToString();

      if (Data.ContainsKey(copy.Identifier))
        Data[copy.Identifier] = copy;
      else
        Data.Add(copy.Identifier, copy);

      return copy;
    }

    protected abstract T Copy(T entity);
  }
}