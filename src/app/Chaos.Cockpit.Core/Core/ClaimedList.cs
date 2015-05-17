using System;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Portal.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core
{
  public class ClaimedList : IKey
  {
    public string Id { get; set; }
    public XDocument Xml { get; set; }

    public ClaimedList(string id, XDocument xml)
    {
      Id = id;
      Xml = xml;
    }

    public ExperimentResult Next()
    {
      foreach (var experiment in Xml.Root.Elements("Item"))
      {
        if (experiment.Attribute("ClaimedOnDate").Value == "")
        {
          var datetime = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

          experiment.Attribute("ClaimedOnDate").Value = datetime;
          
          return new ExperimentResult
          {
            Id = experiment.Attribute("Id").Value,
            ClaimedOnDate = datetime
          };
        }
      }

      throw new ServerException("No more unclaimed experiments", "No Experiments are currently available, sorry for the inconvinience.");
    }

    public XDocument ToXml()
    {
      return XDocument.Parse(Xml.ToString());
    }
  }
}