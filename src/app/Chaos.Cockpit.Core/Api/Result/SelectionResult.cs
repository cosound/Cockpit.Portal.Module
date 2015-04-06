using System.Collections.Generic;
using CHAOS.Serialization;
using Chaos.Portal.Core.Data.Model;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Api.Result
{
  public class SelectionResult : AResult
  {
    [Serialize("Id"), JsonProperty("Id")]
    public string Identifier { get; set; }

    [Serialize]
    public string Name { get; set; }

    [Serialize]
    public IList<Item> Items { get; set; }

    public SelectionResult()
    {
      Items = new List<Item>();
    }
  }

  public class Item
  {
    [Serialize("Id"), JsonProperty("Id")]
    public string Identifier { get; set; }
  }
}