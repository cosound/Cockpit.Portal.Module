namespace Chaos.Cockpit.Core.Data.Mcm
{
  using System.Linq;
  using System.Xml.Linq;
  using Core;

  public class DtuFormatConverter
  {
    public Questionnaire Deserialize(string xml)
    {
      var doc = XDocument.Parse(xml);
      var experiemnt = doc.Root.Element("Experiment");
      var trial = doc.Descendants("Trial").Single();
      
      var result = new Questionnaire();
      result.Id = trial.Element("GUID").Value;
      result.Name = experiemnt.Element("Id").Value;

      return result;
    }
  }
}