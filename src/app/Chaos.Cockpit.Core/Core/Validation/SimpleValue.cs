namespace Chaos.Cockpit.Core.Core.Validation
{
  public class SimpleValue
  {
    public SimpleValue(string key, string value)
    {
      Key = key;
      Value = value;
    }

    public string Key { get; set; }
    public string Value { get; set; }
  }
}