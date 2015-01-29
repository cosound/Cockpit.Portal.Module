using Chaos.Cockpit.Core.Core;

namespace Chaos.Cockpit.Core.Test.Data.InMemory
{
  using Cockpit.Core.Data.InMemory;
  using NUnit.Framework;

  [TestFixture]
  public class EntityGatewayTest
  {
     [Test]
     public void Save_GivenNewEntity_SaveCopyAndGiveIdentifier()
     {
       var entity = new DummyEntityGateway.DummyEntity();
       var gateway = new DummyEntityGateway();

       var actual = gateway.Set(entity);

       Assert.That(actual.Id, Is.Not.Null);
     }

     public class DummyEntityGateway : EntityRepository<DummyEntityGateway.DummyEntity>
     {
       public bool WasCopied { get; set; }

       public class DummyEntity : IKey
       {
         public string Id { get; set; }
       }
     }
  }
}