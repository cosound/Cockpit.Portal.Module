using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Core;
using Chaos.Cockpit.Core.Data.Exceptions;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Data.InMemory
{
  public class EntityRepository<T> where T : IKey
  {
    protected readonly IDictionary<string, string> DataStore = new Dictionary<string, string>();

    public T Set(T asset)
    {
      if (asset.Id == null)
        asset.Id = Guid.NewGuid().ToString();

      var json = JsonConvert.SerializeObject(asset);

      if (DataStore.ContainsKey(asset.Id))
        DataStore[asset.Id] = json;
      else
        DataStore.Add(asset.Id, json);

      return asset;
    }

    public T Retrieve(string id)
    {
      if (id == null || !DataStore.ContainsKey(id))
        throw new DataNotFoundException(typeof(T).Name + " not found, index: " + id);

      var json = DataStore[id];

      return Deserialize(json);
    }

    public IEnumerable<T> Retrieve()
    {
      return DataStore.Values.Select(Deserialize);
    } 

    public static T Deserialize(string json)
    {
      return JsonConvert.DeserializeObject<T>(json);
    }

    public bool Delete(string id)
    {
      if (id == null || !DataStore.ContainsKey(id))
        return false;

      DataStore.Remove(id);

      return true;
    }
  }
}
