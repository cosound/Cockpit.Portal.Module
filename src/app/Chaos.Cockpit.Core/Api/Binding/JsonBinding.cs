namespace Chaos.Cockpit.Core.Api.Binding
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;
  using Newtonsoft.Json;
  using Portal.Core.Bindings;

  // TODO: Promote this class to be part of Portal
  public class JsonBinding<T> : IParameterBinding
  {
    public object Bind(IDictionary<string, string> parameters, ParameterInfo parameterInfo)
    {
      var name = parameterInfo.Name;

      if(!parameters.ContainsKey(name))
        throw new ArgumentException("Parameter not given", parameterInfo.Name);

      return JsonConvert.DeserializeObject<T>(parameters[name]);
    }
  }
}
