using System;
using System.Runtime.Serialization;
using Chaos.Portal.Core.Exceptions;

namespace Chaos.Cockpit.Core.Core.Exceptions
{
  public class DataNotFoundException : ClientException
  {
    public DataNotFoundException(string message, string userMessage) : base(message, userMessage)
    {
    }

    public DataNotFoundException()
    {
    }

    public DataNotFoundException(string message, string userMessage, Exception innerException) : base(message, userMessage, innerException)
    {
    }

    protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}