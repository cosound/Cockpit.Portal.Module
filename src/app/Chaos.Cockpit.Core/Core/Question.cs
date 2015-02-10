using System.Xml.Linq;
using Chaos.Cockpit.Core.Api.Result;

namespace Chaos.Cockpit.Core.Core
{
  using System.Collections.Generic;

  public class Question : IKey
  {
    public string Id { get; set; }
    public Answer UserAnswer { get; set; }
    public string Type { get; set; }
    public IEnumerable<XElement> Input { get; set; }
    public Output Output { get; set; }

    public Question(string type)
    {
      Type = type;
      Input = new List<XElement>();
      Output = new Output();
    }
  }
}
