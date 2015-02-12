using System;
using System.Collections.Generic;
using System.Reflection;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Portal.Core.Bindings;
using Newtonsoft.Json;

namespace Chaos.Cockpit.Core.Api.Binding
{
  public class OutputBinding : IParameterBinding
  {
    public object Bind(IDictionary<string, string> parameters, ParameterInfo parameterInfo)
    {
      var name = parameterInfo.Name;

      if (!parameters.ContainsKey(name))
        throw new ArgumentException("Parameter not given", parameterInfo.Name);

      return JsonConvert.DeserializeObject<OutputDto>(parameters[name], new OutputConverter());
    }
  }
}