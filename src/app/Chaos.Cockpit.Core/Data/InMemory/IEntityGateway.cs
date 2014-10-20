namespace Chaos.Cockpit.Core.Data.InMemory
{
  public interface IEntityGateway<T>
  {
    T Save(T entity);
  }
}