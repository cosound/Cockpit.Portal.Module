using System;
using System.Runtime.Serialization;

namespace Chaos.Cockpit.Core.Core.Exceptions
{
  public class SlideLockedException : Portal.Core.Exceptions.ClientException
  {
    public SlideLockedException()
    {
    }

    public SlideLockedException(string message, string userMessage) : base(message, userMessage)
    {
    }

    public SlideLockedException(string message, string userMessage, Exception innerException) : base(message, userMessage, innerException)
    {
    }

    protected SlideLockedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
