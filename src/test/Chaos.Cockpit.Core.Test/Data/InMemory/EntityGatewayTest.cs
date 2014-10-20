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

       var actual = gateway.Save(entity);

       Assert.That(gateway.WasCopied, Is.True);
       Assert.That(actual.Identifier, Is.Not.EqualTo(entity.Identifier));
     }

     public class DummyEntityGateway : EntityGateway<DummyEntityGateway.DummyEntity>
     {
       protected override DummyEntity Copy(DummyEntity entity)
       {
         WasCopied = true;

         return new DummyEntity();
       }

       public bool WasCopied { get; set; }

       public class DummyEntity : IEntity
       {
         public string Identifier { get; set; }
       }
     }
  }
}