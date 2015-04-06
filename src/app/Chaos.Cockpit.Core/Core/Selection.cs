using System.Collections.Generic;

namespace Chaos.Cockpit.Core.Core
{
  public class Selection : IKey
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public IList<Item> Items { get; set; }

    public Selection()
    {
      Items = new List<Item>();
    }
  }

  public class Item
  {
    public string Identifier { get; set; }
  }
}