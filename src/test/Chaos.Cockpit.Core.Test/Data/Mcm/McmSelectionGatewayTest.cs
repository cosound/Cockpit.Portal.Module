using System;
using System.Xml.Linq;
using CHAOS.Serialization.Standard;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.Mcm;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Dto;
using Moq;
using NUnit.Framework;
using Object = Chaos.Mcm.Data.Dto.Object;

namespace Chaos.Cockpit.Core.Test.Data.Mcm
{
  [TestFixture]
  public class McmSelectionGatewayTest
  {
    private Mock<IMcmRepository> Mcm;

    [SetUp]
    public void SetUp()
    {
      Mcm = new Mock<IMcmRepository>();
    }

    [Test, ExpectedException(ExpectedExceptionName = "Chaos.Cockpit.Core.Core.Exceptions.DataNotFoundException")]
    public void Get_SelectionDoesntExist_Throw()
    {
      var gateway = new McmSelectionGateway(Mcm.Object);

      gateway.Get("90000000-0000-0000-0000-000000000009");
    }

    [Test]
    public void Get_SelectionExist_GetFromMcmAndReturn()
    {
      var gateway = new McmSelectionGateway(Mcm.Object);
      var selectionObject = Make_SelectionObject();
      Mcm.Setup(m => m.ObjectGet(selectionObject.Guid, true, false, false, false, false)).Returns(selectionObject);

      var result = gateway.Get(selectionObject.Guid.ToString());

      Assert.That(result.Id, Is.EqualTo(selectionObject.Guid.ToString()));
      Assert.That(result.Name, Is.EqualTo("name"));
      Assert.That(result.Items, Is.Not.Empty);
    }

    [Test, ExpectedException(ExpectedExceptionName = "Chaos.Cockpit.Core.Core.Exceptions.DataNotFoundException")]
    public void Set_SelectionDoesntExist_Throw()
    {
      var gateway = new McmSelectionGateway(Mcm.Object);

      gateway.Set(new Selection { Id = "90000000-0000-0000-0000-000000000009" });
    }

    [Test]
    public void Set_SelectionExist_UpdateInMcm()
    {
      var gateway = new McmSelectionGateway(Mcm.Object);
      var selectionObject = Make_SelectionObject();
      Mcm.Setup(m => m.ObjectGet(selectionObject.Guid, true, false, false, false, false)).Returns(selectionObject);

      var result  = gateway.Set(new Selection { Id = "10000000-0000-0000-0000-000000000001", Name = "new name"});
     
      Assert.That(result.Id, Is.EqualTo(selectionObject.Guid.ToString()));
      Assert.That(result.Name, Is.EqualTo("new name"));
      Mcm.Verify(m => m.MetadataSet(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), null, 0, It.IsAny<XDocument>(), It.IsAny<Guid>()));
    }

    [Test]
    public void Set_ItemsIsNull_DontUpdateItemsInMcm()
    {
      var gateway = new McmSelectionGateway(Mcm.Object);
      var selectionObject = Make_SelectionObject();
      Mcm.Setup(m => m.ObjectGet(selectionObject.Guid, true, false, false, false, false)).Returns(selectionObject);

      var result = gateway.Set(new Selection { Id = "10000000-0000-0000-0000-000000000001", Name = "new name", Items = null});

      Assert.That(result.Id, Is.EqualTo(selectionObject.Guid.ToString()));
      Assert.That(result.Name, Is.EqualTo("new name"));
      Assert.That(result.Items, Is.Not.Empty);
      Mcm.Verify(m => m.MetadataSet(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), null, 0, It.IsAny<XDocument>(), It.IsAny<Guid>()));
    }

    [Test]
    public void Set_NewSelection_CreateInMcm()
    {
      var gateway = new McmSelectionGateway(Mcm.Object);

      var result = gateway.Set(new Selection { Name = "name"});

      Assert.That(result.Id, Is.Not.Null);
      Assert.That(result.Name, Is.EqualTo("name"));
      Mcm.Verify(m => m.ObjectCreate(It.IsAny<Guid>(), It.IsAny<uint>(), It.IsAny<uint>()));
      Mcm.Verify(m => m.MetadataSet(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), null, 0, It.IsAny<XDocument>(), It.IsAny<Guid>()));
    }

    [Test]
    public void Delete_SelectionDoesExist_DeleteOnMcm()
    {
      var gateway = new McmSelectionGateway(Mcm.Object);

      gateway.Delete("10000000-0000-0000-0000-000000000001");

      Mcm.Verify(m => m.ObjectDelete(It.IsAny<Guid>()));
    }

    private static Object Make_SelectionObject()
    {
      return new Object
        {
          Guid = Guid.Parse("10000000-0000-0000-0000-000000000001"),
          Metadatas = new[]
            {
              new Metadata
                {
                  MetadataSchemaGuid = Context.Config.SelectionMetadataSchemaId,
                  MetadataXml = SerializerFactory.XMLSerializer.Serialize(new Selection
                    {
                      Id = "10000000-0000-0000-0000-000000000001",
                      Name = "name",
                      Items = new []
                        {
                          new Item
                            {
                              Id = "10000001-1001-1001-1001-100000000001"
                            }
                        }
                    })
                }
            }
        };
    }
  }
}